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
    public class Token
    {
        [DataMember(Name = "Token")]
        public string TokenContent { get; set; }

        [DataMember(Name = "Success")]
        public bool TokenSuccess { get; set; }

        [DataMember(Name = "ValidTill_js")]
        public DateTime TokenValidTill { get; set; }

        public Token(string content, bool success, DateTime validTill)
        {
            TokenContent = content;
            TokenSuccess = success;
            TokenValidTill = validTill;
        }
    }
}
