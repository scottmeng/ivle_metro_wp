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
    public class Folder
    {
        [DataMember(Name = "ID")]
        public string folderId { get; set; }

        [DataMember(Name = "FolderName")]
        public string folderName { get; set; }

        [DataMember(Name = "OpenDate_js")]
        public DateTime folderOpenDate { get; set; }

        [DataMember(Name = "CloseDate_js")]
        public DateTime folderCloseDate { get; set; }

        [DataMember(Name = "FileCount")]
        public int folderFileCount { get; set; }

        [DataMember(Name = "Folders")]
        public Folder[] folderInnerFolders { get; set; }

        [DataMember(Name = "Files")]
        public File[] folderFiles { get; set; }

        public string folderModuleCode { get; set; }

        public Folder(string id, string name, DateTime openDate, DateTime closeDate, int count, Folder[] folders, File[] files)
        {
            folderId = id;
            folderName = name;
            folderOpenDate = openDate;
            folderCloseDate = closeDate;
            folderFileCount = count;
            folderInnerFolders = folders;
            folderFiles = files;
        }

        public void recordModuleCode(string moduleCode)
        {
            folderModuleCode = moduleCode;
        }
    }
}
