﻿#pragma checksum "..\..\CustJournal.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "0D7369ABADCF7528E73A841FA7AFBB38"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.3053
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using KineticScrollingPrototype;
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


namespace KineticScrollingPrototype {
    
    
    /// <summary>
    /// CustJournal
    /// </summary>
    public partial class CustJournal : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector, System.Windows.Markup.IStyleConnector {
        
        
        #line 6 "..\..\CustJournal.xaml"
        internal KineticScrollingPrototype.CustJournal UserControl;
        
        #line default
        #line hidden
        
        
        #line 96 "..\..\CustJournal.xaml"
        internal System.Windows.Controls.Border more_content_up;
        
        #line default
        #line hidden
        
        
        #line 116 "..\..\CustJournal.xaml"
        internal System.Windows.Controls.Border more_content_down;
        
        #line default
        #line hidden
        
        
        #line 117 "..\..\CustJournal.xaml"
        internal System.Windows.Controls.Border border;
        
        #line default
        #line hidden
        
        
        #line 123 "..\..\CustJournal.xaml"
        internal System.Windows.Controls.Border shine;
        
        #line default
        #line hidden
        
        
        #line 135 "..\..\CustJournal.xaml"
        internal System.Windows.Controls.ScrollViewer myScrollViewer;
        
        #line default
        #line hidden
        
        
        #line 136 "..\..\CustJournal.xaml"
        internal System.Windows.Controls.ListView listItems;
        
        #line default
        #line hidden
        
        
        #line 149 "..\..\CustJournal.xaml"
        internal System.Windows.Controls.GridView gvItems;
        
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
            System.Uri resourceLocater = new System.Uri("/wpfBuild;component/custjournal.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\CustJournal.xaml"
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
            this.UserControl = ((KineticScrollingPrototype.CustJournal)(target));
            return;
            case 3:
            this.more_content_up = ((System.Windows.Controls.Border)(target));
            
            #line 96 "..\..\CustJournal.xaml"
            this.more_content_up.MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.more_content_up_MouseDown);
            
            #line default
            #line hidden
            return;
            case 4:
            this.more_content_down = ((System.Windows.Controls.Border)(target));
            
            #line 116 "..\..\CustJournal.xaml"
            this.more_content_down.MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.more_content_down_MouseDown);
            
            #line default
            #line hidden
            return;
            case 5:
            this.border = ((System.Windows.Controls.Border)(target));
            return;
            case 6:
            this.shine = ((System.Windows.Controls.Border)(target));
            return;
            case 7:
            this.myScrollViewer = ((System.Windows.Controls.ScrollViewer)(target));
            return;
            case 8:
            this.listItems = ((System.Windows.Controls.ListView)(target));
            
            #line 136 "..\..\CustJournal.xaml"
            this.listItems.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.listItems_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 9:
            this.gvItems = ((System.Windows.Controls.GridView)(target));
            return;
            }
            this._contentLoaded = true;
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        void System.Windows.Markup.IStyleConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 2:
            
            #line 24 "..\..\CustJournal.xaml"
            ((System.Windows.Controls.Grid)(target)).MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.Grid_MouseDown);
            
            #line default
            #line hidden
            break;
            }
        }
    }
}
