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
    public class ClassWrapper
    {
        [DataMember(Name = "Results")]
        public Class[] classes { get; set; }

        [DataMember(Name = "Comments")]
        public string comments { get; set; }

        public ClassWrapper(Class[] myClasses, string myComments)
        {
            classes = myClasses;
            comments = myComments;
        }
    }
}
