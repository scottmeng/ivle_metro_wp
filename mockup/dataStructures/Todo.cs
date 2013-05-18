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
using System.Runtime.Serialization;

namespace mockup
{
    [DataContract]
    public class Todo
    {
        [DataMember]
        public string todoName { get; set; }

        [DataMember]
        public string todoDetail { get; set; }

        [DataMember]
        public string todoRelatedAnnounceName { get; set; }

        [DataMember]
        public string todoRelatedAnnounceContent { get; set; }

        [DataMember]
        public DateTime todoDeadline { get; set; }

        [DataMember]
        public string todoDeadlineDisplay { get; set; }

        [DataMember]
        public string todoRelatedPreview { get; set; }

        [DataMember]
        public string todoModuleCode { get; set; }

        public Todo(string name, string detail, string announceName, string annouceContent, DateTime deadline, string moduleCode)
        {
            todoName = name;
            todoDetail = detail;
            todoRelatedAnnounceName = announceName;
            todoRelatedAnnounceContent = annouceContent;
            todoDeadline = deadline;
            todoDeadlineDisplay = todoDeadline.Day + "/" + todoDeadline.Month;
            todoModuleCode = moduleCode;
            todoRelatedPreview = todoModuleCode + " : " + todoRelatedAnnounceName;
        }
    }
}
