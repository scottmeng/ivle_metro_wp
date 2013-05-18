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
    public class ModuleInfoWrapper
    {
        [DataMember(Name = "Results")]
        public Module[] modules { get; set; }

        [DataMember(Name = "Comments")]
        public string comments { get; set; }

        public ModuleInfoWrapper(Module[] ms, String cs)
        {
            modules = ms;
            comments = cs;
        }
    }
}
