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
    public class Workbin
    {
        [DataMember(Name = "ID")]
        public String workbinID { get; set; }

        [DataMember(Name = "Title")]
        public String workbinTitle { get; set; }

        [DataMember(Name = "Creator")]
        public Member workbinCreator { get; set; }

        [DataMember(Name = "Folders")]
        public Folder[] workbinFolders { get; set; }
        
        public Workbin(String id, String title, Member creator, Folder[] folders)
        {
            workbinID = id;
            workbinTitle = title;
            workbinCreator = creator;
            workbinFolders = folders;
        }
    }
}
