/*
 * This file deals with the fancy animated scrolling list for the media folder list.
 *	Originally design by: Kevin Marshall @ http://blogs.claritycon.com/blogs/kevin_marshall/archive/2007/10/18/3332.aspx
 *	Modified to:	a. actually work properly
 *					b. match the IVCS interface & work a little bit smoother. This required a fair amount to be redesigned
 *					c. not crash with the touch screen interface
 *					d. be able to click on items in the list to select them.
 *					
 * It works pretty well.
 * 
 * Plans for the future (some of these could also apply to all the special scrolling lists):
 * Make it easier to tell if the user should scroll up or down.
 * Display a message if there is nothing in the list. This way it's not just a big empty space
 * Redesign how the user is required to select something. Possibly do a "press-hold" == select, then another click will actually activate it
 */

using System;
using System.Collections.Generic;
//using System.Linq;
//using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
//using System.Windows.Documents;
using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using System.Windows.Navigation;
//using System.Windows.Shapes;
//using System.Globalization;
using System.Windows.Media.Animation;
using System.Collections.ObjectModel;
using wpfBuild.data;

namespace wpfBuild.controls
{
	/// <summary>
	/// Interaction logic for mediaFolderList.xaml
	/// </summary>
	public partial class mediaFolderList : UserControl
	{
		private Point mouseDragStartPoint;
		private DateTime mouseDownTime;
		private Point scrollStartOffset;
		private const double DECELERATION = 980;
		private const double SPEED_RATIO = .5;
		private const double MAX_VELOCITY = 2500;
		private const double MIN_DISTANCE = 0;
		private const double TIME_THRESHOLD = 2;
		private int CLICK_COUNT = 0;

		public static readonly DependencyProperty ScrollOffsetProperty = DependencyProperty.Register("ScrollOffset", typeof(double), typeof(mediaFolderList), new UIPropertyMetadata(mediaFolderList.ScrollOffsetValueChanged));

		public mediaFolderList()
		{
			InitializeComponent();
		}

		public double ScrollOffset
		{
			get { return (double)GetValue(ScrollOffsetProperty); }
			set { SetValue(ScrollOffsetProperty, value); }
		}

		private static void ScrollOffsetValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			mediaFolderList myClass = (mediaFolderList)d;
			myClass.myScrollViewer.ScrollToVerticalOffset((double)e.NewValue);
		}


		public void UpdateItemList(ObservableCollection<mediaFolderListData> items)
		//public void UpdateItemList()
		{
			// STILL doesn't want to time it proplery...
			// Read the initial time. //
			//DateTime startTime = DateTime.Now;

			this.listItems.ItemsSource = null;
			this.listItems.ItemsSource = items;


			// idea: look into doing this:
				//foreach (Object item in new System.Collections.ArrayList(row.Count))
				//for (int x = 1; x < row.Count; x++)
				//{
				//    (management_listview.ItemsSource as System.Collections.IList).Add((ListViewItem)row[x]);
				//}
				//(management_listview.ItemsSource as System.Collections.IList).Add(ii);





			// Read the end time. //
			//DateTime stopTime = DateTime.Now;

			// Compute the duration between the initial and the end time. 
				// Print out the number of elapsed hours, minutes, seconds and milliseconds. //
			//TimeSpan duration = stopTime - startTime;
			//Console.WriteLine("Control <" + this.Name.ToString() + "> refresh duration");
			//Console.WriteLine("seconds:" + duration.TotalSeconds);
			//Console.WriteLine("milliseconds:" + duration.TotalMilliseconds);
		}


		private void Scroll(double startY, double endY, DateTime startTime, DateTime endTime)
		{
			double timeScrolled = endTime.Subtract(startTime).TotalSeconds;

			//if scrolling slowly, don't scroll with force
			// fix: this looks backwards, because threshold is an UPPER LIMIT
			if (timeScrolled < TIME_THRESHOLD) {
				double distanceScrolled = Math.Max(Math.Abs(endY - startY), MIN_DISTANCE);

				double velocity = distanceScrolled / timeScrolled;
				velocity = Math.Min(MAX_VELOCITY, velocity);
				int direction = 1;

				if (endY > startY) {
					direction = -1;
				}

				double timeToScroll = (velocity / DECELERATION) * SPEED_RATIO;

				double distanceToScroll = ((velocity * velocity) / (2 * DECELERATION)) * SPEED_RATIO;

				DoubleAnimation scrollAnimation = new DoubleAnimation();
				scrollAnimation.From = myScrollViewer.VerticalOffset;
				scrollAnimation.To = myScrollViewer.VerticalOffset + distanceToScroll * direction;
				scrollAnimation.DecelerationRatio = .9;
				scrollAnimation.SpeedRatio = SPEED_RATIO;
				scrollAnimation.Duration = new Duration(new TimeSpan(0, 0, 0, Convert.ToInt32(timeToScroll), 0));
				this.BeginAnimation(mediaFolderList.ScrollOffsetProperty, scrollAnimation);
			}
		}


		#region Mouse Overrides
		protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
		{
			try {

				mouseDragStartPoint = e.GetPosition(this);
				mouseDownTime = DateTime.Now;
				scrollStartOffset.X = myScrollViewer.HorizontalOffset;
				scrollStartOffset.Y = myScrollViewer.VerticalOffset;

				this.CLICK_COUNT = e.ClickCount;

				// Update the cursor if scrolling is possible 
				//this.Cursor = (myScrollViewer.ExtentWidth > myScrollViewer.ViewportWidth) ||
				//    (myScrollViewer.ExtentHeight > myScrollViewer.ViewportHeight) ?
				//   Cursors.ScrollAll : Cursors.Arrow;

				if (e.ClickCount >= 2) {
					//Console.WriteLine(this.CLICK_COUNT);
					this.CaptureMouse();
					this.ReleaseMouseCapture();
					return;
				}

				this.CaptureMouse();
				//base.OnPreviewMouseDown(e);
			}
			catch (Exception ex) {
				MessageBox.Show(ex.TargetSite + " => " + ex.Message);
				Logging.WriteToLog(ex.TargetSite + " => " + ex.Message + "\r\n onpreviewMDown");
			}
		}

		protected override void OnPreviewMouseMove(MouseEventArgs e)
		{
			try {
				if (this.IsMouseCaptured) {
					// Get the new mouse position. 
					Point mouseDragCurrentPoint = e.GetPosition(this);

					// Determine the new amount to scroll. 
					Point delta = new Point(
						(mouseDragCurrentPoint.X > this.mouseDragStartPoint.X) ?
						-(mouseDragCurrentPoint.X - this.mouseDragStartPoint.X) :
						(this.mouseDragStartPoint.X - mouseDragCurrentPoint.X),
						(mouseDragCurrentPoint.Y > this.mouseDragStartPoint.Y) ?
						-(mouseDragCurrentPoint.Y - this.mouseDragStartPoint.Y) :
						(this.mouseDragStartPoint.Y - mouseDragCurrentPoint.Y));

					// Scroll to the new position. 
					// looks like it has som problems scrolling horizontally AND vertically
					//myScrollViewer.ScrollToHorizontalOffset(this.scrollStartOffset.X + delta.X);
					myScrollViewer.ScrollToVerticalOffset(this.scrollStartOffset.Y + delta.Y);
				}
				//base.OnPreviewMouseMove(e);
			}
			catch (Exception ex) {
				MessageBox.Show(ex.TargetSite + " => " + ex.Message);
				Logging.WriteToLog(ex.TargetSite + " => " + ex.Message + "\r\n onpreviewMM");
			}
		}

		protected override void OnPreviewMouseUp(MouseButtonEventArgs e)
		{
			if (this.IsMouseCaptured) {
				//this.Cursor = Cursors.Arrow;
				this.ReleaseMouseCapture();
			}

			try {
				Scroll(mouseDragStartPoint.Y, e.GetPosition(this).Y, mouseDownTime, DateTime.Now);
			}
			catch (Exception ex) {
				//MessageBox.Show(mouseDragStartPoint.Y.ToString() + " , " + e.GetPosition(this).Y.ToString() + " , " + mouseDownTime + " , " + DateTime.Now.ToString() );
				//MessageBox.Show(ex.TargetSite + " => " + ex.Message);
				Logging.WriteToLog("Check touchscreen setting. Click on touch seems to cause some problems, switch it to normal! \r\n" + ex.TargetSite + " => " + ex.Message + "\r\n OnPreviewMouseUp");
			}
				//base.OnPreviewMouseUp(e);
			
		}

		#endregion

		private void listItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			ListView listView = (ListView)sender;
			Window1 main_instance = Window1.Instance;

			// because the item gets updated every time the tabcontrol loads... (BAD)
			//	it fires the selection changed if you had something selected, and then close and re-open.
			//	to avoid this, check to make sure a valid item is actually selected.
			// fixme: this could be a bug I'm just working around. however the control reloads the media folders very often

			try {
				if (listView.SelectedIndex == -1) {
					// blank the folder image because nothing is selected
					main_instance.mediafolder_preview.ImageSource = null;
					// clear the path and selected things
					main_instance.mediafolder_folderpath.Text = "";
					main_instance.mediafolder_type_audio.IsChecked = main_instance.mediafolder_type_video.IsChecked = main_instance.mediafolder_type_image.IsChecked = false;
					main_instance.mediafolder_rescan_selected.IsEnabled = false;
					return;
				}
			}
			catch (Exception ex) {
				MessageBox.Show(ex.Message);
				Logging.WriteToLog(ex.Message);
			}


			try {
				mediaFolderListData selected_folder = (mediaFolderListData)main_instance.mediaFolderList[listView.SelectedIndex];

				// swap the folder icon images
				main_instance.mediafolder_preview.ImageSource = audioSystem.getFolderJPG(selected_folder.FolderPath);

				// update some of media folder properties for the selected folder
				main_instance.mediafolder_folderpath.Text = selected_folder.FolderPath;

				if (selected_folder.FolderType == 0) {
					// AUDIO
					main_instance.mediafolder_type_audio.IsChecked = true;
					main_instance.mediafolder_type_video.IsChecked = false;
					main_instance.mediafolder_type_image.IsChecked = false;
				}
				else if (selected_folder.FolderType == 1) {
					// VIDEO
					main_instance.mediafolder_type_audio.IsChecked = false;
					main_instance.mediafolder_type_video.IsChecked = true;
					main_instance.mediafolder_type_image.IsChecked = false;
				}
				else {
					// IMAGE
					main_instance.mediafolder_type_audio.IsChecked = false;
					main_instance.mediafolder_type_video.IsChecked = false;
					main_instance.mediafolder_type_image.IsChecked = true;
				}

				main_instance.mediafolder_search_subfolders.IsChecked = selected_folder.SearchSubfolders;


				// disable the add button, because we're selecting a folder already added
				main_instance.mediafolder_add.IsEnabled = false;
				main_instance.mediafolder_remove.IsEnabled = true;
				main_instance.mediafolder_rescan_selected.IsEnabled = true;
			}
			catch (Exception ex) {
				MessageBox.Show(ex.Message);
				Logging.WriteToLog(ex.Message);
			}
		}

	}

	public sealed class BackgroundConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			ListViewItem item = (ListViewItem)value;
			ListView listView = ItemsControl.ItemsControlFromItemContainer(item) as ListView;
			// Get the index of a ListViewItem
			int index = listView.ItemContainerGenerator.IndexFromContainer(item);

			if (index % 2 == 0) {
				return "#990A0A0A";
			}
			else {
				return "#550A0A0A";
			}
		}
		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new Exception("The method or operation is not implemented.");
		}
	}

}
