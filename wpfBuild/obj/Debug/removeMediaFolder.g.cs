﻿#pragma checksum "..\..\removeMediaFolder.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "638A9BC4B6728536B850E98B7B41F2EA"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.3053
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace wpfBuild {
    
    
    /// <summary>
    /// removeMediaFolder
    /// </summary>
    public partial class removeMediaFolder : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 20 "..\..\removeMediaFolder.xaml"
        internal System.Windows.Controls.Label label1;
        
        #line default
        #line hidden
        
        
        #line 23 "..\..\removeMediaFolder.xaml"
        internal System.Windows.Controls.Label label_item_count;
        
        #line default
        #line hidden
        
        
        #line 32 "..\..\removeMediaFolder.xaml"
        internal System.Windows.Controls.Label folder_id_array;
        
        #line default
        #line hidden
        
        
        #line 35 "..\..\removeMediaFolder.xaml"
        internal System.Windows.Controls.ProgressBar remove_progress;
        
        #line default
        #line hidden
        
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
            System.Uri resourceLocater = new System.Uri("/wpfBuild;component/removemediafolder.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\removeMediaFolder.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 4 "..\..\removeMediaFolder.xaml"
            ((wpfBuild.removeMediaFolder)(target)).Closed += new System.EventHandler(this.Window_Closed);
            
            #line default
            #line hidden
            return;
            case 2:
            this.label1 = ((System.Windows.Controls.Label)(target));
            return;
            case 3:
            this.label_item_count = ((System.Windows.Controls.Label)(target));
            return;
            case 4:
            
            #line 30 "..\..\removeMediaFolder.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            
            #line 31 "..\..\removeMediaFolder.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_Click_1);
            
            #line default
            #line hidden
            return;
            case 6:
            this.folder_id_array = ((System.Windows.Controls.Label)(target));
            return;
            case 7:
            this.remove_progress = ((System.Windows.Controls.ProgressBar)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}
