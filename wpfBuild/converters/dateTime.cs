/*
 *	Convert hours and minutes to proper angle values for use in the basic clock.
 *	Very, very basic function. Barely implemented.
 */

using System;
using System.Windows.Data;

namespace wpfBuild.converters
{

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

}