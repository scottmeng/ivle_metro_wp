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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace mockup
{
    public partial class App : Application
    {
        // define OS version 7.10.8858
        private static Version TargetedVersion = new Version(7, 10, 8858);

        // check if the current OS version is sufficient
        public static bool IsTargetedVersion
        {
            get { return Environment.OSVersion.Version >= TargetedVersion; }
        }

        // list of module information shown on the MenuPage
        public List<Module> modules { get; set;}

        // list of timetable information shown on the MenuPage
        public List<Class> classes { get; set; }

        // list of todo information shown on the MenuPage
        public List<Todo> todos { get; set; }

        // store the selected announcement data
        public Announcement announcement { get; set; }

        // store the selected folder data   
        public List<Folder> folders { get; set; }

        // boolean value indicating the working status of the application
        public bool online { get; set; }

        /// <summary>
        /// Provides easy access to the root frame of the Phone Application.
        /// </summary>
        /// <returns>The root frame of the Phone Application.</returns>
        public PhoneApplicationFrame RootFrame { get; private set; }

        /// <summary>
        /// Constructor for the Application object.
        /// </summary>
        public App()
        {
            // Global handler for uncaught exceptions. 
            UnhandledException += Application_UnhandledException;

            // Standard Silverlight initialization
            InitializeComponent();

            // Phone-specific initialization
            InitializePhoneApplication();

            // Show graphics profiling information while debugging.
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // Display the current frame rate counters.
                Application.Current.Host.Settings.EnableFrameRateCounter = false;

                // Show the areas of the app that are being redrawn in each frame.
                //Application.Current.Host.Settings.EnableRedrawRegions = true;

                // Enable non-production analysis visualization mode, 
                // which shows areas of a page that are handed off to GPU with a colored overlay.
                //Application.Current.Host.Settings.EnableCacheVisualization = true;

                // Disable the application idle detection by setting the UserIdleDetectionMode property of the
                // application's PhoneApplicationService object to Disabled.
                // Caution:- Use this under debug mode only. Application that disables user idle detection will continue to run
                // and consume battery power when the user is not using the phone.
                PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled;
            }

        }

        // Code to execute when the application is launching (eg, from Start)
        // This code will not execute when the application is reactivated
        private void Application_Launching(object sender, LaunchingEventArgs e)
        {
            folders = new List<Folder>();
            online = false;

            classes = new List<Class>();
            
            // restore to-do list from storage
            try
            {
                todos = (List<Todo>)IsoStoreHelper.LoadContent("todo", typeof(List<Todo>));
            }
            catch (Exception ex)
            {
                todos = new List<Todo>();
            }

            // restore modules from storage
            try
            {
                modules = (List<Module>)IsoStoreHelper.LoadContent("module", typeof(List<Module>));
            }
            catch (Exception ex)
            {
                modules = new List<Module>();
            }

            // restore class information from storage
            try
            {
                classes = (List<Class>)IsoStoreHelper.LoadContent("class", typeof(List<Class>));
            }
            catch (Exception ex)
            {
                classes = new List<Class>();
            }

        }

        public void SaveModule()
        {
            IsoStoreHelper.SaveContent("module", modules, typeof(List<Module>));
        }

        public void SaveTodo()
        {
            IsoStoreHelper.SaveContent("todo", todos, typeof(List<Todo>));
        }

        public void SaveClass()
        {
            IsoStoreHelper.SaveContent("class", classes, typeof(List<Class>));
        }


        public void UpdateAppTile()
        {
            string title = null;
            string content = null;
            var backgroundImage = new Uri("NUS_Logo.png", UriKind.Relative);
            var baseUri = new Uri("", UriKind.Relative);

            // update application tile
            if ((Application.Current as App).todos.Count > 0)
            {
                title = (Application.Current as App).todos[0].todoName;

                if ((Application.Current as App).todos[0].todoDetail.Length > 50)
                {
                    content = (Application.Current as App).todos[0].todoDetail.Substring(0, 50);
                }
                else
                {
                    content = (Application.Current as App).todos[0].todoDetail;
                }
            }

            // check if there is any active tiles
            var appTile = ShellTile.ActiveTiles.First();

            if (appTile != null)
            {
                var standardTile = new StandardTileData
                {
                    Title = "IVLE Metro",
                    BackTitle = title,
                    BackContent = content,
                    BackgroundImage = backgroundImage,
                    BackBackgroundImage = null
                };
                
                // update the first application tile (primary tile)
                appTile.Update(standardTile);
                UpdateFlipTile("IVLE Metro", title, content, content, 0, baseUri, backgroundImage, backgroundImage, null, backgroundImage, null); 
            }
        }

        public static void UpdateFlipTile(
            string title,
            string backTitle,
            string backContent,
            string wideBackContent,
            int count,
            Uri tileId,
            Uri smallBackgroundImage,
            Uri backgroundImage,
            Uri backBackgroundImage,
            Uri wideBackgroundImage,
            Uri wideBackBackgroundImage)
        {
            if (IsTargetedVersion)
            {
                // Get the new FlipTileData type.
                Type flipTileDataType = Type.GetType("Microsoft.Phone.Shell.FlipTileData, Microsoft.Phone");

                // Get the ShellTile type so we can call the new version of "Update" that takes the new Tile templates.
                Type shellTileType = Type.GetType("Microsoft.Phone.Shell.ShellTile, Microsoft.Phone");

                // Loop through any existing Tiles that are pinned to Start.
                foreach (var tileToUpdate in ShellTile.ActiveTiles)
                {
                    // Look for a match based on the Tile's NavigationUri (tileId).
                    //if (tileToUpdate.NavigationUri.ToString() == tileId.ToString())
                    //{
                        // Get the constructor for the new FlipTileData class and assign it to our variable to hold the Tile properties.
                        var UpdateTileData = flipTileDataType.GetConstructor(new Type[] { }).Invoke(null);

                        // Set the properties. 
                        SetProperty(UpdateTileData, "Title", title);
                        SetProperty(UpdateTileData, "Count", count);
                        SetProperty(UpdateTileData, "BackTitle", backTitle);
                        SetProperty(UpdateTileData, "BackContent", backContent);
                        SetProperty(UpdateTileData, "SmallBackgroundImage", smallBackgroundImage);
                        SetProperty(UpdateTileData, "BackgroundImage", backgroundImage);
                        SetProperty(UpdateTileData, "BackBackgroundImage", backBackgroundImage);
                        SetProperty(UpdateTileData, "WideBackgroundImage", wideBackgroundImage);
                        SetProperty(UpdateTileData, "WideBackBackgroundImage", wideBackBackgroundImage);
                        SetProperty(UpdateTileData, "WideBackContent", wideBackContent);

                        // Invoke the new version of ShellTile.Update.
                        shellTileType.GetMethod("Update").Invoke(tileToUpdate, new Object[] { UpdateTileData });
                        break;
                    //}
                }
            }

        }

        private static void SetProperty(object instance, string name, object value)
        {
            var setMethod = instance.GetType().GetProperty(name).GetSetMethod();
            setMethod.Invoke(instance, new object[] { value });
        }

        // Code to execute when the application is activated (brought to foreground)
        // This code will not execute when the application is first launched
        private void Application_Activated(object sender, ActivatedEventArgs e)
        {
        }

        // Code to execute when the application is deactivated (sent to background)
        // This code will not execute when the application is closing
        private void Application_Deactivated(object sender, DeactivatedEventArgs e)
        {
            SaveTodo();
            SaveModule();
            SaveClass();
        }

        // Code to execute when the application is closing (eg, user hit Back)
        // This code will not execute when the application is deactivated
        private void Application_Closing(object sender, ClosingEventArgs e)
        {
            SaveTodo();
            SaveModule();
            SaveClass();
        }

        // Code to execute if a navigation fails
        private void RootFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // A navigation has failed; break into the debugger
                System.Diagnostics.Debugger.Break();
            }
        }

        // Code to execute on Unhandled Exceptions
        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // An unhandled exception has occurred; break into the debugger
                System.Diagnostics.Debugger.Break();
            }
        }

        #region Phone application initialization

        // Avoid double-initialization
        private bool phoneApplicationInitialized = false;

        // Do not add any additional code to this method
        private void InitializePhoneApplication()
        {
            if (phoneApplicationInitialized)
                return;

            // Create the frame but don't set it as RootVisual yet; this allows the splash
            // screen to remain active until the application is ready to render.
            RootFrame = new PhoneApplicationFrame();
            RootFrame.Navigated += CompleteInitializePhoneApplication;

            // Handle navigation failures
            RootFrame.NavigationFailed += RootFrame_NavigationFailed;

            // Ensure we don't initialize again
            phoneApplicationInitialized = true;
        }

        // Do not add any additional code to this method
        private void CompleteInitializePhoneApplication(object sender, NavigationEventArgs e)
        {
            // Set the root visual to allow the application to render
            if (RootVisual != RootFrame)
                RootVisual = RootFrame;

            // Remove this handler since it is no longer needed
            RootFrame.Navigated -= CompleteInitializePhoneApplication;
        }

        #endregion
    }
}