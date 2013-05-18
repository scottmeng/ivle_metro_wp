﻿using System;
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
    public class File
    {
        [DataMember(Name = "ID")]
        public string fileId { get; set; }

        [DataMember(Name = "FileName")]
        public string fileName { get; set; }

        [DataMember(Name = "FileDescription")]
        public string fileDescrip { get; set; }

        [DataMember(Name = "FileSize")]
        public long fileSize { get; set; }

        [DataMember(Name = "FileType")]
        public string fileType { get; set; }

        [DataMember(Name = "isDownloaded")]
        public bool fileDownloaded { get; set; }

        [DataMember(Name = "UploadTime_js")]
        public DateTime fileUploadTime { get; set; }

        [DataMember(Name = "Creator")]
        public Member fileCreator { get; set; }

        public String fileSizeDisplay { get; set; }

        public File(string id, string name, string descrip, long size, string type, bool downloaded, DateTime uploadTime, Member creator)
        {
            fileId = id;
            fileName = name;
            fileDescrip = descrip;
            fileSize = size;
            fileType = type;
            fileDownloaded = downloaded;
            fileUploadTime = uploadTime;
            fileCreator = creator;
        }

        public void GenerateDisplayContent()
        {
            long integerPart, decimalPart;

            if (fileSize > 1000000)
            {
                integerPart = fileSize / 1000000;
                decimalPart = (fileSize - integerPart * 1000000 )/100000;

                fileSizeDisplay = integerPart + "." + decimalPart + "MB";
            }
            else if (fileSize > 1000)
            {
                integerPart = fileSize / 1000;
                decimalPart = (fileSize - integerPart * 1000) / 100;

                fileSizeDisplay = integerPart + "." + decimalPart + "KB";
            }
            else
            {
                fileSizeDisplay = fileSize + "B";
            }
        }
    }
}
