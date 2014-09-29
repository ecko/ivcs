/*
 *		Video Tab
 *	This section deals with the playback of videos in the IVCS.
 *	It uses VLC to play back video files. (http://www.videolan.org/vlc/)
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
//using System.Windows.Documents;
using System.Windows.Input;
//using System.Windows.Media;
using System.Windows.Media.Imaging;
//using System.Windows.Navigation;
//using System.Windows.Shapes;

using System.Windows.Threading;
using System.Data.SQLite;
using System.Data.Common;
using System.IO;

using VideoLan;

namespace wpfBuild
{
	/// <summary>
	/// Interaction logic for Window1.xaml
	/// </summary>
	public partial class Window1 : Window
	{

		VideoLan.VideoLanClient Vlc = new VideoLanClient();
		VideoLan.VlcMediaPlayer VlcPlayer = null;
		VlcMedia vlcMedia;

		public bool enable_video_position_update = true;

		/*
		 * this caused problems when loading things.
		 *	it breaks the file path (MRL)
		 */
		VlcMedia ProcessSource(string source)
		{
			//VlcMedia tempVlcMedia;

			char[] delimiterChars = { ' ', };

			string[] words = source.Split(delimiterChars, StringSplitOptions.RemoveEmptyEntries);
			if (words.Length > 1) {
				string[] options = new string[words.Length - 1];
				for (int i = 1; i < words.Length; i++) {
					options[i - 1] = words[i];
				}
				vlcMedia = Vlc.NewMedia(words[0]);
				for (int i = 0; i < options.Length; i++) {
					vlcMedia.AddOption(options[i]);
				}
			}
			else {
				vlcMedia = Vlc.NewMedia(words[0]);
			}

			return vlcMedia;
		}

		void VlcPlayer_PositionChanged(object sender, PositionChangedEventArgs e)
		{
			//Console.WriteLine("position: " + e.Position);

			if (enable_video_position_update == true) {
				this.Dispatcher.BeginInvoke(
						DispatcherPriority.Normal,
						new NextPrimeDelegate((Action)(() =>
						{
							media_position.Value = e.Position;
							media_position_rect.Width = (media_position.ActualWidth) * (media_position.Value / media_position.Maximum);
						}))
				);
			}
		}


		public delegate void NextPrimeDelegate();

		void VlcPlayer_StateChanged(object sender, StateChangedEventArgs e)
		{
			Console.WriteLine(e.NewState.ToString());

			if (e.NewState == VlcState.Ended) {


				/*
				 * this works as well!
				 */
				this.Dispatcher.BeginInvoke(
                    DispatcherPriority.Normal,
                    new NextPrimeDelegate(videoSkip));
			}
		}

		public void videoSkip()
		{
			if (medialist_video.listItems.SelectedIndex >= (medialist_video.listItems.Items.Count - 1)) {
				medialist_video.listItems.SelectedIndex = 0;
			}
			else {
				medialist_video.listItems.SelectedIndex++;
			}
		}

		private void Grid_Initialized(object sender, EventArgs e)
		{
			// REALLY only happens once.
			System.Diagnostics.Debug.WriteLine("*** Grid Initialized *** ");


			if (VlcPlayer == null) {
				VlcPlayer = Vlc.NewMediaPlayer(vlcPanel.Handle);
			}

			// REMOVES THE OLD SELECTION CHANGED ACTION!!!! -- this function must be public to access it!
			medialist_video.listItems.SelectionChanged -= medialist_video.listItems_SelectionChanged;
			medialist_video.listItems.SelectionChanged += new SelectionChangedEventHandler(listItems_SelectionChanged);

			// set up some of the VLC-specific events.
			//	these events should only be set up one time.
			VlcPlayer.StateChanged += new EventHandler<StateChangedEventArgs>(VlcPlayer_StateChanged);
			VlcPlayer.PositionChanged += new EventHandler<PositionChangedEventArgs>(VlcPlayer_PositionChanged);

		}

		private void Grid_Loaded(object sender, RoutedEventArgs e)
		{
			// called each time the parent tabitem changes and then comes back.
			// eg: called once when opening video, not every time when going to watch then back to videos sub-tabs
			//System.Diagnostics.Debug.WriteLine("*** Grid Loaded *** ");

			//TabItem tabitem = (TabItem)sender;
			Grid grid = (Grid)sender;

			// only do work if it's being displayed.  will help the initial startup speed.
			//	after that, it'll be called on every display...
			if (grid.IsVisible == false) {
				return;
			}

			string filepath = null;
			uint ms_length = 0;
			uint minute = 0;
			uint second = 0;

			System.Collections.ObjectModel.ObservableCollection<playlistData> test = new System.Collections.ObjectModel.ObservableCollection<playlistData>();

			using (SQLiteConnection cnn = new SQLiteConnection(sqldatasource))
			using (SQLiteCommand cmd = cnn.CreateCommand()) {
				cnn.Open();
				cmd.CommandText = @"SELECT ID,path,title,length FROM video";

				using (SQLiteDataReader reader = cmd.ExecuteReader()) {
					while (reader.Read()) {
						// idea: look into saving the length as an int the class, and then when "get"ing the data back, properly format it as a string
						ms_length = Convert.ToUInt32(reader[3]);
						minute = (ms_length / 1000 / 60);
						second = (ms_length / 1000 % 60);
						filepath = reader[1].ToString();

						// todo: the check should be done when we're trying to play the file?
						test.Add(new playlistData()
						{
							ID = Convert.ToInt32(reader[0]),
							// causes an EXTREME delay
							//SongCoverImage = audioSystem.getAlbumArtImage(filepath),
							SongTitle = reader[2].ToString(),
							SongLength = minute.ToString() + ":" + second.ToString("00"),
							MediaPath = filepath
						});
					}

				}
			}

			medialist_video.UpdateItemList(test);


			// set up the VLC events.
			// TODO: could have it add the proper events onLoad and then remove them when this grid gets unloaded (remove = switch back to audio events)

			// going to have to initialized the VlcPlayer var if we do any vlc work here.  because it doesn't get initialized until the first video is played!
			// these events are set up each time the Grid loads because we remove them when the grid unloads

			// remove the audio-specific events from the UI
			media_play.Click -= media_play_Click;
			media_previous.Click -= media_previous_Click;
			media_next.Click -= media_next_Click;
			media_position.ValueChanged -= media_position_ValueChanged;
			media_position.GotMouseCapture -= media_position_GotMouseCapture;
			media_position.LostMouseCapture -= media_position_LostMouseCapture;
			media_mute.Click -= media_mute_Click;
			media_volume.ValueChanged -= media_volume_ValueChanged;

			// add the video-specific events to the UI
			media_play.Click += video_play_Click;
			media_previous.Click += video_nextprev_Click;
			media_next.Click += video_nextprev_Click;
			// don't need this because VLC has an event which monitors the position of the video that I can tie into
			//media_position.ValueChanged += video_position_ValueChanged;
			media_position.GotMouseCapture += video_position_GotMouseCapture;
			media_position.LostMouseCapture += video_position_LostMouseCapture;
			media_mute.Click += video_mute_Click;
			media_volume.ValueChanged += video_volume_ValueChanged;

			media_position.LargeChange = 0.01d;
			media_volume.Maximum = 200;

			if (audioSystem.channel != null) {
				audioSystem.paused = true;
			}
		}

		private void Grid_Unloaded(object sender, RoutedEventArgs e)
		{
			// add the audio-specific events back
			media_play.Click += media_play_Click;
			media_previous.Click += media_previous_Click;
			media_next.Click += media_next_Click;
			media_position.ValueChanged += media_position_ValueChanged;
			media_position.GotMouseCapture += media_position_GotMouseCapture;
			media_position.LostMouseCapture += media_position_LostMouseCapture;
			media_mute.Click += media_mute_Click;
			media_volume.ValueChanged += media_volume_ValueChanged;

			// remove the video-specific events
			media_play.Click -= video_play_Click;
			media_previous.Click -= video_nextprev_Click;
			media_next.Click -= video_nextprev_Click;
			media_position.GotMouseCapture -= video_position_GotMouseCapture;
			media_position.LostMouseCapture -= video_position_LostMouseCapture;
			media_mute.Click -= video_mute_Click;
			media_volume.ValueChanged -= video_volume_ValueChanged;


			media_position.LargeChange = 1d;
			media_volume.Maximum = 100d;

			// stop any videos playing
			if (VlcPlayer.State == VlcState.Playing) {
				VlcPlayer.Pause();
			}
		}

		void listItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{

			//System.Diagnostics.Debug.WriteLine("WOOT");

			//medialist_video.listItems.SelectedItem

			//System.Diagnostics.Debug.WriteLine(sender);

			ListView video_list = (ListView)sender;

			if (video_list.SelectedIndex == -1) {
				return;
			}

			System.Diagnostics.Debug.WriteLine("Attempting to play: " + ((wpfBuild.playlistData)video_list.SelectedItem).MediaPath);

			// vlc handle needs to initialize.
			//	either do that here or on the tab load. better to do it manually so it doesn't re-load on tab switches.

			//Grid parent = (Grid)tabitem_video.Content;

			// TODO: make code more portable.
			// just go to the last item...
			//parent.SelectedIndex = parent.Items.Count - 1;

			//Vlc = new VideoLanClient();

			//System.Diagnostics.Debug.WriteLine(VlcPlayer.WillPlay());


			//VlcPlayer.Load(Vlc.NewMedia(((wpfBuild.playlistData)video_list.SelectedItem).MediaPath));

			vlcMedia = Vlc.NewMedia(((wpfBuild.playlistData)video_list.SelectedItem).MediaPath);
			//this.vlcMedia.AddOption(@":no-video-title-show");
			VlcPlayer.Load(vlcMedia);
			//VlcPlayer.Load(ProcessSource(((wpfBuild.playlistData)video_list.SelectedItem).MediaPath));
			//VlcPlayer.Video.Width = 300;
			//VlcPlayer.Video.Height = 200;

			//((wpfBuild.playlistData)video_list.SelectedItem).SongLength

			//media_position.Maximum = (double) 
			media_position.Maximum = 1;



			//Console.WriteLine(vlcMedia.ReadMeta(VideoLan.libvlc_meta_t.libvlc_meta_Title));
			//Console.WriteLine(vlcMedia.ReadMeta(VideoLan.libvlc_meta_t.));

			//vlcMedia.Dispose();
			VlcPlayer.Play();
			media_volume.Value = VlcPlayer.Audio.Volume;
			media_play.IsChecked = true;

			medialist_video.listItems.ScrollIntoView(medialist_video.listItems.SelectedItem);

			//VlcPlayer.Position
			updateVideo_status();
		}






		// ########################
		// here are the video events for the UI


		void video_play_Click(object sender, RoutedEventArgs e)
		{

			if (medialist_video.listItems.SelectedIndex == -1) {
				// set it up to start playing the first item
				medialist_video.listItems.SelectedIndex = 0;
			}
			else {
				// play/pause control

				if (VlcPlayer.State == VlcState.Playing) {
					VlcPlayer.Pause();
				}
				else if (VlcPlayer.State == VlcState.Paused) {
					VlcPlayer.Play();
				}
			}
			updateVideo_status();
			//updateMedia_status();
		}

		private void video_nextprev_Click(object sender, RoutedEventArgs e)
		{
			Button button_direction = (Button)sender;

			if (button_direction.Name == "media_previous") {
				//medialist_video.listItems.SelectedIndex = 
				if ((medialist_video.listItems.SelectedIndex == -1) || (medialist_video.listItems.SelectedIndex == 0)) {
					// move it to the last item if there's nothing current selected or we're on the first item
					medialist_video.listItems.SelectedIndex = medialist_video.listItems.Items.Count - 1;
				}
				else {
					medialist_video.listItems.SelectedIndex--;
				}
			}
			else {
				if ((medialist_video.listItems.SelectedIndex == -1) || (medialist_video.listItems.SelectedIndex == (medialist_video.listItems.Items.Count - 1))) {
					medialist_video.listItems.SelectedIndex = 0;
				}
				else {
					medialist_video.listItems.SelectedIndex++;
				}
			}

		}

		private void video_position_GotMouseCapture(object sender, MouseEventArgs e)
		{
			if (VlcPlayer.State == VlcState.Playing) {
				enable_video_position_update = false;
			}
			
			if (audioSystem.channel != null) {
				enable_songtime_update = false;
			}
		}

		private void video_position_LostMouseCapture(object sender, MouseEventArgs e)
		{
			if (VlcPlayer.State == VlcState.Playing) {
				// update position of the video
				VlcPlayer.Position = (float)media_position.Value;
				enable_video_position_update = true;
			}
			
		}

		/// <summary>
		/// Set all the proper values for details about the media (top bar)
		/// </summary>
		private void updateVideo_status()
		{
			//media_status_title.Content = audioSystem.media_info.title + "  (" + audioSystem.media_info.artists + ")";
			media_status_title.Text = ((wpfBuild.playlistData)medialist_video.listItems.SelectedItem).SongTitle;

			// these will get reset back to their proper states by the updateMedia_status function for the audio, so don't worry
			media_status_artists.Visibility = Visibility.Collapsed;
			media_status_album.Visibility = Visibility.Collapsed;

			// UPDATE THE RATING STARS
			//updateRatingDisplays(_nowPlayingList[media_nowplaying_id].SongRating, media_status_stars);

			String smallicon_path = "Resources/images/silk/control_play.png";

			if (VlcPlayer.State == VlcState.Paused) {
				smallicon_path = "Resources/images/silk/control_pause.png";
			}

			media_status_statusicon.Source = new BitmapImage(new Uri(@smallicon_path, UriKind.Relative));

			TagLib.File media = TagLib.File.Create(((wpfBuild.playlistData)medialist_video.listItems.SelectedItem).MediaPath);
			media_status_filetype.Text = media.MimeType.Split('/')[1];

			//media_status_filetype.Text = vlcMedia.ReadMeta(VideoLan.libvlc_meta_t.;
			//image1.ImageSource = image2.ImageSource = audioSystem.media_info.albumart;
			//image1_border.Visibility = Visibility.Visible;

				//updateMedia_playlistDetails();
		}

		private void video_mute_Click(object sender, RoutedEventArgs e)
		{
			if (media_mute.IsChecked == true) {
				media_volume_image.Source = new BitmapImage(new Uri(@"Resources/images/silk/sound_mute.png", UriKind.Relative));
			}
			else {
				// find out if we're in the "low" range or not and set appropriate icon
				if (media_volume.Value <= 25d) {
					media_volume_image.Source = new BitmapImage(new Uri(@"Resources/images/silk/sound_low.png", UriKind.Relative));
				}
				else {
					media_volume_image.Source = new BitmapImage(new Uri(@"Resources/images/silk/sound.png", UriKind.Relative));
				}
			}

			VlcPlayer.Audio.ToggleMute();
		}

		private void video_volume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			// weird init errors if I don't wait until this is set to 100... or wait until it's loaded (loaded comes a bit after initialized so it's better to use loaded)
			if (media_volume.IsLoaded == true) {
				media_volume_label.Content = media_volume.Value.ToString("0") + "%";

				if (media_volume.Value <= media_volume.Minimum) {
					media_volume_down.IsEnabled = false;
					media_volume_image.Source = new BitmapImage(new Uri(@"Resources/images/silk/sound_none.png", UriKind.Relative));
				}
				else if (media_volume.Value >= media_volume.Maximum) {
					media_volume_up.IsEnabled = false;
				}
				else {
					media_volume_down.IsEnabled = true;
					media_volume_up.IsEnabled = true;

					if (media_volume.Value <= 25d) {
						media_volume_image.Source = new BitmapImage(new Uri(@"Resources/images/silk/sound_low.png", UriKind.Relative));
					}
					else {
						// todo: is it a waste of resources to update the image *EVERY* time even if it's not low?  (from some testing I couldn't see much/any cpu increase)
						//		it seems like the slider itself causes HIGH (30%+) usage (just by sliding it)
						media_volume_image.Source = new BitmapImage(new Uri(@"Resources/images/silk/sound.png", UriKind.Relative));
					}
				}
			}

			VlcPlayer.Audio.Volume = Convert.ToInt32(media_volume.Value);
		}


		private void BuildVideoThumbnails()
		{
			// idea: if they pass a string then we just do the one file?

			// don't think this is possible without calling the vlc.exe file...
			// could possibly try using ffmpeg, but would be better to do that after a rewrite


			////this.vlcMedia.AddOption(@":no-video-title-show");
			//vlc -V image --start-time 0 --stop-time 1 --image-out-format jpg --image-out-ratio 24 --image-out-prefix snap test.mpg vlc://quit

		}

	}

}