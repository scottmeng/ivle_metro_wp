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
    public class Member
    {
        [DataMember(Name = "UserId")]
        public String memberId { get; set; }

        [DataMember(Name = "Name")]
        public String memberName { get; set; }

        [DataMember(Name = "Email")]
        public String memberEmail { get; set; }

        [DataMember(Name = "UserGuid")]
        public String memberGuid { get; set; }

        public Member(String id, String name, String email, String guid)
        {
            memberId = id;
            memberName = name;
            memberEmail = email;
            memberGuid = guid;
        }
    }
}
