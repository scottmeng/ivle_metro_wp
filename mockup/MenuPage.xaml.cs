using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.IO;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using Microsoft.Phone.Shell;

namespace mockup
{
    public partial class PanoramaPage1 : PhoneApplicationPage
    {
        // using observable collection for data binding
        public ObservableCollection<Module> Modules = new ObservableCollection<Module>();
        public ObservableCollection<Class> Classes = new ObservableCollection<Class>();
        public ObservableCollection<Todo> Todos = new ObservableCollection<Todo>();
        
        // to store academic year and semester information
        private List<SemInfo> Sems = new List<SemInfo>();
        private int numOfSems;

        // define OS version 7.10.8858
        private static Version TargetedVersion = new Version(7, 10, 8858);
        
        // check if the current OS version is sufficient
        public static bool IsTargetedVersion { 
            get { return Environment.OSVersion.Version >= TargetedVersion; } 
        }

        public PanoramaPage1()
        {
            InitializeComponent();

            if ((Application.Current as App).online)
            {
                loadModules();
            }
            else
            {
                // load modules information from memory
                Modules = new ObservableCollection<Module>((Application.Current as App).modules);

                foreach (Module module in Modules)
                {
                    foreach (Announcement announce in module.moduleAnnouncements)
                    {
                        announce.GenerateDisplayContent(module.moduleCode);
                    }
                }

                // load classes information from memory
                Classes = new ObservableCollection<Class>((Application.Current as App).classes);

                foreach (Class myClass in Classes)
                {
                    myClass.GenerateDisplay();
                }
            }

            // link view with model
            ModuleInfo.ItemsSource = Modules;
            ClassInfo.ItemsSource = Classes;
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (NavigationService.CanGoBack)
            {
                while (NavigationService.RemoveBackEntry() != null)
                {
                    NavigationService.RemoveBackEntry();
                }
            }

            // load to-do data from public variables
            // every time the menupage is reloaded
            // just in case user have modified the to-do list
            Todos = new ObservableCollection<Todo>((Application.Current as App).todos);
            TodoInfo.ItemsSource = Todos;
        }

        private void loadModules()
        {
            string[] paramNames = new string[] { "Duration", "IncludeAllInfo" };
            string[] paramValues = new string[] { "0", "true" };

            string URL = LAPI.requestURL("Modules", paramNames, paramValues);
            GetFeed(URL);
        }

        private void viewModule(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (ModuleInfo.SelectedIndex != -1)
            {
                int moduleIndex = ModuleInfo.SelectedIndex;

                NavigationService.Navigate(new Uri(("/ModulePage.xaml?moduleIndex=" + moduleIndex), UriKind.Relative));
            }
        }

        private void viewTodo(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (TodoInfo.SelectedIndex != -1)
            {
                int todoIndex = TodoInfo.SelectedIndex;

                NavigationService.Navigate(new Uri(("/TodoPage.xaml?todoIndex=" + todoIndex), UriKind.Relative));
            }
        }

        public void GetFeed(string url)
        {
            loadModuleProgressBar.IsIndeterminate = true;

            HttpWebRequest request = HttpWebRequest.CreateHttp(url);
            request.BeginGetResponse(new AsyncCallback(HandleResponse), request);
        }

        // handle the response and get the json string
        public void HandleResponse(IAsyncResult result)
        {
            HttpWebRequest request = (HttpWebRequest)result.AsyncState;
            HttpWebResponse response;

            try
            {
                response = (HttpWebResponse)request.EndGetResponse(result);
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    string feed = reader.ReadToEnd();

                    ModuleInfoWrapper test = (ModuleInfoWrapper)Deserialize(feed, typeof(ModuleInfoWrapper));

                    if (test.comments.Equals("Valid login!"))
                    {
                        Deployment.Current.Dispatcher.BeginInvoke(() =>
                        {
                            foreach (Module module in test.modules)
                            {
                                foreach (Announcement announce in module.moduleAnnouncements)
                                {
                                    announce.GenerateDisplayContent(module.moduleCode);
                                }
                                Modules.Add(module);

                                SemInfo newSemInfo = new SemInfo(module.moduleAcadYear, module.moduleSemester.Replace("Semester ", String.Empty));
                                if (!Sems.Contains(newSemInfo, new SeminfoEqualityComparer()))
                                {
                                    Sems.Add(newSemInfo);
                                }
                            }

                            // update existing tiles
                            UpdateAllModuleTile();

                            numOfSems = Sems.Count;

                            foreach (SemInfo sem in Sems)
                            {
                                string[] paramNames = new string[] { "AcadYear", "Semester" };
                                string[] paramValues = new string[] { sem.AcademicYear, sem.Semester };

                                string URL = LAPI.requestURL("Timetable_Student", paramNames, paramValues);

                                GetClass(URL);
                            }

                            (Application.Current as App).modules = Modules.ToList();
                        });
                    }
                }
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.RequestCanceled)
                {
                    Dispatcher.BeginInvoke(() =>
                    {
                        loadModules();
                    });
                }
            }
        }

        public void GetClass(string url)
        {
            HttpWebRequest request = HttpWebRequest.CreateHttp(url);
            request.BeginGetResponse(new AsyncCallback(HandleClassResponse), request);
        }

        // handle the response and get the json string
        public void HandleClassResponse(IAsyncResult result)
        {
            HttpWebRequest request = result.AsyncState as HttpWebRequest;

            if (request != null)
            {
                using (WebResponse response = request.EndGetResponse(result))
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        string feed = reader.ReadToEnd();

                        ClassWrapper test = (ClassWrapper)Deserialize(feed, typeof(ClassWrapper));

                        if (test.comments.Equals("Valid login!"))
                        {
                            Deployment.Current.Dispatcher.BeginInvoke(() =>
                            {
                                numOfSems--;

                                foreach (Class myClass in test.classes)
                                {
                                    myClass.GenerateDisplay();

                                    Classes.Add(myClass);
                                }

                                if (numOfSems == 0)
                                {
                                    // hide the progress bar
                                    loadModuleProgressBar.IsIndeterminate = false;

                                    Classes = new ObservableCollection<Class>(Classes.OrderBy(Class => Class.classTimePoint));

                                    ClassInfo.ItemsSource = Classes;

                                    (Application.Current as App).classes = Classes.ToList();
                                }                             
                            });
                        }
                    }
                }
            }
        }

        public static object Deserialize(String input, Type objectType)
        {
            if (objectType == null || input == null)
                return null;
                      
            JsonSerializer ser = new JsonSerializer();
            JsonReader jr = new JsonTextReader(new StringReader(input));
            return ser.Deserialize(jr, objectType);
        }

        private void PinToStart_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            ListBoxItem selectedListBoxItem = this.ModuleInfo.ItemContainerGenerator.ContainerFromItem((sender as MenuItem).DataContext) as ListBoxItem;

            int selectedModuleIndex = this.ModuleInfo.ItemContainerGenerator.IndexFromContainer(selectedListBoxItem);

            // generate a secondary tile

            string moduleTitle = Modules[selectedModuleIndex].moduleCode;

            if (Modules[selectedModuleIndex].moduleAnnouncements.Length > 0)
            {
                string moduleLatestAnnounceTitle = Modules[selectedModuleIndex].moduleAnnouncements[0].announceNameDisplay;
                string moduleLatestAnnounceContent = Modules[selectedModuleIndex].moduleAnnouncements[0].announceContentPreview;

                if (moduleLatestAnnounceContent.Length > 50)
                {
                    CreateModuleTile(selectedModuleIndex, moduleTitle, moduleLatestAnnounceTitle, moduleLatestAnnounceContent.Substring(0, 50));
                }
                else
                {
                    CreateModuleTile(selectedModuleIndex, moduleTitle, moduleLatestAnnounceTitle, moduleLatestAnnounceContent);
                }
            }
            else
            {
                CreateModuleTile(selectedModuleIndex, moduleTitle, null, null);
            }
        }

        private void CreateModuleTile(int moduleIndex, string title, string backTitle, string backContent)
        {
            // check for existing tiles
            var foundTile = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains("moduleIndex=" + moduleIndex));

            if (foundTile == null)
            {
                var moduleTile = new StandardTileData
                {
                    BackgroundImage = new Uri("IVLE_module.png", UriKind.Relative),
                    Title = title,
                    BackTitle = backTitle,
                    BackContent = backContent
                };

                ShellTile.Create(new Uri(("/ModulePage.xaml?moduleIndex=" + moduleIndex), UriKind.Relative), moduleTile);
            }
            else
            {
                MessageBox.Show("The module has been added to the start screen!");
            }
        }

        private void UpdateAllModuleTile()
        {
            for (int i = 0; i < Modules.Count; i++)
            {
                string moduleLatestAnnounceTitle = null;
                string moduleLatestAnnounceContent = null;
                string moduleTitle = Modules[i].moduleCode;

                if (Modules[i].moduleAnnouncements.Length > 0)
                {
                    moduleLatestAnnounceTitle = Modules[i].moduleAnnouncements[0].announceNameDisplay;
                    moduleLatestAnnounceContent = Modules[i].moduleAnnouncements[0].announceContentPreview;
                }

                UpdateModuleTile(i, moduleTitle, moduleLatestAnnounceTitle, moduleLatestAnnounceContent);
            }
        }

        private void UpdateModuleTile(int moduleIndex, string title, string backTitle, string backContent)
        {
            // check for existing tiles
            var foundTile = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains("moduleIndex=" + moduleIndex));

            if (foundTile != null)
            {
                if (backContent != null && backContent.Length > 50)
                {
                    backContent = backContent.Substring(0, 50);
                }

                var moduleTile = new StandardTileData
                {
                    BackgroundImage = new Uri("IVLE_module.png", UriKind.Relative),
                    Title = title,
                    BackTitle = backTitle,
                    BackContent = backContent
                };

                foundTile.Update(moduleTile);
            }
        }

        class SeminfoEqualityComparer : IEqualityComparer<SemInfo>
        {
            public bool Equals(SemInfo s1, SemInfo s2)
            {
                if (s1.Semester == s2.Semester && s1.AcademicYear == s2.AcademicYear)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            public int GetHashCode(SemInfo sem)
            {
                int hCode = sem.AcademicYear.Length ^ sem.Semester.Length;
                return hCode.GetHashCode();
            }
        }

        private void RemoveToDo(object sender, System.Windows.Input.GestureEventArgs e)
        {
            ListBoxItem selectedListBoxItem = this.TodoInfo.ItemContainerGenerator.ContainerFromItem((sender as MenuItem).DataContext) as ListBoxItem;

            int selectedTodoIndex = this.TodoInfo.ItemContainerGenerator.IndexFromContainer(selectedListBoxItem);

            (Application.Current as App).todos.RemoveAt(selectedTodoIndex);
            (Application.Current as App).UpdateAppTile();

            Todos.RemoveAt(selectedTodoIndex);
        }
    }
}