﻿#pragma checksum "C:\Users\Scott\ivle_metro_wp\mockup\MenuPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "6E1344BDEC396A50CD14C646AA1B9C12"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18046
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
    
    
    public partial class PanoramaPage1 : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.ListBox ModuleInfo;
        
        internal System.Windows.Controls.ListBox ClassInfo;
        
        internal System.Windows.Controls.ListBox TodoInfo;
        
        internal Microsoft.Phone.Controls.PerformanceProgressBar loadModuleProgressBar;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/mockup;component/MenuPage.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.ModuleInfo = ((System.Windows.Controls.ListBox)(this.FindName("ModuleInfo")));
            this.ClassInfo = ((System.Windows.Controls.ListBox)(this.FindName("ClassInfo")));
            this.TodoInfo = ((System.Windows.Controls.ListBox)(this.FindName("TodoInfo")));
            this.loadModuleProgressBar = ((Microsoft.Phone.Controls.PerformanceProgressBar)(this.FindName("loadModuleProgressBar")));
        }
    }
}

