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
    public partial class AddToDoPage : PhoneApplicationPage
    {
        private Announcement todoRelatedAnnouncement;

        public AddToDoPage()
        {
            InitializeComponent();

            // get the announcement information from global variable
            todoRelatedAnnouncement = (Application.Current as App).announcement;

            // show the name of the related announcement
            ToDoRelatedAnnouncement.Text = todoRelatedAnnouncement.announceModuleCode + "-" + todoRelatedAnnouncement.announceNameDisplay;
        }

        private void ConfirmButton_Click(object sender, EventArgs e)
        {
            string todoName = TodoName.Text;
            string todoRemarks = TodoRemarks.Text;
            DateTime todoDeadline = (DateTime)TodoDeadline.Value;

            // create a new to-do item
            Todo newTodo = new Todo(todoName, todoRemarks, todoRelatedAnnouncement.announceNameDisplay, todoRelatedAnnouncement.announceContentDisplay, todoDeadline, (Application.Current as App).announcement.announceModuleCode);

            // add the new to-do item onto the list
            (Application.Current as App).todos.Add(newTodo);
            (Application.Current as App).UpdateAppTile();

            MessageBox.Show("To-do item added successfully");

            IsoStoreHelper.SaveContent("todo", (Application.Current as App).todos, typeof(List<Todo>));

            // return back to the announcement page
            NavigationService.GoBack();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            // discard all changes made and navigate back to the announcement page
            NavigationService.GoBack();
        }
    }
}