/*
 * 
 * This file deals with the fancy animated scrolling lists for the new version of the media management section.
 * Currently not being used extensively.
 * I gave up before I realized I would have to make seperate events for each side, so that double-clicking would "move" the item to the other list.
 * 
 *	Originally design by: Kevin Marshall @ http://blogs.claritycon.com/blogs/kevin_marshall/archive/2007/10/18/3332.aspx
 *	Modified to:	a. actually work properly
 *					b. match the IVCS interface & work a little bit smoother. This required a fair amount to be redesigned
 *					c. not crash with the touch screen interface
 *					d. be able to click on items in the list to select them.
 */

using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
//using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
//using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Globalization;
using System.Windows.Media.Animation;
using System.Collections.ObjectModel;

namespace wpfBuild
{
	/// <summary>
	/// Interaction logic for playlistControl.xaml
	/// </summary>
	public partial class playlistControl : UserControl
	{
		private Point mouseDragStartPoint;
        private DateTime mouseDownTime;
        private Point scrollStartOffset;
        private const double DECELERATION = 980;
        private const double SPEED_RATIO = .5;
        private const double MAX_VELOCITY = 2500;
        private const double MIN_DISTANCE = 0;
        //private const double TIME_THRESHOLD = .4;
		private const double TIME_THRESHOLD = 2;


		//private int OLD_TIMESTAMP = 0;

		private int CLICK_COUNT = 0;

        public static readonly DependencyProperty ScrollOffsetProperty = DependencyProperty.Register("ScrollOffset", typeof(double), typeof(playlistControl), new UIPropertyMetadata(playlistControl.ScrollOffsetValueChanged));

		//public static DependencyProperty ShowCoverArtProperty = DependencyProperty.Register("CoverArtIndex", typeof(int), typeof(playlistControl));

        public double ScrollOffset
        {
            get { return (double)GetValue(ScrollOffsetProperty); }

            set { SetValue(ScrollOffsetProperty, value); }
        }

        private static void ScrollOffsetValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            playlistControl myClass = (playlistControl)d;
            myClass.myScrollViewer.ScrollToVerticalOffset((double)e.NewValue);
        }

		public playlistControl()
		{
			InitializeComponent();
			// check which item is currently selected and then auto populate the list
		}

		public void UpdateItemList(ObservableCollection<playlistData> items)
        {
			// STILL doesn't want to time it proplery...
			/* Read the initial time. */
			//DateTime startTime = DateTime.Now;



			//using (this.listItems.Items.DeferRefresh()) {
				this.listItems.ItemsSource = null;
				this.listItems.ItemsSource = items;
			//}

			//this.listItems.Items.Refresh();


			//this.listItems.ItemsSource = null;
            //this.listItems.ItemsSource = items;

			/* Read the end time. */
			//DateTime stopTime = DateTime.Now;

			/* Compute the duration between the initial and the end time. 
				* Print out the number of elapsed hours, minutes, seconds and milliseconds. */
			//TimeSpan duration = stopTime - startTime;
			//Console.WriteLine("Control <" + this.Name.ToString() + "> refresh duration");
			//Console.WriteLine("seconds:" + duration.TotalSeconds);
			//Console.WriteLine("milliseconds:" + duration.TotalMilliseconds);
        }

		/*
		public int CoverArtIndex
		{
			get { return (int)GetValue(ShowCoverArtProperty); }

			set { 
				// hide the old cover art image
				//hideCoverArt();
				SetValue(ShowCoverArtProperty, value); 
			}
		}

		private void hideCoverArt(int item_index)
		{
			//listItems.Items.
			//string[] media_info = (string[])((ListViewItem)listview_now_playing.Items[media_nowplaying_id]).Content;
		}
		*/

		

        private void Scroll(double startY, double endY, DateTime startTime, DateTime endTime)
        {
            double timeScrolled = endTime.Subtract(startTime).TotalSeconds;

            //if scrolling slowly, don't scroll with force
			// fix: this looks backwards, because threshold is an UPPER LIMIT
            if (timeScrolled < TIME_THRESHOLD)
            {
                double distanceScrolled = Math.Max(Math.Abs(endY - startY), MIN_DISTANCE);

                double velocity = distanceScrolled / timeScrolled;
                velocity = Math.Min(MAX_VELOCITY, velocity);
                int direction = 1;

                if (endY > startY)
                {
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
                this.BeginAnimation(playlistControl.ScrollOffsetProperty, scrollAnimation);
            }
        }
      

        #region Mouse Overrides
        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {
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

        protected override void OnPreviewMouseMove(MouseEventArgs e)
        {
            if (this.IsMouseCaptured)
            {
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

        protected override void OnPreviewMouseUp(MouseButtonEventArgs e)
        {
            if (this.IsMouseCaptured)
            {
                //this.Cursor = Cursors.Arrow;
                this.ReleaseMouseCapture();
            }

            //Scroll(mouseDragStartPoint.Y, e.GetPosition(this).Y, mouseDownTime, DateTime.Now);
			try {
				Scroll(mouseDragStartPoint.Y, e.GetPosition(this).Y, mouseDownTime, DateTime.Now);
			}
			catch (Exception ex) {
				//MessageBox.Show(mouseDragStartPoint.Y.ToString() + " , " + e.GetPosition(this).Y.ToString() + " , " + mouseDownTime + " , " + DateTime.Now.ToString() );
				//MessageBox.Show(ex.TargetSite + " => " + ex.Message);
				Logging.WriteToLog("Check touchscreen setting. Click on touch seems to cause some problems, switch it to normal! \r\n" + ex.TargetSite + " => " + ex.Message + "\r\n OnPreviewMouseUp -- playlistControl");
			}

            //base.OnPreviewMouseUp(e);
        }

        #endregion

		private void listItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			ListView listView = (ListView)sender;

			Window1 asdf = Window1.Instance;
			asdf.playNowPlayingID(listView.SelectedIndex);
		}
	}


	// ######################


	public sealed class BackgroundConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			ListViewItem item = (ListViewItem)value;
			ListView listView = ItemsControl.ItemsControlFromItemContainer(item) as ListView;
			// Get the index of a ListViewItem
			int index = listView.ItemContainerGenerator.IndexFromContainer(item);

			if (index % 2 == 0) {
				//return Brushes.Silver;
				return "#990A0A0A";
			}
			else {
				return "#550A0A0A";

				//wpfBuild.Window1.Instance.

				//return Brushes.Transparent;
				//return System.Windows.Media.Color.
			}
		}
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new Exception("The method or operation is not implemented.");
		}
	}

	// #######################


	/// <summary>
	/// This will contain a structure for all our playlist data
	/// Note: this information only gets updated on playlist load, so some information may get stale.
	/// </summary>
	public class playlistData
	{
		public playlistData()
		{
		}

		private int id;
		private string title;
		private string artist;
		private string album;
		private string length;
		private int rating;
		private uint playcount;
		private BitmapImage cover_path;
		private bool using_alt_template;

		private string filepath;

		//public string SongCoverImage
		public BitmapImage SongCoverImage
		{
			//get { return this.cover_path.ToString(); }
			//set { this.cover_path = new BitmapImage(new Uri(@value, UriKind.Absolute)); }

			get { return this.cover_path; }
			set { this.cover_path = value; }
		}

		public int ID
		{
			get { return this.id; }
			set { this.id = value; }
		}

		public string SongTitle
		{
			get { return this.title; }
			set { this.title = value; }
		}

		public int SongRating
		{
			get { return this.rating; }
			set { this.rating = value; }
		}

		public string SongArtist
		{
			get { return this.artist; }
			set { this.artist = value; }
		}

		public string SongAlbum
		{
			get { return this.album; }
			set { this.album = value; }
		}

		public string SongLength
		{
			get { return this.length; }
			set { this.length = value; }
		}

		public string MediaPath
		{
			get { return this.filepath; }
			set { this.filepath = value; }
		}

		public uint TimesPlayed
		{
			get { return this.playcount; }
			set { this.playcount = value; }
		}

		public bool UseTemplateB
		{
			get { return this.using_alt_template; }
			set { this.using_alt_template = value; }
		}

	}
}
