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
    public class Lecturer
    {
        [DataMember(Name = "ID")]
        public String lecturerId { get; set; }

        [DataMember(Name = "User")]
        public Member lecturerMember { get; set; }

        [DataMember(Name = "Role")]
        public String lecturerRole { get; set; }

        public Lecturer(String id, Member member, String role)
        {
            lecturerId = id;
            lecturerMember = member;
            lecturerRole = role;
        }
    }
}
