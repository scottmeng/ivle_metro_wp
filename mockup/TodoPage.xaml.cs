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
    public partial class TodoPage : PhoneApplicationPage
    {
        private Todo selectedTodo;

        public TodoPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            // loading existing data for info page and announcement page
            base.OnNavigatedTo(e);

            int todoIndex = 0;
            string index = "";

            if (NavigationContext.QueryString.TryGetValue("todoIndex", out index))
            {
                todoIndex = Convert.ToInt32(index);
            }

            selectedTodo = (Application.Current as App).todos[todoIndex];

            TodoNameTitle.Text = selectedTodo.todoName;
            name.Text = selectedTodo.todoName;
            details.Text = selectedTodo.todoDetail;
            deadline.Text = selectedTodo.todoDeadlineDisplay;
            announceName.Text = selectedTodo.todoRelatedAnnounceName;
            announceDetail.Text = selectedTodo.todoRelatedAnnounceContent;
        }

        private void RemoveTodo(object sender, EventArgs e)
        {
            (Application.Current as App).todos.Remove(selectedTodo);
            (Application.Current as App).UpdateAppTile();

            NavigationService.GoBack();
        }
    }
}