﻿#pragma checksum "C:\Users\Scott\Desktop\IVLEMetro\mockup\TodoPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "686B3AE7ED1041982AD894FD503DB78F"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.17379
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Phone.Controls;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace mockup {
    
    
    public partial class TodoPage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.StackPanel TitlePanel;
        
        internal System.Windows.Controls.TextBlock ApplicationTitle;
        
        internal System.Windows.Controls.TextBlock TodoNameTitle;
        
        internal System.Windows.Controls.Grid ContentPanel;
        
        internal System.Windows.Controls.TextBlock name;
        
        internal System.Windows.Controls.TextBlock details;
        
        internal System.Windows.Controls.TextBlock deadline;
        
        internal System.Windows.Controls.TextBlock announceName;
        
        internal System.Windows.Controls.TextBlock announceDetail;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/mockup;component/TodoPage.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.TitlePanel = ((System.Windows.Controls.StackPanel)(this.FindName("TitlePanel")));
            this.ApplicationTitle = ((System.Windows.Controls.TextBlock)(this.FindName("ApplicationTitle")));
            this.TodoNameTitle = ((System.Windows.Controls.TextBlock)(this.FindName("TodoNameTitle")));
            this.ContentPanel = ((System.Windows.Controls.Grid)(this.FindName("ContentPanel")));
            this.name = ((System.Windows.Controls.TextBlock)(this.FindName("name")));
            this.details = ((System.Windows.Controls.TextBlock)(this.FindName("details")));
            this.deadline = ((System.Windows.Controls.TextBlock)(this.FindName("deadline")));
            this.announceName = ((System.Windows.Controls.TextBlock)(this.FindName("announceName")));
            this.announceDetail = ((System.Windows.Controls.TextBlock)(this.FindName("announceDetail")));
        }
    }
}
