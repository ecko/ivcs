/*
 *		Audio Now Playing Playlist
 *	This file deals with the fancy animated scrolling list for the audio now playing playlist.
 *	Originally design by: Kevin Marshall @ http://blogs.claritycon.com/blogs/kevin_marshall/archive/2007/10/18/3332.aspx
 *	Modified to:	a. actually work properly
 *					b. match the IVCS interface & work a little bit smoother. This required a fair amount to be redesigned
 *					c. not crash with the touch screen interface
 *					d. be able to click on items in the list to select them.
 *					
 * It's still called CustJournal because it just started out as a test, then I kept adding to it and it ended up working!
 */
using System;
//using System.IO;
//using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
//using System.Windows.Navigation;
using System.Collections.ObjectModel;
using System.Windows.Input;

using wpfBuild;


namespace KineticScrollingPrototype
{
	public partial class CustJournal
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

        public static readonly DependencyProperty ScrollOffsetProperty = DependencyProperty.Register("ScrollOffset", typeof(double), typeof(CustJournal), new UIPropertyMetadata(CustJournal.ScrollOffsetValueChanged));

        public double ScrollOffset
        {
            get { return (double)GetValue(ScrollOffsetProperty); }

            set { SetValue(ScrollOffsetProperty, value); }
        }

        private static void ScrollOffsetValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CustJournal myClass = (CustJournal)d;
            myClass.myScrollViewer.ScrollToVerticalOffset((double)e.NewValue);


			//Console.WriteLine(e.OldValue + " => " + e.NewValue);

			// if oldvalue is <= 0 then don't display the ^ arrow.
			if (Convert.ToDouble(e.NewValue) <= 0) {
				myClass.more_content_up.Visibility = Visibility.Hidden;
				myClass.more_content_down.Visibility = Visibility.Visible;
			}
			else if (Convert.ToDouble(e.NewValue) >= myClass.myScrollViewer.ScrollableHeight) {
				myClass.more_content_down.Visibility = Visibility.Hidden;
				myClass.more_content_up.Visibility = Visibility.Visible;
			}
			else {
				myClass.more_content_up.Visibility = Visibility.Visible;
				myClass.more_content_down.Visibility = Visibility.Visible;
			}
        }
        
        public CustJournal()
		{
			this.InitializeComponent();
		}

		public void UpdateItemList(ObservableCollection<playlistData> items)
        {
            this.listItems.ItemsSource = null;
            this.listItems.ItemsSource = items;
        }

		/*
		public void UpdateItemList(ObservableCollection<Product> items)
		{

			this.listItems.ItemsSource = null;
			this.listItems.ItemsSource = items;

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
                this.BeginAnimation(CustJournal.ScrollOffsetProperty, scrollAnimation);
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



				if (Math.Abs((mouseDragCurrentPoint.X - this.mouseDragStartPoint.X)) < 5) {
					//Console.WriteLine
					//Console.WriteLine("TEST");
					//Console.WriteLine(e.Timestamp);


					/*FIXME:
					 * this doens't work properly because we need to do things on the SECOND CLICK
					 */

					
					//return;
					
					/*
					if (this.OLD_TIMESTAMP == 0) {
						this.OLD_TIMESTAMP = e.Timestamp;
					}
					else {
						int difference = (e.Timestamp - this.OLD_TIMESTAMP);
						if (difference < 250) {
							//Console.WriteLine("TEST");
							//this.ReleaseMouseCapture();
						}
						this.OLD_TIMESTAMP = 0;
						return;
					}
					 */
					//return;
					/*
					//base.is
					MouseButtonEventArgs test = new MouseButtonEventArgs(e.MouseDevice, e.Timestamp,MouseButton.Left);
					if (test.ClickCount >= 2) {
						Console.WriteLine("TEST");
						//this.ReleaseMouseCapture();
						return;
					}
					 * */
				}
				



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
				Logging.WriteToLog("Check touchscreen setting. Click on touch seems to cause some problems, switch it to normal! \r\n" + ex.TargetSite + " => " + ex.Message + "\r\n OnPreviewMouseUp -- cj (audio/video playlists)");
			}


            //base.OnPreviewMouseUp(e);
        }

        #endregion







		private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
		{
			/*
			if (e.ClickCount >= 2) {
				// change to this song!
				Grid child = (Grid)sender;
				ListView listView = ItemsControl.ItemsControlFromItemContainer(child) as ListView;

				int index = listView.ItemContainerGenerator.IndexFromContainer(child);
				Console.WriteLine(index);

			}
			 * */
		}

		public void listItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			ListView listView = (ListView)sender;
			// only make the change if we're selecting something an index that IS NOT currently playing
			//	this is in hopes that we can ignore the forware/reverse presses that will start the next index, and then afterwards updated selectedIndex to match

			// if we're trying to start the first song (index 0) after loading the application, it will not start playing song 0
			//	that's because selectedIndex will be 0 and nowplaying_id will also be 0.
			//	check to see if the audioSystem is initialized. if not, then just play whatever we change to otherwise do our regular check

			if (audioSystem.channel == null) {
				Window1.Instance.playNowPlayingID(listView.SelectedIndex);
			}
			else {
				if (listView.SelectedIndex != Window1.Instance.media_nowplaying_id) {
					Window1.Instance.playNowPlayingID(listView.SelectedIndex);
				}
			}
		}

		private void more_content_up_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.ClickCount >= 2) {
				// scroll to the top
				more_content_up.Visibility = Visibility.Hidden;
				more_content_down.Visibility = Visibility.Visible;
				myScrollViewer.ScrollToTop();
			}
		}

		private void more_content_down_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.ClickCount >= 2) {
				// scroll to the bottom
				more_content_up.Visibility = Visibility.Visible;
				more_content_down.Visibility = Visibility.Hidden;
				myScrollViewer.ScrollToBottom();
			}
		}




	}
}