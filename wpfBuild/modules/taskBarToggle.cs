/*
 *	Some code used by the settings section to show the taskbar.
 *	DO NOT USE THE HIDE CODE! The taskbar will be hidden for good if you use that. This means the user will HAVE to open up the IVCS and show the taskbar again.
 */

using System;

namespace wpfBuild
{
	public partial class Window1 : System.Windows.Window
	{
		private const int SWP_HIDEWINDOW = 0x80;
		private const int SWP_SHOWWINDOW = 0x40;

		[System.Runtime.InteropServices.DllImport("user32.dll")]

		public static extern bool SetWindowPos(
		int hWnd,                 //   handle to window    
		int hWndInsertAfter,  //   placement-order handle    
		short X,                  //   horizontal position    
		short Y,                  //   vertical position    
		short cx,                 //   width    
		short cy,                //    height    
		uint uFlags             //    window-positioning options    
		);



		[System.Runtime.InteropServices.DllImport("user32.dll")]
		public static extern int FindWindow(
		string lpClassName,      //   class name    
		string lpWindowName   //   window name    
		);

	}
}