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
using System.Runtime.Serialization;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Text;

namespace mockup
{
    public partial class ModulePage : PhoneApplicationPage
    {
        private int moduleIndex;
        private Module selectedModule;
        public ObservableCollection<Folder> Folders;

        public ModulePage()
        {
            InitializeComponent();

            // downloading data for workbin page
            try
            {
                Folders = new ObservableCollection<Folder>(selectedModule.moduleWorkbins[0].workbinFolders);
            }
            catch
            {
                Folders = new ObservableCollection<Folder>();
            }
        }
        
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            // loading existing data for info page and announcement page
            base.OnNavigatedTo(e);

            string index = "";

            if (NavigationContext.QueryString.TryGetValue("moduleIndex", out index))
            {
                moduleIndex = Convert.ToInt32(index);
            }

            if ((Application.Current as App).online.Equals(false))
            {
                selectedModule = (Application.Current as App).modules[moduleIndex];

                foreach (Announcement at in selectedModule.moduleAnnouncements)
                {
                    at.GenerateDisplayContent(selectedModule.moduleCode);
                }

                // change the context to fit the selected module
                ModuleCodeTitle.Text = selectedModule.moduleCode;
                ModuleCode.Text = selectedModule.moduleCode;
                ModuleTitle.Text = selectedModule.moduleName;
                Semester.Text = selectedModule.moduleSemester;
                ModuleCredits.Text = selectedModule.moduleMc;
                Department.Text = selectedModule.moduleDepart;

                Facilitators.ItemsSource = selectedModule.moduleLecturers;

                AnnounceInfo.ItemsSource = selectedModule.moduleAnnouncements;

                WorkbinInfo.ItemsSource = Folders;
            }
            else
            {
                // initiate local variables with global variables
                selectedModule = (Application.Current as App).modules[moduleIndex];

                // change the context to fit the selected module
                ModuleCodeTitle.Text = selectedModule.moduleCode;
                ModuleCode.Text = selectedModule.moduleCode;
                ModuleTitle.Text = selectedModule.moduleName;
                Semester.Text = selectedModule.moduleSemester;
                ModuleCredits.Text = selectedModule.moduleMc;
                Department.Text = selectedModule.moduleDepart;

                Facilitators.ItemsSource = selectedModule.moduleLecturers;

                AnnounceInfo.ItemsSource = selectedModule.moduleAnnouncements;

                WorkbinInfo.ItemsSource = Folders;

                if (Folders.Count == 0)
                {
                    getWorkbinData();
                }
            }     
        }

        private void getWorkbinData()
        {
            string[] paramNames = new string[] { "CourseId", "Duration", "WorkbinID", "TitleOnly" };
            if (selectedModule.moduleWorkbins.Length > 0)
            {
                string[] paramValues = new string[] { selectedModule.moduleId, "0", selectedModule.moduleWorkbins[0].workbinID, "false" };
                string URL = LAPI.requestURL("Workbins", paramNames, paramValues);
                GetFeed(URL);
            }
        }

        public void GetFeed(string url)
        {
            HttpWebRequest request = HttpWebRequest.CreateHttp(url);
            request.BeginGetResponse(new AsyncCallback(HandleResponse), request);
        }

        // handle the response and get the json string
        public void HandleResponse(IAsyncResult result)
        {
            HttpWebRequest request = result.AsyncState as HttpWebRequest;

            if (request != null)
            {
                using (WebResponse response = request.EndGetResponse(result))
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        string feed = reader.ReadToEnd();

                        WorkbinWrapper test = (WorkbinWrapper)Deserialize(feed, typeof(WorkbinWrapper));

                        if (test.workbins.Length > 0)
                        {
                            Deployment.Current.Dispatcher.BeginInvoke(() =>
                            {
                                foreach (Folder folder in test.workbins[0].workbinFolders)
                                {
                                    folder.recordModuleCode(selectedModule.moduleCode);
                                    Folders.Add(folder);
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

        public static string Serialize(Object input, Type objectType)
        {
            StringBuilder temp = new StringBuilder();

            if (objectType == null || input == null)
                return null;

            JsonSerializer ser = new JsonSerializer();
            JsonWriter jw = new JsonTextWriter(new StringWriter(temp));
            ser.Serialize(jw, input);
            jw.Flush();

            return temp.ToString();
        }

        private void ViewAnnouncement(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (AnnounceInfo.SelectedIndex != -1)
            {
                int announceIndex = AnnounceInfo.SelectedIndex;

                (Application.Current as App).announcement = selectedModule.moduleAnnouncements[announceIndex];

                NavigationService.Navigate(new Uri(("/AnnouncementPage.xaml"), UriKind.Relative));
            }
        }

        private void ViewWorkbin(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (WorkbinInfo.SelectedIndex != -1)
            {
                int folderIndex = WorkbinInfo.SelectedIndex;

                (Application.Current as App).folders.Add(Folders[folderIndex]);

                NavigationService.Navigate(new Uri(("/WorkbinPage.xaml"), UriKind.Relative));
            }
        }

        private void AddAsToDo(object sender, System.Windows.Input.GestureEventArgs e)
        {
            ListBoxItem selectedListBoxItem = this.AnnounceInfo.ItemContainerGenerator.ContainerFromItem((sender as MenuItem).DataContext) as ListBoxItem;

            int announceIndex = this.AnnounceInfo.ItemContainerGenerator.IndexFromContainer(selectedListBoxItem);

            (Application.Current as App).announcement = selectedModule.moduleAnnouncements[announceIndex];

            NavigationService.Navigate(new Uri(("/AddToDoPage.xaml"), UriKind.Relative));
        }
    }
}