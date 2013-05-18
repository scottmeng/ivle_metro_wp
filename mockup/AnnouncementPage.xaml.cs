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

namespace mockup
{
    public partial class AnnouncementPage : PhoneApplicationPage
    {
        private Announcement selectedAnnouncement;

        public AnnouncementPage()
        {
            InitializeComponent();

            selectedAnnouncement = (Application.Current as App).announcement;
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            ModuleCode.Text = selectedAnnouncement.announceModuleCode;

            AnnouncementContent.Text = selectedAnnouncement.announceContentDisplay;

            AnnouncementName.Text = selectedAnnouncement.announceNameDisplay;

            AnnouncementDate.Text = selectedAnnouncement.announceTime.ToLongDateString() + " " + selectedAnnouncement.announceTime.ToShortTimeString();
        }

        private void AddTodoButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri(("/AddToDoPage.xaml"), UriKind.Relative));
        }
    }
}