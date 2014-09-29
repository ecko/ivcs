/*
 *		Window1 
 * This is the main class used by the IVCS. It is the first thing loaded so it sets up and initializes various items.
 * Especially useful for the UI so things are not loaded repeatedly.
 * 
 * Lots of functions are defined within here. Some have been depricated or moved into external files.
 * 
 * 
 */


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
using KineticScrollingPrototype;
using System.Windows.Media.Animation;
using System.Collections.ObjectModel;

using System.Diagnostics;

namespace wpfBuild
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
		private string sqldatasource = @"C:\ivcs\ivcs.sqlite";
		private string ivcs_install_dir = @"C:\ivcs\";
		private const string DBNAME = @"ivcs.sqlite";
        
        private Byte media_status_timerstyle = 0;

		private static Window1 instance;

		// Instance allows us to globally reference the Window1 object without having to re-initialize it inside of other classes
		//	this is useful bbecause some things should only be loaded the one time in the Window1 Class.
		public static Window1 Instance
		{
			get
			{
				//if (instance == null) {
					//instance = new Singleton();
				//}
				return instance;
			}
		}


		public ObservableCollection<wpfBuild.data.mediaFolderListData> mediaFolderList = new ObservableCollection<wpfBuild.data.mediaFolderListData>();

        /// <summary>
        /// *** outdated, uses the right # now (see below) ** I believe this is actually the *NEXT* song ID instead of the current one.
		/// no longer true if using playNowPlayingID as it uses the _NowPlaying collection (0 based)
        /// </summary>
        /// lolz can't be a byte (only 256 songs) now we can have 65k+
        public UInt16 media_nowplaying_id = 0;
		private const Boolean FORWARD = true;
		private const Boolean REVERSE = false;
		private Boolean media_repeat = false;
		private Boolean media_shuffle = false;

        private bool enable_songtime_update = true;

        private const int SPECTRUMSIZE = 512;
        private const int WAVEDATASIZE = 256;
        private float[] spectrum = new float[SPECTRUMSIZE];
        private float[] wavedata = new float[WAVEDATASIZE];

        float GRAPHICWINDOW_HEIGHT = 0;
        float GRAPHICWINDOW_WIDTH = 0;

        int numchannels = 0;
        int dummy = 0;
        FMOD.SOUND_FORMAT dummyformat = FMOD.SOUND_FORMAT.NONE;
        FMOD.DSP_RESAMPLER dummyresampler = FMOD.DSP_RESAMPLER.LINEAR;


        DispatcherTimer ani = new DispatcherTimer();
        //DispatcherTimer peakDrops = new DispatcherTimer();

        //private Line[] lines = new Line[512];
		private Line[] lines = new Line[subband_count];
        //Brush channelBrush = Brushes.Black;
        //private Line myLine = null;
        int[] highest = new int[512];

        Pen whitePen = new Pen();
        Pen gridPen = new Pen();

        public Window1()
        {
			Logging.WriteToLog("Application started: " + DateTime.Now.ToString());

			InitializeComponent();

			Window1.instance = this;

            tabControl1.SelectionChanged += new SelectionChangedEventHandler(tabControl1_SelectionChanged);

            DispatcherTimer timerSeconds = new DispatcherTimer();
			timerSeconds.Interval = TimeSpan.FromMilliseconds(500);
            timerSeconds.Tick += new EventHandler(timerSeconds_Tick);
            timerSeconds.IsEnabled = true;

           // ani.Interval = TimeSpan.FromMilliseconds(115);
            ani.Interval = TimeSpan.FromMilliseconds(50);
            ani.Tick += new EventHandler(ani_Tick2);
            //ani.Start();

            //Pen whitePen = new Pen();
            whitePen.Brush = Brushes.WhiteSmoke;
            whitePen.Thickness = 0.5;
            whitePen.Freeze();

            gridPen.Brush = Brushes.Black;
            gridPen.Thickness = 0.5;
            gridPen.DashStyle = DashStyles.Dash;
            gridPen.DashCap = PenLineCap.Round;
            //myLine.StrokeDashArray = dashes;
            //myLine.StrokeDashCap = PenLineCap.Round;
            gridPen.Freeze();

			BrushConverter conv = new BrushConverter();
			SolidColorBrush brush = conv.ConvertFromString("#990A0A0A") as SolidColorBrush;

            //for (int i = 0; i < 512; i++)
			for (int i = 0; i < subband_count; i++)
            {
				Line line = new Line();
                //line.Stroke = System.Windows.Media.Brushes.Yellow;
				line.Stroke = brush;
                line.StrokeThickness = 5;
                line.IsHitTestVisible = false;

				line.X1 = line.X2 = (i * 7);
				line.Y1 = line.Y2 = 250;
                lines[i] = line;

                // add all the lines to the canvas
                // then we just manipulate the height of the line instead of recreating them everytime!
				media_visualization_2.Children.Add(lines[i]);

            }
            //peakDrops.Interval = TimeSpan.FromMilliseconds(

            /*
             * Homepage Buttons
             * idea: wouldn't this be better in the actual xaml?
             */
            //hp_audio.Click += new RoutedEventHandler(hp_audio_Click);
            hp_settings.Click += new RoutedEventHandler(hp_settings_Click);
			//hp_gps.Click += new RoutedEventHandler(hp_gps_Click);
            


            /*
             * Media dock controls
             */
            //this.media_play.Click += new RoutedEventHandler(media_play_Click);
            this.media_status_songtime.MouseDown += new MouseButtonEventHandler(media_status_songtime_MouseDown);

        }

		/// <summary>
		/// create our DB & tables if the database doesn't already exist
		/// </summary>
		private void firstRunSetup()
		{
			if (Directory.Exists(@"C:\ivcs") == false) {
				Directory.CreateDirectory(@"C:\ivcs");
			}

			this.sqldatasource = @"Data Source=" + this.sqldatasource;
			Dictionary<string, string> tables = new Dictionary<string, string>();

			// build the tables
			//	1 - audio
			//	2 - media_folders
			//	3 - media_status
			//	4 - now_playing
			//	5 - video

			tables.Add("audio", @"CREATE TABLE 'audio' ('ID' INTEGER PRIMARY KEY ,'path' TEXT,'ID_mediafolder' INTEGER DEFAULT '' ,'album' TEXT DEFAULT '' ,'artists' TEXT DEFAULT '' ,'date' INTEGER DEFAULT '' ,'genres' TEXT DEFAULT '' ,'title' TEXT DEFAULT '' ,'tracknumber' INTEGER DEFAULT '' ,'times_played' INTEGER NOT NULL  DEFAULT '0' ,'length' INTEGER NOT NULL  DEFAULT '0', 'rating' INTEGER NOT NULL  DEFAULT 0)");

			tables.Add("media_folders", "CREATE TABLE 'media_folders' ('ID' INTEGER PRIMARY KEY ,'path' TEXT,'recursive' INTEGER NOT NULL  DEFAULT '0' ,'num_folders' INTEGER NOT NULL  DEFAULT '0' ,'num_files' INTEGER NOT NULL  DEFAULT '0' ,'type' text DEFAULT '' )");

			//tables.Add("media_status", "");

			// todo: check that this line works, I replaced the ` with ' characters
			tables.Add("now_playing", "CREATE TABLE 'now_playing' ('ID' INTEGER PRIMARY KEY ,'position_seconds' INTEGER NOT NULL  DEFAULT '0' ,'media_type' INTEGER NOT NULL  DEFAULT '1' ,'media_ID' INTEGER NOT NULL  DEFAULT '0' )");

			tables.Add("video", "CREATE TABLE 'video' ('ID' INTEGER PRIMARY KEY  AUTOINCREMENT  NOT NULL , 'path' TEXT NOT NULL , 'ID_mediafolder' INTEGER NOT NULL  DEFAULT 0, 'title' TEXT, 'times_played' INTEGER NOT NULL  DEFAULT 0, 'length' INTEGER NOT NULL  DEFAULT 0)");

			// Create a connection and a command
			using (SQLiteConnection cnn = new SQLiteConnection(sqldatasource)) {
				cnn.Open();
				using (SQLiteTransaction dbTrans = cnn.BeginTransaction()) {
					using (SQLiteCommand cmd = cnn.CreateCommand()) {
						foreach (KeyValuePair<String, String> entry in tables) {
							//do something with entry.Value or entry.Key
							Logging.WriteToLog("Added table: '" + entry.Key + "' to database");

							cmd.CommandText = entry.Value;
							cmd.ExecuteNonQuery();
						}
					}
					dbTrans.Commit();
				}
				cnn.Close();
			}


		}

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
			Logging.WriteToLog("Window Loaded");

			this.tabControl1.SelectedIndex = 0;

			// check the DB path to make sure it exists
			//	maybe check to make sure that the tables also exist?
			if (File.Exists(this.sqldatasource) == true) {
				this.sqldatasource = @"Data Source=" + this.sqldatasource;
			}
			else {
				firstRunSetup();
			}

			wpfBuild.Properties.Settings.Default.datasource = sqldatasource;

			refresh_nowplaying();
            //draw_grids();

			createVisAnimation();


			Debug.WriteLine("************************** Form has loaded **************************");
			Logging.WriteToLog("Window Load Complete");

        }

        /// <summary>
        /// As the program is closing, write some status info to the DB so we know what/where to resume from
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void window_main_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // todo: song resume after closing
			// write the current position to the current song, or to a special table with just the ID# and the position
			// then when we load we will will load that song.
			/*
            if (audioSystem.channel != null)
            {

                using (SQLiteConnection cnn = new SQLiteConnection(sqldatasource))
                {
                    using (SQLiteCommand cmd = cnn.CreateCommand())
                    {
                        cnn.Open();
                        //cmd.CommandText = "SELECT * FROM now_playing, audio WHERE (now_playing.media_type=1) AND (now_playing.media_ID=audio.ID)";
                        cmd.CommandText = @"UPDATE media_status SET (media_id = @media_id, position = @position, status = @status) WHERE id = 1";

                        //cmd.Parameters.AddWithValue("@media_id", audioSystem.media_info.table_Id);
                        // TODO: use the above to create this tool
                        //      must use the ID from the audio table so we can ref the song.

                        cmd.ExecuteNonQuery();
                    }
                    cnn.Close();
                }
            }
             */

			Logging.WriteToLog("Application closing\r\n\r\n");

        }

		/// <summary>
		/// Output the duration
		/// </summary>
		/// <param name="stopTime"></param>
		/// <param name="startTime"></param>
		public void Duration(DateTime stopTime, DateTime startTime)
		{
			TimeSpan duration = stopTime - startTime;
			Debug.WriteLine("seconds:" + duration.TotalSeconds);
			Debug.WriteLine("milliseconds:" + duration.TotalMilliseconds);
		}

        //private void draw_grids(object sender, RoutedEventArgs e)
        private void draw_grids()
        {
            /*
            DrawingVisual dv = new DrawingVisual();

            using (DrawingContext ctx = dv.RenderOpen())
            {
                for (int n = 0; n < 21; n++)
                {
                    ctx.DrawLine(gridPen, new Point(0, (1 / 20.0) * 512 * n), new Point(512, (1 / 20.0) * 512 * n));
                }

                // Vertical Lines
                for (int n = 0; n < 20; n++)
                {	// 21 -> 20
                    ctx.DrawLine(gridPen, new Point((1 / 20.0) * 512 * n, 0), new Point((1 / 20.0) * 512 * n, 512));
                }

            }

            RenderTargetBitmap rtb = new RenderTargetBitmap(512, 512, 96, 96, PixelFormats.Pbgra32);
            rtb.Render(dv);

            ImageBrush ib = new ImageBrush(rtb);
            ib.Freeze();

            media_visualization_grids.Background = ib;
            */

            // this should work
            // only draw the lines once, all 159 of them...
        //    if (media_visualization_grids.Children.Count < 159)
        //    {

                /*
                 * For some reason even having grids on the screen causes another +25% cpu usage!
                 * 
                int x;
                for (x = 0; x <= media_visualization_grids.ActualWidth; x += 5)
                {
                    // x
                    //   for (y = 1; y < 200; y += 5)
                    //   {
                    // Add a Line Element
                    Line myLine = new Line();
                    myLine.Stroke = Brushes.Gray;

                    DoubleCollection dashes = new DoubleCollection();
                    dashes.Add(2);
                    dashes.Add(2);
                    myLine.StrokeDashArray = dashes;
                    myLine.StrokeDashCap = PenLineCap.Round;

                    myLine.Opacity = 0.5;

                    myLine.X1 = x;
                    myLine.X2 = myLine.X1;
                    myLine.Y1 = 0;
                    myLine.Y2 = media_visualization_grids.ActualHeight;
                    //myLine.HorizontalAlignment = HorizontalAlignment.Left;
                    //myLine.VerticalAlignment = VerticalAlignment.Center;
                    myLine.StrokeThickness = 1;

                    media_visualization_grids.Children.Add(myLine);

                    //   }
                }
                 */

         //       GRAPHICWINDOW_HEIGHT = (float)media_visualization_grids.ActualHeight;
         //       GRAPHICWINDOW_WIDTH = (float)media_visualization_grids.ActualWidth;
         //   }
        }

        private void media_visualization_Loaded(object sender, RoutedEventArgs e)
        {
            //media_visualization_stuff();

            // Define the Image element
         //   _random.Stretch = Stretch.None;
         //   _random.Margin = new Thickness(20);

            // Add the Image to the parent StackPanel
         //   if (_random.Parent == null)
         //   {
         //       media_visualization.Children.Add(_random);
         //   }

            GRAPHICWINDOW_HEIGHT = (float)media_visualization.ActualHeight;
            GRAPHICWINDOW_WIDTH = (float)media_visualization.ActualWidth;
        }

        /// <summary>
        /// Only enable the animation timer if we're on the animation page!
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void now_playingTabs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (now_playingTabs.SelectedIndex == 0)
            {
                ani.IsEnabled = true;
				visAniTimer.IsEnabled = false;
            }
			else if (now_playingTabs.SelectedIndex == 1) {
				visAniTimer.IsEnabled = true;
				ani.IsEnabled = false;
			}
			else {
				ani.IsEnabled = false;
				visAniTimer.IsEnabled = false;
			}
        }

        void ani_Tick2(object sender, EventArgs e)
        {
            if ((audioSystem.channel == null) || (audioSystem.paused != false) || (audioSystem.playing != true) || (now_playingTabs.SelectedIndex != 0))
            {
                return;
            }


            int count = 0;
            int count2 = 0;

            audioSystem.system.getSoftwareFormat(ref dummy, ref dummyformat, ref numchannels, ref dummy, ref dummyresampler, ref dummy);

            DrawingVisual dv = new DrawingVisual();
            using (DrawingContext ctx = dv.RenderOpen())
            {
                for (count = 0; count < numchannels; count++)
                {
                    float max = 0;

                    audioSystem.system.getSpectrum(spectrum, SPECTRUMSIZE, count, FMOD.DSP_FFT_WINDOW.TRIANGLE);

                    //for (count2 = 0; count2 < 255; count2++)
                    //{
                    for (count2 = 0; count2 < 512; count2++)
                    {
                        if (max < spectrum[count2])
                        {
                            max = spectrum[count2];
                        }
                    }
                    //}

                    if (max > 0.0001f)
                    {
                        /*
                            The upper band of frequencies at 44khz is pretty boring (ie 11-22khz), so we are only going to display the first 256 frequencies, or (0-11khz) 
                        */
                        //for (count2 = 0; count2 < 255; count2++)
                        // part of it might be rendering off screen? or is it scaling the background to fit
                        //for (count2 = 0; count2 < 512; count2++)
                        for (count2 = 0; count2 < 275; count2++)
                        {
                            float height;

                            height = spectrum[count2] / max * GRAPHICWINDOW_HEIGHT;

                            if (height >= GRAPHICWINDOW_HEIGHT)
                            {
                                height = GRAPHICWINDOW_HEIGHT - 1;
                            }

                            if (height < 0)
                            {
                                height = 0;
                            }

                            height = GRAPHICWINDOW_HEIGHT - height;

                            if (height > highest[count2])
                            {
                                // todo:  instead of adding to the background in the loop, what about putting the heights into an array,
                                // then using that array outside to plot all the lines?
                                // that's probably how we will have to do it if we want to work with peak dropoffs
                                ctx.DrawLine(whitePen, new Point(count2, 275), new Point(count2, height));
                            }
                        }
                    }
                }


            }

            RenderTargetBitmap rtb = new RenderTargetBitmap(400, 275, 96, 96, PixelFormats.Pbgra32);

            rtb.Render(dv);

            ImageBrush ib = new ImageBrush(rtb);
            ib.Freeze();

            media_visualization.Background = ib;
        }


        /* SO MUCH LESS CPU USAGE! */

        void ani_Tick(object sender, EventArgs e)
        {
            // we shouldn't have to worry about this check, as the timer gets paused with the pause button.
            // now we do worry about it because the check against the "visualization" tab has been addded
            if ((audioSystem.channel != null) && (audioSystem.paused == false) && (audioSystem.playing == true) && (now_playingTabs.SelectedIndex == 0))
            {
                int count = 0;
                int count2 = 0;

                audioSystem.system.getSoftwareFormat(ref dummy, ref dummyformat, ref numchannels, ref dummy, ref dummyresampler, ref dummy);

                /*
                        DRAW SPECTRUM
                */
                //int perchannel_512 = 0;

				// using a drawing visual helps to reduce some of the processing required by the CPU!

                DrawingVisual dv = new DrawingVisual();
                using (DrawingContext ctx = dv.RenderOpen())
                {

                    

                    /*
                    LinearGradientBrush lgb = new LinearGradientBrush();
                    lgb.GradientStops.Add(new GradientStop(Colors.CornflowerBlue, 0));
                    lgb.GradientStops.Add(new GradientStop(Colors.DarkOrchid, 1));
                    lgb.StartPoint = new Point(0, 0);
                    lgb.EndPoint = new Point(0, 1);
                    lgb.Freeze();
                    */
                    //ctx.DrawRectangle(lgb, null, new Rect(0, 0, 512, 512));



                    for (count = 0; count < numchannels; count++)
                    {
                        float max = 0;

                        //audioSystem.sound.@lock(
                        audioSystem.system.getSpectrum(spectrum, SPECTRUMSIZE, count, FMOD.DSP_FFT_WINDOW.TRIANGLE);

                        //for (count2 = 0; count2 < 255; count2++)
                        //{
                        for (count2 = 0; count2 < 512; count2++)
                        {
                            if (max < spectrum[count2])
                            {
                                max = spectrum[count2];
                            }
                        }
                        //}

                        if (max > 0.0001f)
                        {
                            /*
                                The upper band of frequencies at 44khz is pretty boring (ie 11-22khz), so we are only going to display the first 256 frequencies, or (0-11khz) 
                            */
                            //for (count2 = 0; count2 < 255; count2++)
                            for (count2 = 0; count2 < 512; count2++)
                            {
                                float height;

                                height = spectrum[count2] / max * GRAPHICWINDOW_HEIGHT;

                                if (height >= GRAPHICWINDOW_HEIGHT)
                                {
                                    height = GRAPHICWINDOW_HEIGHT - 1;
                                }

                                if (height < 0)
                                {
                                    height = 0;
                                }

                                height = GRAPHICWINDOW_HEIGHT - height;

                                if (height > highest[count2])
                                {
                                    //lines[count2].Stroke = channelBrush;
                                    /*
                                    lines[count2].X1 = count2;
                                    lines[count2].X2 = lines[count2].X1;
                                    lines[count2].Y1 = GRAPHICWINDOW_HEIGHT;
                                    lines[count2].Y2 = height;
                                     */


                                    //media_visualization



                                    //LinearGradientBrush lgb = new LinearGradientBrush();
                                    //lgb.GradientStops.Add(new GradientStop(Colors.CornflowerBlue, 0));
                                    //lgb.GradientStops.Add(new GradientStop(Colors.DarkOrchid, 1));
                                    //lgb.StartPoint = new Point(0, 0);
                                    //lgb.EndPoint = new Point(0, 1);
                                    //lgb.Freeze();

                                    //ctx.DrawRectangle(lgb, null, new Rect(0, 0, 1024, 1024));

                                    //Pen whitePen = new Pen();
                                    //whitePen.Brush = Brushes.DarkGray;
                                    //whitePen.Thickness = 4;
                                    //whitePen.Freeze();




                                    //ctx.DrawLine(whitePen, new Point(count2, count2), new Point(0, height));
                                    ctx.DrawLine(whitePen, new Point(count2, GRAPHICWINDOW_HEIGHT), new Point(count2, height));

                                    //ctx.DrawRectangle(

                                    //for (int n = 0; n < 21; n++)
                                    //{
                                    //    ctx.DrawLine(whitePen, new Point(0, (1 / 20.0) * 1024 * n), new Point(1024, (1 / 20.0) * 1024 * n));
                                    //}

                                    // Vertical Lines
                                    //for (int n = 0; n < 20; n++)
                                    //{	// 21 -> 20
                                    //    ctx.DrawLine(whitePen, new Point((1 / 20.0) * 1024 * n, 0), new Point((1 / 20.0) * 1024 * n, 1024));
                                    //}

                                    //}



                                }
                                //perchannel_512++;
                            }
                        }
                    }

                    
                }

                RenderTargetBitmap rtb = new RenderTargetBitmap((int)GRAPHICWINDOW_WIDTH, (int)GRAPHICWINDOW_HEIGHT, 96, 96, PixelFormats.Pbgra32);

                rtb.Render(dv);

                ImageBrush ib = new ImageBrush(rtb);
                ib.Freeze();

                media_visualization.Background = ib;
            }
        }

        void media_status_songtime_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // scroll through our display time styles
            if (media_status_timerstyle >= 3)
            {
                media_status_timerstyle = 0;
            }
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                // we ++
                media_status_timerstyle++;
            }
				// rightclick on the touchscreen isn't advised!
            else if (e.RightButton == MouseButtonState.Pressed)
            {
                // we --

                if (media_status_timerstyle < 1)
                {
                    media_status_timerstyle = 3;
                }
                media_status_timerstyle--;
            }
        }

        void media_play_Click(object sender, RoutedEventArgs e)
        {
            if (audioSystem.system == null)
            {
                if (_nowPlayingList.Count < 1)
                {
                    MessageBox.Show("Please add something to play!");
                    return;
                }

             //   media_nowplaying_id = 1;
                // because of the way media_skip works, we have to pass the next song AFTER the one we want, then tell it to backtrack to play
                // our correctly selected item
             //   media_skip(-1);
				// todo: will have to grab the last ID of the song playing from when program was closed and then put it into a var here for this to use

				if (this.media_shuffle == true) {
					// just skip it in any direction because we're shuffling it.
					media_skip(FORWARD);
				}
				else {
					playNowPlayingID(0);
				}
            }
            else
            {
                audioSystem.paused = !audioSystem.paused;
                // idea: I don't think the animation should be paused when muted. because it's still technically playing
                //ani.IsEnabled = !ani.IsEnabled;
            }
            updateMedia_status();
        }

		/// <summary>
		/// Set all the proper values for details about the media (top bar)
		/// </summary>
        private void updateMedia_status()
        {
            //media_status_title.Content = audioSystem.media_info.title + "  (" + audioSystem.media_info.artists + ")";
			media_status_title.Text = audioSystem.media_info.title;

			// do a check here to make sure we have something to display!
			if (string.IsNullOrEmpty(audioSystem.media_info.artists) == false) {
				media_status_artists.Text = "[" + audioSystem.media_info.artists + "]";
				media_status_artists.Visibility = Visibility.Visible;
			}
			else {
				media_status_artists.Visibility = Visibility.Collapsed;
			}

			media_status_album.Text = audioSystem.media_info.album;

			// UPDATE THE RATING STARS
			updateRatingDisplays(_nowPlayingList[media_nowplaying_id].SongRating, media_status_stars);

			if (audioSystem.system != null)
			{
				String smallicon_path = "Resources/images/silk/control_play.png";

				if (audioSystem.paused == true)
				{
					smallicon_path = "Resources/images/silk/control_pause.png";
				}

				media_status_statusicon.Source = new BitmapImage(new Uri(@smallicon_path, UriKind.Relative));
				media_status_filetype.Text = audioSystem.media_info.codec;
				image1.ImageSource = image2.ImageSource = audioSystem.media_info.albumart;
				image1_border.Visibility = Visibility.Visible;

				updateMedia_playlistDetails();
			}
        }

        private void updateMedia_playlistDetails()
        {
			// version 2
			mediainfo_title.Text = audioSystem.media_info.title;
			mediainfo_filepath.Text = audioSystem.media_info.filepath;

			mediainfo_artist.Text = audioSystem.media_info.artists;
			mediainfo_album.Text = audioSystem.media_info.album;
			mediainfo_trackname.Text = audioSystem.media_info.title;
			mediainfo_tracknumber.Text = audioSystem.media_info.tracknumber.ToString();
			mediainfo_genre.Text = audioSystem.media_info.genres;
			mediainfo_year.Text = audioSystem.media_info.date.ToString();

			mediainfo_playlistnumber.Text = (media_nowplaying_id + 1).ToString();
			// fixme: this gives me the file mime type, which probably won't work correctly for everything
			mediainfo_bitrate.Text = audioSystem.media_info.bitrate + " kbps " + audioSystem.media_info.codec;
			mediainfo_channels.Text = audioSystem.media_info.channels.ToString();

			// get the size of the file
			FileInfo fi = new FileInfo(audioSystem.media_info.filepath);
			mediainfo_filesize.Text = (fi.Length / 1024f / 1024f).ToString("0.00") + " MB";

			mediainfo_playcount.Text = _nowPlayingList[media_nowplaying_id].TimesPlayed.ToString();
			// IDEA: add 2 columsn for last played date/time
			//	column 1 will be the current date/time when the song starts playing, and then column2 will be the previous time, and we just keep bumping from 1 -> 2 every time we play
			//mediainfo_lastplayed.Text = "####";

			// UPDATE THE RATING STARS
			updateRatingDisplays(_nowPlayingList[media_nowplaying_id].SongRating, mediainfo_stars);


        }

        void tabControl1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //throw new NotImplementedException();
            if (this.tabControl1.SelectedIndex == 0)
            {
                this.homeExit.Content = "Exit";
				// show the ivcs stamp again
				bg_image_stamp.Visibility = Visibility.Visible;
            }
            else
            {
                this.homeExit.Content = "Home";
				bg_image_stamp.Visibility = Visibility.Hidden;
            }
			TabControl tc = (TabControl)sender;
			TabItem selectedTab = (TabItem)tc.SelectedItem;
			current_page_title.Text = selectedTab.Header.ToString();
        }

        void hp_audio_Click(object sender, RoutedEventArgs e)
        {
            this.tabControl1.SelectedIndex = 1;
        }

		private void hp_manage_media_Click(object sender, RoutedEventArgs e)
		{
			tabControl1.SelectedIndex = 2;
		}

        private void hp_video_Click(object sender, RoutedEventArgs e)
        {
            this.tabControl1.SelectedIndex = 3;
        }

        void hp_settings_Click(object sender, RoutedEventArgs e)
        {
            this.tabControl1.SelectedIndex = 7;
        }

		void hp_gps_Click(object sender, RoutedEventArgs e)
		{
			this.tabControl1.SelectedIndex = 9;
		}

		private void hp_obd_Click(object sender, RoutedEventArgs e)
		{
			this.tabControl1.SelectedIndex = 4;
		}

		private void hp_control_Click(object sender, RoutedEventArgs e)
		{
			this.tabControl1.SelectedIndex = 5;
		}

        void homeExit_Click(object sender, RoutedEventArgs e)
        {
            if (this.tabControl1.SelectedIndex == 0)
            {
                Application.Current.Shutdown();
            }
            else
            {
                //this.homeExit.Content = "Exit";
                this.tabControl1.SelectedIndex = 0;
            }
        }

        void timerSeconds_Tick(object sender, EventArgs e)
        {
			DateTime now = DateTime.Now;
			status_time.Text = now.ToLongTimeString();
			status_date.Text = now.ToShortDateString();

			//if ((audioSystem.channel != null) && (audioSystem.paused == false) && (audioSystem.playing == true)) {
			// big bug fixed!
			//	when a song reached the end, FMOD would "pause" the status before we could poll the position to see that it's at the end
			//	(the closest I got was off by 1 value)
			//		sooooo.... don't worry if it's paused, keep polling the current position.  if it's at the end it will give us the MAX value which we're looking for!
			if (audioSystem.channel != null) {
				uint ms_position = 0;
				double test2 = 0;
				uint minute = 0;
				uint second = 0;
				uint ms_time_diff = 0;

				audioSystem.channel.getPosition(ref ms_position, FMOD.TIMEUNIT.MS);

				test2 = (ms_position / 1000D);
				minute = (ms_position / 1000 / 60);
				second = (ms_position / 1000 % 60);

				if (enable_songtime_update == true) {
					media_position.Value = test2;
				}


				if (media_status_timerstyle == 1) {
					media_status_songtime.Text = minute.ToString() + ":" + second.ToString("00");
				}
				else if (media_status_timerstyle == 2) {
					ms_time_diff = audioSystem.media_info.length - ms_position;
					media_status_songtime.Text = '-' + (ms_time_diff / 1000 / 60).ToString() + ':' + (ms_time_diff / 1000 % 60).ToString("00");
				}
				else {
					media_status_songtime.Text = minute.ToString() + ":" + second.ToString("00") + "/" + (audioSystem.media_info.length / 1000 / 60).ToString() + ":" + (audioSystem.media_info.length / 1000 % 60).ToString("00");
				}

				if (mediainfo_media_progress.IsLoaded == true) {
					// make the progress rectangle's width a percentage of the media's current position
					mediainfo_media_progress.Width = (mediainfo_container.ActualWidth) * (media_position.Value / media_position.Maximum);
				}
			}
        }


        private void media_position_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
#if DEBUG
           // Console.WriteLine(media_position.Value + " / " + media_position.Maximum + "; ");

#endif

            if (media_position.Value == media_position.Maximum)
            {
                // if we're at the max, then we advance to the next song
                // move 1 song forward
               // media_skip(1);

				if (this.media_repeat == true) {
					playNowPlayingID(media_nowplaying_id);
				}
				else {
					media_skip(FORWARD);
				}
            }

            // as soon as we move it, we change the song position, and then we take off the focus on this element
            if (audioSystem.channel != null)
            {

                media_position_rect.Width = (media_position.ActualWidth) * (media_position.Value / media_position.Maximum);
            }

        }

        private void media_position_KeyUp(object sender, KeyEventArgs e)
        {
            media_position_LostMouseCapture(sender, null);
        }

        private void media_position_GotMouseCapture(object sender, MouseEventArgs e)
        {
            if (audioSystem.channel != null)
            {
                enable_songtime_update = false;
            }
        }

        private void media_position_LostMouseCapture(object sender, MouseEventArgs e)
        {
            if (audioSystem.channel != null)
            {
                updateSongPosition();
                //scrollingTime.Visible = false;
                enable_songtime_update = true;
            }
        }

        private void updateSongPosition()
        {
            //uint song_position = (uint)media_position.Value * 1000;
            audioSystem.channel.setPosition((uint)media_position.Value * 1000, FMOD.TIMEUNIT.MS);
            //songTimeTrackBar.Focus();
        }

        private void media_volume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (audioSystem.channel != null)
            {
                audioSystem.channel.setVolume((float)media_volume.Value / 100f);
            }
			//Console.WriteLine(media_volume.Maximum);

			// weird init errors if I don't wait until this is set to 100... or wait until it's loaded (loaded comes a bit after initialized so it's better to use loaded)
			if (media_volume.IsLoaded == true)
			{
				media_volume_label.Content = media_volume.Value.ToString("0") + "%";

				if (media_volume.Value <= media_volume.Minimum)
				{
					media_volume_down.IsEnabled = false;
					media_volume_image.Source = new BitmapImage(new Uri(@"Resources/images/silk/sound_none.png", UriKind.Relative));
				}
				else if (media_volume.Value >= media_volume.Maximum)
				{
					media_volume_up.IsEnabled = false;
				}
				else
				{
					media_volume_down.IsEnabled = true;
					media_volume_up.IsEnabled = true;

					if (media_volume.Value <= 25d)
					{
						media_volume_image.Source = new BitmapImage(new Uri(@"Resources/images/silk/sound_low.png", UriKind.Relative));
					}
					else
					{
						// todo: is it a waste of resources to update the image *EVERY* time even if it's not low?  (from some testing I couldn't see much/any cpu increase)
						//		it seems like the slider itself causes HIGH (30%+) usage (just by sliding it)
						media_volume_image.Source = new BitmapImage(new Uri(@"Resources/images/silk/sound.png", UriKind.Relative));
					}
				}
			}

        }

        private void TabControl_Loaded(object sender, RoutedEventArgs e)
        {
            // we'll save some power if we check to make sure the control is visible before doing work.
            // otherwise, this would get fired when the the app first loads, and then when we open this panel
            TabControl tcontrol = (TabControl)sender;
            if (tcontrol.IsVisible == true)
            {
                refreshMediaFolderList();
            }
        }

        private void refreshMediaFolderList()
        {
			// todo: fix to allow audio/video/image Icon types as well.
			// FIXME: this gets called many times.  check if anything could be cached so that it doesn't have to reaload so often.
			BitmapImage folder_type_image = null;

			// fixme: these paths will need to be corrected to someting other than drive C:
			//	try again to make them relative or something.
			BitmapImage folder_image_audio = new BitmapImage(new Uri("pack://application:,,,/Resources/images/silk/music.png"));
			//BitmapImage folder_image_video = new BitmapImage(new Uri(@"C:/ivcs/include/images/silk/film.png", UriKind.Relative));
			BitmapImage folder_image_video = new BitmapImage(new Uri("pack://application:,,,/Resources/images/silk/film.png"));
			//BitmapImage folder_image_images = new BitmapImage(new Uri(@"C:/ivcs/include/images/silk/picture.png", UriKind.Relative));
			BitmapImage folder_image_images = new BitmapImage(new Uri("pack://application:,,,/Resources/images/silk/picture.png"));

			mediaType type = 0;
			Boolean search_subfolders = false;
			
			// this needs to be cleared because we're now using a global var.
			mediaFolderList.Clear();

			/*
            <Image x:Name="audioIconImage" Source="Resources/images\silk\music.png" Width="17" Height="16" />
            <Image x:Name="videoIconImage" Source="Resources/images\silk\film.png" Width="17" Height="16" />
            <Image x:Name="imageIconImage" Source="Resources/images\silk\picture.png" Width="17" Height="16" />
			 */


			using (SQLiteConnection cnn = new SQLiteConnection(sqldatasource))
            using (SQLiteCommand cmd = cnn.CreateCommand())
            {
                cnn.Open();
                cmd.CommandText = "SELECT * FROM media_folders";
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    //ListViewItem ii;
                    //string[] str = null;
                    //listview_mediafolders.ItemsSource = null;

                    //System.Collections.ArrayList row = new System.Collections.ArrayList();

                    while (reader.Read())
                    {
                        /*
						str = new string[6];
                        str[0] = reader["path"].ToString();
                        str[1] = reader["recursive"].ToString();
                        str[2] = null;
                        //str[2] = reader["previews"].ToString();
                        str[3] = reader["num_folders"].ToString();
                        str[4] = reader["num_files"].ToString();

                        str[5] = reader["type"].ToString();

                        ii = new ListViewItem();
                        ii.Content = str;
                        ii.Tag = reader["ID"].ToString();
                        row.Add(ii);
						*/


						if (Convert.ToInt32(reader["recursive"]) == 1) {
							search_subfolders = true;
						}
						else {
							search_subfolders = false;
						}

						// idea: maybe look into setting FolderType in the addition at the bottom, to use a mediaType ot begin with.
						type = (mediaType)Convert.ToUInt16(reader["type"]);

						if (type == mediaType.audio) {
							folder_type_image = folder_image_audio;
						}
						else if (type == mediaType.video) {
							folder_type_image = folder_image_video;
						}
						else {
							folder_type_image = folder_image_images;
						}

						mediaFolderList.Add(new wpfBuild.data.mediaFolderListData()
						{
							ID = Convert.ToInt32(reader["ID"]),
							FolderPath = reader["path"].ToString(),
							NumFiles = Convert.ToUInt32(reader["num_files"]),
							NumFolders = Convert.ToUInt32(reader["num_folders"]),
							FolderType = Convert.ToUInt16(reader["type"]),
							SearchSubfolders = search_subfolders,
							FolderTypeImage = folder_type_image
						});
                    }
                    // idea: possibly think of adding the defer refresh stuff here if we have a lot of media folders
                    //listview_mediafolders.ItemsSource = row;
                }
                cmd.Dispose();
                cnn.Close();
            }

			mediafolderlist.UpdateItemList(mediaFolderList);
        }

        private static void ExpandRecursively(ItemsControl itemsControl, bool expand)
        {
            ItemContainerGenerator itemContainerGenerator = itemsControl.ItemContainerGenerator;

            for (int i = itemsControl.Items.Count - 1; i >= 0; --i)
            {
                ItemsControl childControl = itemContainerGenerator.ContainerFromIndex(i) as ItemsControl;
                if (childControl != null)
                    ExpandRecursively(childControl, expand);

            }

            TreeViewItem treeViewItem = itemsControl as TreeViewItem;

            if (treeViewItem != null)
                treeViewItem.IsExpanded = expand;
        }

		public void playNowPlayingID(int play_this_id) {
			if (_nowPlayingList.Count <= 0) {
				MessageBox.Show("Please add media to the now playing list");
				//media_play.IsChecked = true;
				return;
			}
			
			
			if (audioSystem.system == null) {
				audioSystem.Initialize();
			}


			// try/catch is used in case the media is somehow started somewhere else?
			//	still might caused problems though. at the very least the same song keeps repeating...
			try {
				audioSystem.media_info.filepath = _nowPlayingList[play_this_id].MediaPath;
			}
			catch {
				audioSystem.media_info.filepath = _nowPlayingList[this.media_nowplaying_id].MediaPath;
				play_this_id = this.media_nowplaying_id;
			}

			try {

				audioSystem.prepare_stream();
				audioSystem.channel.setVolume((float)media_volume.Value / 100f);

				// todo: fix this function to make it smarter (this should really be a part of the audioSystem class).. it's also not really needed
				//	only used to find the proper # of channels for the visualization, and we don't really care if there's more than 2 channels...
				updateNumberofChannels();

				media_position.Value = 0d;
				// we get the length in MS, then divide by 1000 and cast to int for our max scroll value
				//TagLib.File media = TagLib.File.Create(audioSystem.media_info.filepath);
				//media_position.Maximum = media.Properties.Duration.TotalMilliseconds / 1000d;

				//media_position.Maximum = Math.Floor(audioSystem.media_info.length / 1000d);
				media_position.Maximum = audioSystem.media_info.length / 1000d;

				// unpause the media (the stream gets created paused)
				audioSystem.paused = false;
				media_play.IsChecked = true;

				// todo: careful with this conversion, it'll make things confusing
				media_nowplaying_id = Convert.ToUInt16(play_this_id);

				// updateMeida_status check is the audioSystem is paused!
				updateMedia_status();

				// select the song that's being played in our new playlist
				// also scroll the selected song into view

				custJournal.listItems.SelectedIndex = play_this_id;

				custJournal.listItems.ScrollIntoView(custJournal.listItems.Items[play_this_id]);

				// empty the visualization array elments (reset them back to zero so they don't look funky)
				Array.Clear(energy_history_buffer, 0, subband_count);
				for (int i = 0; i < subband_count; i++) {
					lines[i].Y1 = lines[i].Y2 = 250;
				}

				ani.Start();

				// update the number of times this track has been played
				updateNowPlayingTimesPlayed(_nowPlayingList[play_this_id].ID);
			}
			catch (Exception ex) {
				Logging.WriteToLog("Failed to load audio: " + ex.TargetSite + " => " + ex.Message);
			}
		}

		/// <summary>
		/// Increment the number of times a track has been played.
		/// </summary>
		/// <param name="audio_id">Audio ID is the ID # of the audio file in the audio table of the database</param>
		private void updateNowPlayingTimesPlayed(int audio_id)
		{
			using (SQLiteConnection cnn = new SQLiteConnection(sqldatasource))
			using (SQLiteCommand cmd = cnn.CreateCommand()) {
				cnn.Open();

				cmd.CommandText = "UPDATE audio SET times_played = times_played + 1 WHERE ID = @ID";
				cmd.Parameters.AddWithValue("@ID", audio_id);
				cmd.ExecuteNonQuery();

				cmd.Dispose();
				cnn.Close();
			}

		}

		/// <summary>
		/// Move forward or reverse 1 song.
		/// </summary>
		private void media_skip(Boolean direction)
		{
			int play_id = 0;

			if (this.media_shuffle == true) {
				// if shuffle is on, randomly select the next track!  it doesn't matter which direction because it's random!
				Random RandomClass = new Random();
				play_id = RandomClass.Next(_nowPlayingList.Count);
			}
			else {
				if (direction == FORWARD) {
					if (media_nowplaying_id < (_nowPlayingList.Count - 1)) {
						play_id = media_nowplaying_id + 1;
					}
				}
				else if (direction == REVERSE) {
					if (media_nowplaying_id == 0) {
						play_id = _nowPlayingList.Count - 1;
					}
					else {
						play_id = media_nowplaying_id - 1;
					}
				}
			}

			playNowPlayingID(play_id);
		}

        private void media_next_Click(object sender, RoutedEventArgs e)
        {
            // move 1 song forward
            //media_skip(1);
			media_skip(FORWARD);
        }

        private void media_previous_Click(object sender, RoutedEventArgs e)
        {
            // move 1 song back
            //media_skip(-1);
			media_skip(REVERSE);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.tabControl1.SelectedIndex = 8;
        }

		private void refresh_nowplaying_Click(object sender, RoutedEventArgs e)
		{
			refresh_nowplaying();
		}

        private void refresh_nowplaying()
        {
            //string[] str = null;
            //this.listview_now_playing.Items.Clear();
            //ListViewItem ii;

			try {

				_nowPlayingList = new System.Collections.ObjectModel.ObservableCollection<playlistData>();
				uint ms_length = 0;
				uint minute = 0;
				uint second = 0;
				string filepath = null;

				using (SQLiteConnection cnn = new SQLiteConnection(sqldatasource))
				using (SQLiteCommand cmd = cnn.CreateCommand()) {
					cnn.Open();
					// SELECT Artists.ArtistName, CDs.Title FROM Artists, CDs WHERE Artists.ArtistID=CDs.ArtistID; 
					cmd.CommandText = "SELECT * FROM now_playing, audio WHERE (now_playing.media_type=1) AND (now_playing.media_ID=audio.ID)";
					//cmd.CommandText = "SELECT * FROM media_folders WHERE (removed = 0) AND (type = audio)";
					using (SQLiteDataReader reader = cmd.ExecuteReader()) {
						while (reader.Read()) {
							//str = new string[5];
							//str[0] = "";

							filepath = reader["path"].ToString();

							//str[0] = reader["ID"].ToString();
							//str[1] = reader["title"].ToString();
							//str[2] = reader["artists"].ToString();
							//str[3] = reader["album"].ToString();
							//str[4] = filepath;
							//str[4] = "";

							ms_length = Convert.ToUInt32(reader["length"]);
							minute = (ms_length / 1000 / 60);
							second = (ms_length / 1000 % 60);

							//str[4] = minute.ToString() + ":" + second.ToString("00");

							if (File.Exists(filepath)) {
								_nowPlayingList.Add(new playlistData()
								{
									// ID is the ID of the playlist item, media_ID is the actual media file's ID #
									ID = Convert.ToInt32(reader["media_ID"]),
									SongCoverImage = audioSystem.getAlbumArtImage(filepath),
									SongTitle = reader["title"].ToString(),
									SongAlbum = reader["album"].ToString(),
									SongArtist = reader["artists"].ToString(),
									SongLength = minute.ToString() + ":" + second.ToString("00"),
									TimesPlayed = Convert.ToUInt32(reader["times_played"]),
									MediaPath = filepath,
									SongRating = Convert.ToInt32(reader["rating"])
								});

								//ii = new ListViewItem();
								//ii.Content = str;
								//this.listview_now_playing.Items.Add(ii);
							}
						}
					}
					cmd.Dispose();
					cnn.Close();
				}

				custJournal.UpdateItemList(_nowPlayingList);
			}
			catch (Exception ex) {
				//Logging.WriteToLog("Check touchscreen setting. Click on touch seems to cause some problems, switch it to normal! \r\n" + ex.TargetSite + " => " + ex.Message + "\r\n OnPreviewMouseUp");
				MessageBox.Show(ex.TargetSite + " => " + ex.Message);
			}
        }

		public UInt16 NowPlayingMediaID
		{
			get { return this.media_nowplaying_id; }
			set { this.media_nowplaying_id = value; }
		}

        private void playlist_details_Click(object sender, RoutedEventArgs e)
        {
            updateMedia_playlistDetails();
            now_playingTabs.SelectedIndex = 2;
        }

        void subdirs_Expanded(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = (TreeViewItem)sender;
            //if (item.Items.Count == 1 && item.Items[0] == dummyNode)
            if (item.Items.Count == 1)
            {
                item.Items.Clear();
                try
                {
                    foreach (string s in Directory.GetDirectories(item.Tag.ToString()))
                    {
                        TreeViewItem subitem = new TreeViewItem();
                        
                        subitem.Header = s.Substring(s.LastIndexOf("\\") + 1);
                        //subitem.Header = s.;
                        subitem.Tag = s;
                        //subitem.Items.Add(dummyNode);
                        subitem.Items.Add(new TreeViewItem().Header = "");
                        subitem.Expanded += new RoutedEventHandler(folder_Expanded);
                        item.Items.Add(subitem);
                    }
                }
                catch (Exception) { }
            }
        }

        void folder_Expanded(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = (TreeViewItem)sender;
            //if (item.Items.Count == 1 && item.Items[0] == dummyNode)
            if (item.Items.Count == 1)
            {
                item.Items.Clear();
                try
                {
                    foreach (string s in Directory.GetDirectories(item.Tag.ToString()))
                    {
                        TreeViewItem subitem = new TreeViewItem();
                        subitem.Header = s.Substring(s.LastIndexOf("\\") + 1);
                        subitem.Tag = s;
                        subitem.FontWeight = FontWeights.Normal;
                        //subitem.Items.Add(dummyNode);
                        subitem.Expanded += new RoutedEventHandler(folder_Expanded);
                        item.Items.Add(subitem);
                    }
                }
                catch (Exception) { }
            }
        }

        private void management_treeview_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            TreeViewItem selectedNode = (TreeViewItem)management_treeview.SelectedItem;
            //Console.WriteLine(selectedNode.Parent.GetType());
            //System.Windows.Controls.TreeView
           // if ( == null)
          //  {

           // }

            // set it to null so we can check it outside this compare
            TreeViewItem parent_node = null;

            if (selectedNode.Parent.GetType() == typeof(System.Windows.Controls.TreeViewItem))
            {
                parent_node = (TreeViewItem)selectedNode.Parent;

                /*
                if (parent_node.Header.ToString() == "Playlists")
                {
                    delete_playlist.Visibility = Visibility.Visible;
                }
                else
                {
                    delete_playlist.Visibility = Visibility.Collapsed;
                }
                */
            }
            
            //MessageBox.Show(selectedNode.Header.ToString());
            management_label.Content = selectedNode.Header.ToString();


            string SQL_table = null;

            switch (selectedNode.Header.ToString())
            {
                case "Audio":
                    SQL_table = "audio";
                    goto case "Images";
                case "Videos":
                    // fixme: this whole thing will need a rewrite to support audio&video&images
					SQL_table = null;
                    goto case "Images";
                case "Images":
                    if (SQL_table == null)
                    {
                        //SQL_table = "audio";
                        return;
                    }
                    
                    
                    using (SQLiteConnection cnn = new SQLiteConnection(sqldatasource))
                    using (SQLiteCommand cmd = cnn.CreateCommand())
                    {
                        // Open the connection. If the database doesn't exist,
                        // it will be created automatically
                        cnn.Open();

                        // TODO this will have to be modified to be used for audi/video/images/etc
                        cmd.CommandText = "SELECT COUNT(*) FROM " + SQL_table;
                        uint node_audio_songs_new = Convert.ToUInt32(cmd.ExecuteScalar());

                        // if we don't see any new records, skip refreshing the grid
                        if (management_listview.Items.Count == node_audio_songs_new)
                        {
                            // FIX ME if we remove and then add an item, the count will be the same but we do need to update the list!
                            // this is fixed. I always update the list now that I'm using a new, FASTER itemsource method.
                           // return;
                        }

                        
                        // we can't test it against update records, we need to test it against which
                        // medialibrary item is selected!
                        //if (node_audio_songs_new > node_audio_songs)

                        cmd.CommandText = "SELECT ID,title,artists,album,length FROM " + SQL_table;

                        //cmd.CommandText = "SELECT * FROM now_playing, audio WHERE (now_playing.media_type=1) AND (now_playing.media_ID=audio.ID)";
                        //cmd.CommandText = "SELECT * FROM media_folders WHERE (removed = 0) AND (type = audio)";

                        string[] str = null;
                        

                        //management_listview.Items.Clear();
                        management_listview.ItemsSource = null;
                       // management_listview.Items.DeferRefresh();




                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.VisibleFieldCount == 0)
                            {
                                break;
                            }

                            System.Collections.ArrayList row = new System.Collections.ArrayList();
                            ListViewItem ii = null;
                            /*

                            using (listview_mediafolders.Items.DeferRefresh())
                            {
                                foreach (Object item in new System.Collections.ArrayList(listview_mediafolders.SelectedItems))
                                {
                                    (listview_mediafolders.ItemsSource as System.Collections.IList).Remove(item);
                                }
                            }
                             */
                            //int count = 1;
                            while (reader.Read())
                            {
                                str = new string[5];
                                //str[0] = "";
                                //fix me -- the id is the autoincrement from the DB, not the actual row #
                                //fi xed added a count to the first column instead of using the ID
                                //str[0] = reader["ID"].ToString();
                                //str[0] = count++.ToString();
                                str[0] = reader["title"].ToString();
                                str[1] = "";
                                str[2] = reader["artists"].ToString();
                                str[3] = reader["album"].ToString();

                                /*
                                TimeSpan ts = new TimeSpan(12, 6, 7);
                                string x = string.Format ("{0:00}:{1:00}:{2:00}", (int)ts.TotalHours, ts.Minutes, ts.Seconds);
                                str[4] = reader["length"].ToString();
                                 */

                                uint ms_length = Convert.ToUInt32(reader["length"]);
                                uint minute = (ms_length / 1000 / 60);
                                uint second = (ms_length / 1000 % 60);

                                str[4] = minute.ToString() +":" + second.ToString("00");
                                //str[4] = String.Format("{0:mm:ss}", ms_length);

                                ii = new ListViewItem();
                                ii.Content = str;
                                ii.Tag = reader["ID"].ToString();

                                //ii.VerticalContentAlignment = VerticalAlignment.Bottom;
                                //ii.HorizontalContentAlignment = HorizontalAlignment.Center;


                                row.Add(ii);

                                //management_listview.Items.Add(new ListViewItem().Content = str);
                                //management_listview.Items.a
                            }
                            using (management_listview.Items.DeferRefresh())
                            {
                                //foreach (Object item in new System.Collections.ArrayList(row.Count))
                                //for (int x = 1; x < row.Count; x++)
                                //{
                                //    (management_listview.ItemsSource as System.Collections.IList).Add((ListViewItem)row[x]);
                                //}
                                //(management_listview.ItemsSource as System.Collections.IList).Add(ii);

                                // much faster but when we're debugging it KILL THE COMPUTER
                                // because it prints so much shit to the console about horizontal/vertical contentalignment
                                management_listview.ItemsSource = row;

                            }
                            management_listview.Items.Refresh();
                           // management_listview.ItemsSource = row;
                        }
                    }

                    break;
                case "Removable Storage":

                    //MessageBox.Show("YES");

                    selectedNode.Items.Clear();

                    DriveInfo[] allDrives = DriveInfo.GetDrives();

                    if (allDrives.Length == selectedNode.Items.Count)
                    {
                        // 
                    }

                    foreach (DriveInfo d in allDrives)
                    {
                        //Console.WriteLine(d.Name + " => " + d.DriveType + " :> " + d.IsReady);
                        
                        if (d.IsReady && ( (d.DriveType == DriveType.Removable) || (d.DriveType == DriveType.CDRom) ) )
                        {
                            // This is the drive you want...
                            //MessageBox.Show(d.Name);
                            
                            // for some reason it doesn't really create a TRUE treeviewitem when I do it on one line, it only creates a string.
                           // selectedNode.Items.Add(new TreeViewItem().Header = d.Name + " (" + d.VolumeLabel + ")");
                            TreeViewItem asdf = new TreeViewItem();
                            asdf.Header = d.Name + " (" + d.VolumeLabel + ")";
                            selectedNode.Items.Add(asdf);
                        }
                    }

                    // expand this selection
                    selectedNode.IsExpanded = true;


                    break;

                case "New Playlist":
                    //MessageBox.Show(parent_node.Header.ToString());
                    
                    /*
                     * TreeViewItem new_playlist = new TreeViewItem();
                    new_playlist.Header = "Playlist " + parent_node.Items.Count;
                    parent_node.Items.Add(new_playlist);
                     */

                    // todo when they have this item clicked, change the window on the right so that it has a button to add new pl (with box for name)

                    break;

                case "Now Playing":

                    using (SQLiteConnection cnn = new SQLiteConnection(sqldatasource))
                    {
                        using (SQLiteCommand cmd = cnn.CreateCommand())
                        {
                            cnn.Open();
                            cmd.CommandText = "SELECT * FROM now_playing, audio WHERE (now_playing.media_type=1) AND (now_playing.media_ID=audio.ID)";

                            string[] str = null;
                            management_listview.ItemsSource = null;

                            using (SQLiteDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.VisibleFieldCount == 0)
                                {
                                    break;
                                }

                                System.Collections.ArrayList row = new System.Collections.ArrayList();
                                ListViewItem ii = null;

                                while (reader.Read())
                                {
                                    str = new string[5];
                                    str[0] = reader["title"].ToString();
                                    str[1] = "";
                                    str[2] = reader["artists"].ToString();
                                    str[3] = reader["album"].ToString();

                                    uint ms_length = Convert.ToUInt32(reader["length"]);
                                    uint minute = (ms_length / 1000 / 60);
                                    uint second = (ms_length / 1000 % 60);

                                    str[4] = minute.ToString() + ":" + second.ToString("00");

                                    ii = new ListViewItem();
                                    ii.Content = str;
                                    ii.Tag = reader["ID"].ToString();

									ii.MouseDoubleClick += new MouseButtonEventHandler(ii_MouseDoubleClick);
									//ii.MouseDoubleClick += now_playingPlaySelected(Convert.ToUInt32(reader["ID"]));


                                    row.Add(ii);

                                }
                                using (management_listview.Items.DeferRefresh())
                                {
                                    management_listview.ItemsSource = row;
                                }
                                management_listview.Items.Refresh();
                            }
                        }
                        cnn.Close();
                    }
                    break;
            }
        }

		void ii_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			//ListViewItem clicked_listviewitem = (ListViewItem)sender;
			// found in the tab_media_management.cs file
			//ListView parent = (ListView)clicked_listviewitem.Parent;
			//now_playingPlaySelected(Convert.ToUInt16(parent.SelectedIndex));

			// todo: re-add the wiping playlist functionality
			playNowPlayingID(management_listview.SelectedIndex);

			//now_playingPlaySelected(Convert.ToUInt16(management_listview.SelectedIndex), false);

		}

        private void media_details_vistoggle_Click(object sender, RoutedEventArgs e)
        {
           // enable_vis = !enable_vis;
            ani.IsEnabled = !ani.IsEnabled;
        }

        private void media_mute_Click(object sender, RoutedEventArgs e)
        {
			if (audioSystem.channel != null)
			{
				bool mute = false;
				audioSystem.channel.getMute(ref mute);
				audioSystem.channel.setMute(!mute);
			}
			else
			{
				if (media_mute.IsChecked == true)
				{
					media_volume_image.Source = new BitmapImage(new Uri(@"Resources/images/silk/sound_mute.png", UriKind.Relative));
				}
				else
				{
					// find out if we're in the "low" range or not and set appropriate icon
					if (media_volume.Value <= 25d)
					{
						media_volume_image.Source = new BitmapImage(new Uri(@"Resources/images/silk/sound_low.png", UriKind.Relative));
					}
					else
					{
						media_volume_image.Source = new BitmapImage(new Uri(@"Resources/images/silk/sound.png", UriKind.Relative));
					}
				}
			}
        }

        private void media_volume_down_Click(object sender, RoutedEventArgs e)
        {
			if (audioSystem.channel != null)
			{
				float current_volume = 0f;
				audioSystem.channel.getVolume(ref current_volume);
				current_volume -= 0.05f;
				audioSystem.channel.setVolume(current_volume);
				//Console.WriteLine(current_volume);
				media_volume.Value = Convert.ToDouble(current_volume * 100f);
			}
			else
			{
				// do this if the audio is not yet loaded
				media_volume.Value -= 5d;
			}
        }

        private void media_volume_up_Click(object sender, RoutedEventArgs e)
        {
            if (audioSystem.channel != null)
            {
                float current_volume = 0f;
                audioSystem.channel.getVolume(ref current_volume);
                current_volume += 0.05f;
                audioSystem.channel.setVolume(current_volume);
                media_volume.Value = Convert.ToDouble(current_volume * 100f);
            }
			else
			{
				// do this if the audio is not yet loaded
				media_volume.Value += 5d;
			}
        }

		private void status_time_MouseDown(object sender, MouseButtonEventArgs e)
		{
			// go to our date/time tabItem
			if (tabControl1.SelectedIndex != 8)
			{
				tabControl1.SelectedIndex = 8;
			}
		}

		private void Button_Click_2(object sender, RoutedEventArgs e)
		{
			LoadNowPlayingListV2();
		}

		private void expander_playlist_options_Expanded(object sender, RoutedEventArgs e)
		{
			/*
			DoubleAnimation dblAnimation = new DoubleAnimation();
			//set the width that we want the expander control to
			//have when animation is complete
			dblAnimation.To = 35;
			//set the duration of this animation - generally 1/4 or less looks best
			dblAnimation.Duration = new Duration(TimeSpan.FromSeconds(.25));
			//begin the animation on the width property of the expMenu control
			expander_playlist_options.BeginAnimation(Expander.HeightProperty, dblAnimation);
			 */
		}

		private void TabItem_SourceUpdated(object sender, DataTransferEventArgs e)
		{

		}

		private void setRating(object sender, RoutedEventArgs e)
		{
			Debug.WriteLine(sender);

			System.Windows.Controls.Primitives.ToggleButton the_star;
			System.Windows.Controls.Primitives.ToggleButton star_clicked = (System.Windows.Controls.Primitives.ToggleButton)sender;
			bool? first_star_old_state = false;
			int rating = 0;
			StackPanel parent = (StackPanel)star_clicked.Parent;

			Debug.WriteLine(parent.Children);


			for (int i = 0; i < parent.Children.Count; i++) {
				if (star_clicked == parent.Children[i]) {
					Debug.WriteLine("THEY MATCH: " + i.ToString());
				}
			}

			if (star_clicked == parent.Children[0]) {
				first_star_old_state = star_clicked.IsChecked;
			}


			// make them all blank
			for (int x = 0; x < parent.Children.Count; x++) {
				the_star = (System.Windows.Controls.Primitives.ToggleButton)parent.Children[x];
				the_star.IsChecked = false;
			}

			for (int i = 0; i < parent.Children.Count; i++) {
				// do a check for the very first one.
				//	if it's currently checked, but they click to turn it off, just hide it. (aka skip this portion)
				if (star_clicked == parent.Children[0]) {
					star_clicked.IsChecked = first_star_old_state;
					if (first_star_old_state == true) {
						rating = 1;
					}
					break;
				}
				else {
					if (star_clicked == parent.Children[i]) {
						//Debug.WriteLine("THEY MATCH: " + i.ToString());

						// set the range of stars
						for (rating = 0; rating <= i; rating++) {
							the_star = (System.Windows.Controls.Primitives.ToggleButton)parent.Children[rating];
							the_star.IsChecked = true;
						}
						break;
					}
				}
			}

			// apply rating to the currently playing item
			applyRatingToNowPlaying(rating);
			
			// then update the audio details tab's rating bar
			updateRatingDisplays(_nowPlayingList[media_nowplaying_id].SongRating, mediainfo_stars);
		}

		private void applyRatingToNowPlaying(int rating)
		{
			if (audioSystem.channel != null) {
				Debug.WriteLine(rating);

				using (SQLiteConnection cnn = new SQLiteConnection(this.sqldatasource)) {
					cnn.Open();
					using (SQLiteCommand cmd = cnn.CreateCommand()) {
						// todo: what about video?  will have to customize the table name and how I get the ID
						cmd.CommandText = "UPDATE audio SET rating = @new_rating WHERE (ID = @ID)";
						cmd.Parameters.AddWithValue("@ID", _nowPlayingList[media_nowplaying_id].ID);
						cmd.Parameters.AddWithValue("@new_rating", rating);
						cmd.ExecuteNonQuery();
					}
					cnn.Close();
				}

				// now update the actual rating in the nowPlaying list so we don't have to poll the DB again
				_nowPlayingList[media_nowplaying_id].SongRating = rating;
			}
		}

		/// <summary>
		/// Set the stars to the correct rating value
		/// maybe have a parameter for the stars container and then I can just call this function multiple times as needed for seperate areas
		/// </summary>
		/// <param name="rating"></param>
		private void updateRatingDisplays(int rating, StackPanel parent_container)
		{
			System.Windows.Controls.Primitives.ToggleButton the_star;

			// make them all blank
			for (int x = 0; x < parent_container.Children.Count; x++) {
				the_star = (System.Windows.Controls.Primitives.ToggleButton)parent_container.Children[x];
				the_star.IsChecked = false;
			}

			if (rating > 0) {
				for (int x = 0; x < rating; x++) {
					the_star = (System.Windows.Controls.Primitives.ToggleButton)parent_container.Children[x];
					the_star.IsChecked = true;
				}
			}

		}

		private void button_media_repeat_Click(object sender, RoutedEventArgs e)
		{
			this.media_repeat = !this.media_repeat;
		}

		private void button_media_shuffle_Click(object sender, RoutedEventArgs e)
		{
			this.media_shuffle = !this.media_shuffle;
		}

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			//TESTBOX
		//	tabControl1.SelectedIndex = Convert.ToInt32(TESTBOX.Text);
		}

		//private void showWindow lib "user32" alias "ShowIndow"  (ByVal hwnd As Integer, ByVal nCmdShow As Integer) As Integer


		private void show_taskbar_Click(object sender, RoutedEventArgs e)
		{
			int TaskBarHwnd;
			TaskBarHwnd = FindWindow("Shell_traywnd", "");

			//if (show_taskbar.IsChecked == false) {
			//	SetWindowPos(TaskBarHwnd, 0, 0, 0, 0, 0, SWP_HIDEWINDOW);
			//}
			//else {
				SetWindowPos(TaskBarHwnd, 0, 0, 0, 0, 0, SWP_SHOWWINDOW);
			//}


		}

		private void obd_log_clear_Click(object sender, RoutedEventArgs e)
		{
			obd_log.Text = "";
		}

		private void button_compact_db_Click(object sender, RoutedEventArgs e)
		{
			// compact the database
			compactSQLDatabase();
		}

		private void compactSQLDatabase()
		{
			// find out how big the db file is before
			// do the compact
			// find db size after
			// let the user know how much has been saved

			// todo: ideas to add for compacting.
			//	keep track of the size of the file. maybe on startup?
			//	prompt user to compact the db if it is large
			//	pause all audio/video actions while compacting
			//	back up DB before compacting
			//	after X number of days prompt the user to compact the db, probably keep track of when it was last compacted
			//	disable compacting right after another (not really needed, just to let the user know)

			Double filesize_orig = 0;
			Double filesize_new = 0;
			Double savings = 0;


			FileInfo asdf = new FileInfo(System.IO.Path.Combine(this.ivcs_install_dir, DBNAME));


			filesize_orig = asdf.Length;



			using (SQLiteConnection cnn = new SQLiteConnection(this.sqldatasource)) {
				cnn.Open();
				using (SQLiteCommand cmd = cnn.CreateCommand()) {
					cmd.CommandText = "VACUUM";
					cmd.ExecuteNonQuery();
				}
				cnn.Close();
			}

			asdf.Refresh();


			filesize_new = asdf.Length;
			//filesize_new = 974848;

			savings = Convert.ToDouble(filesize_new / filesize_orig);
			//Console.WriteLine(savings);
			//Console.WriteLine(savings * 100);

			savings = Math.Round(100d - (filesize_new/filesize_orig)*100d, 2);

			MessageBox.Show("Compacted database successfully \n\nOld Size: " + filesize_orig + " Bytes\n"	+
							"New Size: " + filesize_new + " Bytes\n" +
							"Savings: " + savings.ToString()  + "%");

		}

		
		

		


		

		















	}
}
