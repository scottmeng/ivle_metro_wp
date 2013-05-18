using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;



namespace mockup
{
    public class User
    {
        private String myToken { get; set; }
        public Boolean validated { get; set; }
        public Boolean initialed { get; set; }
        public List<Todo> todoList { get; set; }
        public LAPI myLAPI { get; set; }
        public int updateInterval { get; set; }

        public User(String token, List<Todo> list)
        {
            myToken = token;
            todoList = list;
            validated = false;
            initialed = false;

            //myLAPI = new LAPI(this); // pass the user object to LAPI, get the methods
            initialUser();           // initialize the API requests
        }

        public string getUserToken()
        {
            return myToken;
        }

        public List<Todo> getTodoList()
        {
            return todoList;
        }

        // initialize the user
        public void initialUser()
        {
            // generate the Get_UserName url and get the response
            string nameURL = LAPI.requestURL("UserName_Get", null, null);
            //JObject nameObj = ...

            // generate the module url and get the response
            string moduleURL = LAPI.requestURL("Modules", new string[] { "Duration", "IncludeAllInfo" }, new string[] { "60", "true" });
            // JObject moduleObj = ...

            // generate the timetable url and hget the response
            string timetableURL = LAPI.requestURL("Timetable_Student", new string[] { "AcadYear", "Semester" }, new string[] { "2011/2012", "2" });
            // JObject timetableObj = ...
        }

        // update the modules, including announcements
        public void updateModules() { }

        // uupdate the timetable
        public void updateTimetable() { }

    };
}
