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
using System.Text;
using Newtonsoft.Json;
using System.Threading;
using System.ComponentModel;
using System.IO.IsolatedStorage;
using Microsoft.Phone.Shell;
using System.Net.NetworkInformation;

namespace mockup
{
    public partial class MainPage : PhoneApplicationPage
    {
        private string postString;
        private string username;
        private string password;
        private string domain;
        private string[] domains = { "NUSSTU", "NUSSTF", "NUSEXT", "GUEST" };

         

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            LoadCredentials();

            // if token exists in isolated storage
            if (LoadToken())
            {
                if (!NetworkInterface.GetIsNetworkAvailable())
                {
                    MessageBox.Show("No internet connection is available. Make sure you have either Wi-Fi or data connection.");
                }
                else
                {
                    DisableControls();
                    loginProgressBar.IsIndeterminate = true;
                    UpdateToken();
                }
            }

            // upload application tile with the latest to-dos
            (Application.Current as App).UpdateAppTile();
        }

        // load token from isolated storage if existing
        // return false if not
        private bool LoadToken()
        {
            var settings = IsolatedStorageSettings.ApplicationSettings;

            if (settings.Contains("token"))
            {
                LAPI.token = settings["token"].ToString();
                return true;
            }
            else
            {
                return false;
            }
        }

        // validate token and update it if 
        // expiration is soon
        private void UpdateToken()
        {
            HttpWebRequest request = HttpWebRequest.CreateHttp(LAPI.validateTokenURL());
            request.BeginGetResponse(new AsyncCallback(HandleTokenResponse), request);
        }

        // disable all controls
        private void DisableControls()
        {
            UsernameInput.IsEnabled = false;
            PasswordInput.IsEnabled = false;
            DomainInput.IsEnabled = false;
        }

        // enable all controls
        private void EnableControls()
        {
            UsernameInput.IsEnabled = true;
            PasswordInput.IsEnabled = true;
            DomainInput.IsEnabled = true;
        }

        // handle the response and get the json string
        public void HandleTokenResponse(IAsyncResult result)
        {
            HttpWebRequest request = (HttpWebRequest)result.AsyncState;
            HttpWebResponse response;

            try
            {
                response = (HttpWebResponse)request.EndGetResponse(result);
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    // returned JSON string for validation
                    string responseString = reader.ReadToEnd();

                    Token newToken = (Token)Deserialize(responseString, typeof(Token));

                    if (newToken != null && newToken.TokenSuccess.Equals(true))
                    {
                        LAPI.token = newToken.TokenContent;

                        Dispatcher.BeginInvoke(() =>
                        {
                            (Application.Current as App).online = true;

                            SaveCredentials();
                            SaveToken();
                            EnableControls();
                            loginProgressBar.IsIndeterminate = false;
                            NavigationService.Navigate(new Uri(("/MenuPage.xaml"), UriKind.Relative));
                        });
                    }
                    else
                    {
                        Dispatcher.BeginInvoke(() =>
                        {
                            EnableControls();
                            loginProgressBar.IsIndeterminate = false;
                        });
                    }
                }
            }
            catch (WebException ex)
            {
            }
        }

        // log in event by pressing the log in button
        private void login_Click(object sender, RoutedEventArgs e)
        {
            if (!NetworkInterface.GetIsNetworkAvailable())
            {
                MessageBox.Show("No internet connection is available. Make sure you have either Wi-Fi or data connection.");
            }
            else
            {
                loginProgressBar.IsIndeterminate = true;

                // getting user credentials from input
                username = UsernameInput.Text;
                password = PasswordInput.Password;

                // cast the selected listpickeritem and extract the content
                domain = domains[DomainInput.SelectedIndex];

                postString = LAPI.GeneratePostString(username, password, domain);

                string authenticationURL = "https://ivle.nus.edu.sg/api/Lapi.svc/Login_JSON";

                // Create a new HttpWebRequest object.
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(authenticationURL);

                // Set the Method property to 'POST' to post data to the URI.
                request.Method = "POST";

                request.ContentType = "application/x-www-form-urlencoded";

                request.BeginGetRequestStream(new AsyncCallback(GetRequestStreamCallback), request);
            }
        }

        private void Login()
        {
            if (!NetworkInterface.GetIsNetworkAvailable())
            {
                MessageBox.Show("No internet connection is available. Make sure you have either Wi-Fi or data connection.");
            }
            else
            {
                loginProgressBar.IsIndeterminate = true;

                // getting user credentials from input
                username = UsernameInput.Text;
                password = PasswordInput.Password;

                // cast the selected listpickeritem and extract the content
                domain = domains[DomainInput.SelectedIndex];

                postString = LAPI.GeneratePostString(username, password, domain);

                string authenticationURL = "https://ivle.nus.edu.sg/api/Lapi.svc/Login_JSON";

                // Create a new HttpWebRequest object.
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(authenticationURL);

                // Set the Method property to 'POST' to post data to the URI.
                request.Method = "POST";

                request.ContentType = "application/x-www-form-urlencoded";

                request.BeginGetRequestStream(new AsyncCallback(GetRequestStreamCallback), request);
            }
        }

        public void GetRequestStreamCallback(IAsyncResult asynchronousResult)
        {
            HttpWebRequest request = (HttpWebRequest)asynchronousResult.AsyncState;

            Stream postStream = request.EndGetRequestStream(asynchronousResult);

            byte[] byteArray = Encoding.UTF8.GetBytes(postString);

            postStream.Write(byteArray, 0, postString.Length);
            postStream.Close();

            request.BeginGetResponse(new AsyncCallback(GetResponseCallback), request);
        }

        private void GetResponseCallback(IAsyncResult asynchronousResult)
        {
            HttpWebRequest request = (HttpWebRequest)asynchronousResult.AsyncState;
            HttpWebResponse response;

            try
            {
                response = (HttpWebResponse)request.EndGetResponse(asynchronousResult);
                Stream streamResponse = response.GetResponseStream();
                StreamReader streamRead = new StreamReader(streamResponse);

                // returned JSON string for validation
                string responseString = streamRead.ReadToEnd();

                // remove the last "}"
                responseString = responseString.Remove(responseString.Length - 1);

                // remove the first "{" and its associated header
                responseString = responseString.Substring(responseString.IndexOf(":") + 1);
                Token token = (Token)Deserialize(responseString, typeof(Token));

                if (token != null && token.TokenSuccess.Equals(true))
                {
                    LAPI.token = token.TokenContent;

                    Dispatcher.BeginInvoke(() =>
                    {
                        (Application.Current as App).online = true;

                        SaveCredentials();
                        SaveToken();
                        loginProgressBar.IsIndeterminate = false;
                        NavigationService.Navigate(new Uri(("/MenuPage.xaml"), UriKind.Relative));
                    });
                }
                else
                {
                    Dispatcher.BeginInvoke(() =>
                    {
                        loginProgressBar.IsIndeterminate = false;

                        MessageBox.Show("Log in failed");
                    });
                }

                // Close the stream object
                streamResponse.Close();
                streamRead.Close();

                // Release the HttpWebResponse
                response.Close();
            }
            catch(WebException ex)
            {
                if (ex.Status == WebExceptionStatus.RequestCanceled)
                {
                    Dispatcher.BeginInvoke(() =>
                    {
                        Login();
                    });
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

        private void SaveCredentials()
        {
            var settings = IsolatedStorageSettings.ApplicationSettings;

            if (settings.Contains("username"))
            {
                settings["username"] = username;
            }
            else
            {
                settings.Add("username", username);
            }

            if (settings.Contains("password"))
            {
                settings["password"] = password;
            }
            else
            {
                settings.Add("password", password);
            }

            if (settings.Contains("domain"))
            {
                settings["domain"] = domain;
            }
            else
            {
                settings.Add("domain", domain);
            }

            settings.Save();
        }

        // save token into isolated storage
        private void SaveToken()
        {
            var settings = IsolatedStorageSettings.ApplicationSettings;

            if (settings.Contains("token"))
            {
                settings["token"] = LAPI.token;
            }
            else
            {
                settings.Add("token", LAPI.token);
            }

            settings.Save();
        }

        private void LoadCredentials()
        {
            // if credential is memorized before
            // load credentials
            var settings = IsolatedStorageSettings.ApplicationSettings;

            if (settings.Contains("username") && settings.Contains("password") && settings.Contains("domain"))
            {
                username = settings["username"].ToString();
                password = settings["password"].ToString();
                domain = settings["domain"].ToString();

                DomainInput.SelectedItem = DomainInput.FindName(domain);
                UsernameInput.Text = username;
                PasswordInput.Password = password;
            }
        }

        //public void UpdateApplicationTile()
        //{
        //    string backTitle = null;
        //    string backContent = null;

        //    if ((Application.Current as App).todos.Count > 0)
        //    {
        //        backTitle = (Application.Current as App).todos[0].todoName;

        //        if ((Application.Current as App).todos[0].todoDetail.Length > 50)
        //        {
        //            backContent = (Application.Current as App).todos[0].todoDetail.Substring(0,50);
        //        }
        //        else
        //        {
        //            backContent = (Application.Current as App).todos[0].todoDetail;
        //        }
        //    }

        //    var appTile = ShellTile.ActiveTiles.First();

        //    if(appTile != null)
        //    {
        //        var standardTile = new StandardTileData
        //        {
        //            Title = "IVLE Metro",
        //            BackTitle = backTitle,
        //            BackContent = backContent,
        //            BackgroundImage = new Uri("NUS_Logo.png", UriKind.Relative),
        //            BackBackgroundImage = null
        //        };

        //        appTile.Update(standardTile);
        //    }
        //}

        private void offlineMode_Click(object sender, RoutedEventArgs e)
        {
            // navigate to panaroma menu page immediately without downloading data
            (Application.Current as App).online = false;

            NavigationService.Navigate(new Uri(("/MenuPage.xaml"), UriKind.Relative));
        }
    }
}