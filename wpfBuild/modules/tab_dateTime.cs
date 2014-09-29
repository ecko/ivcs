using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
//using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Windows.Threading;
using System.Data.SQLite;
using System.Data.Common;
using System.IO;

namespace wpfBuild
{
	/// <summary>
	/// Interaction logic for Window1.xaml
	/// </summary>
	public partial class Window1 : Window
	{

		//public bool video_fullscreen = false;

		//private void tabitem_video_Loaded(object sender, RoutedEventArgs e)
		//{

		/*
		 * need the guys DLL file...
		void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			//http://thispointer.spaces.live.com/blog/cns!74930F9313F0A720!252.entry?_c11_blogpart_blogpart=blogview&_c=blogpart#permalink
			this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
			{
				secondHand.Angle = DateTime.Now.Second * 6;
				minuteHand.Angle = DateTime.Now.Minute * 6;
				hourHand.Angle = (DateTime.Now.Hour * 30) + (DateTime.Now.Minute * 0.5);
			}));
		}

		private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
		{
			this.DragMove();
		}
		*/
	}

	


	#region doesn't work

	public class HourToAngle : IValueConverter
	{
		#region IValueConverter Members

		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			DateTime time = System.Convert.ToDateTime(value);
			double Angle = time.Hour * 30;
			Angle += 12 * time.Minute / 60;
			return Angle;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			// Not required.
			return null;
		}
		#endregion
	}

	public class MinuteToAngle : IValueConverter
	{
		#region IValueConverter Members

		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			DateTime time = System.Convert.ToDateTime(value);
			double Angle = time.Minute * 6;
			return Angle;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			// Not required;
			return null;
		}
		#endregion
	}
#endregion


}
