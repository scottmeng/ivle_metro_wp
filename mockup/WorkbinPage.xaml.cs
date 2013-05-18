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
using Microsoft.Phone.Tasks;

namespace mockup
{
    public partial class WorkbinPage : PhoneApplicationPage
    {
        private Folder selectedFolder;

        public WorkbinPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            // loading existing data for info page and announcement page
            base.OnNavigatedTo(e);

            selectedFolder = (Application.Current as App).folders.Last();

            ModuleCode.Text = selectedFolder.folderModuleCode;

            FolderTitle.Text = selectedFolder.folderName;

            FolderInfo.ItemsSource = selectedFolder.folderInnerFolders;

            foreach (File file in selectedFolder.folderFiles)
            {
                file.GenerateDisplayContent();
            }

            FileInfo.ItemsSource = selectedFolder.folderFiles;
        }

        private void ViewFolder(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (FolderInfo.SelectedIndex != -1)
            {
                int folderIndex = FolderInfo.SelectedIndex;
                Folder selected = selectedFolder.folderInnerFolders[folderIndex];

                selected.folderModuleCode = selectedFolder.folderModuleCode;

                (Application.Current as App).folders.Add(selectedFolder.folderInnerFolders[folderIndex]);

                // navigate to a new instance of WorkbinPage
                NavigationService.Navigate(new Uri(String.Format("/WorkbinPage.xaml?ID={0}", Guid.NewGuid()), UriKind.Relative));
            }
        }

        private void DownloadFile(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (FileInfo.SelectedIndex != -1)
            {
                int fileIndex = FileInfo.SelectedIndex;

                string URL = LAPI.GenerateDownloadURL(selectedFolder.folderFiles[fileIndex].fileId);

                WebBrowserTask webBrowserTask = new WebBrowserTask();
                webBrowserTask.Uri = new Uri(URL, UriKind.Absolute);
                webBrowserTask.Show();
            }
        }

        //private void DownloadFile(object sender, System.Windows.Input.GestureEventArgs e)
        //{
        //    if (FileInfo.SelectedIndex != -1)
        //    {
        //        int fileIndex = FileInfo.SelectedIndex;

        //        string URL = LAPI.GenerateDownloadURL(selectedFolder.folderFiles[fileIndex].fileId);
        //        Download(URL, selectedFolder.folderFiles[fileIndex].fileName);
        //    }
        //}

        //private void Download(string url, string fileName)
        //{
        //    HttpWebRequest request = HttpWebRequest.CreateHttp(url);

        //    // download file from workbin
        //    request.BeginGetResponse((asynchronousResult) =>
        //    {
        //        var httpResponse = request.EndGetResponse(asynchronousResult);
        //        using (Stream dataStream = httpResponse.GetResponseStream())
        //        {
        //            using (StreamReader streamReader = new StreamReader(dataStream))
        //            {
        //                string data = streamReader.ReadToEnd();

        //                byte[] buffer = new byte[data.Length];

        //                for (int i = 0; i < data.Length; i++)
        //                {
        //                    buffer[i] = (byte)data[i];
        //                }

        //                dataStream.Close();
        //                streamReader.Close();

        //                FileStream fileStream = new FileStream(fileName, FileMode.CreateNew);
        //                BinaryWriter binaryWriter = new BinaryWriter(fileStream);

        //                for (int i = 0; i < buffer.Length; i++)
        //                {
        //                    binaryWriter.Write(buffer[i]);
        //                }

        //            }
        //        }
        //    }, null);

        //}

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            // remove the currently selected module from list
            (Application.Current as App).folders.RemoveAt((Application.Current as App).folders.Count - 1);

            base.OnBackKeyPress(e);
        }
    }
}