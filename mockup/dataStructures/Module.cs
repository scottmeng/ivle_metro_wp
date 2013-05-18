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
    public class Module
    {
        [DataMember(Name = "Announcements")]
        public Announcement[] moduleAnnouncements { get; set; }

        [DataMember(Name = "Workbins")]
        public Workbin[] moduleWorkbins { get; set; }

        [DataMember(Name = "ID")]
        public String moduleId { get; set; }

        [DataMember(Name = "CourseCode")]
        public String moduleCode { get; set; }

        [DataMember(Name = "CourseName")]
        public String moduleName { get; set;}

        [DataMember(Name = "CourseDepartment")]
        public String moduleDepart { get; set; }

        [DataMember(Name = "CourseSemester")]
        public String moduleSemester { get; set; }

        [DataMember(Name = "CourseAcadYear")]
        public String moduleAcadYear { get; set; }

        [DataMember(Name = "CourseMC")]
        public String moduleMc { get; set; }

        [DataMember(Name = "Lecturers")]
        public Lecturer[] moduleLecturers { get; set; }

        public Module(Announcement[] announces, Workbin[] workins, String id, String code, String name, String depart, String sem, String ay, String mc, Lecturer[] lecturers)
        {
            moduleAnnouncements = announces;
            moduleWorkbins = workins;
            moduleId = id;
            moduleCode = code;
            moduleName = name;
            moduleDepart = depart;
            moduleSemester = sem;
            moduleAcadYear = ay;
            moduleMc = mc;
            moduleLecturers = lecturers;
        }
    }
}
