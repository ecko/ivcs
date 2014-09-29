/*
 *		Audio Tab
 * This section deals with the Audio tab of the IVCS. At the moment it mainly deals with the visualizations (particularly the 64 band visualization).
 * Attempts were made to have the visualization update according to the beat of the music playing, but it became too complicated.
 */
using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
//using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Windows.Threading;
using System.Data.SQLite;
using System.Data.Common;
using System.IO;

using System.Windows.Media.Animation;
using System.Collections.ObjectModel;

//using System.Net;
//using System.Windows.Navigation;

using KineticScrollingPrototype;
using System.Globalization;
using System.Collections;

namespace wpfBuild
{
	public partial class Window1 : Window
	{
		protected ObservableCollection<playlistData> _nowPlayingList;
		//protected ObservableCollection<Product> _products;

		//public Queue energy_history_buffer = new Queue();


		const int spectrum_size = 1024;
		const int subband_count = 64;
		//public float energy_subband_multiplier = subband_count / spectrum_size;

		float[,] energy_history_buffer = new float[subband_count,43];
		public int energy_history_buffer_index = 0;

		private void LoadNowPlayingListV2()
		{
			/*
			_products = new ObservableCollection<Product>();

			for (int i = 0; i <= 100; i++) {
				_products.Add(new Product() { ActualPrice = 5.99M, Description = "Test " + i.ToString(), Quantity = 1 });
			}

			custJournal.UpdateItemList(_products);
			 */

			//_nowPlayingList = new ObservableCollection<NowPlaying>();

			//for (int i = 0; i <= 100; i++) {
				//_products.Add(new Product() { ActualPrice = 5.99M, Description = "Test " + i.ToString(), Quantity = 1 });
			//}
			//_nowPlayingList.Add(new NowPlaying() { SongCoverImage = @"S:\MUSIC\Silversun Pickups - Carnavas [2006]\folder.jpg", SongTitle = "TEST" });

			//custJournal.UpdateItemList(_nowPlayingList);
		}

		//private static Singleton instance;

		//public static Singleton Instance()
		//{
			/*
			get 
			{
         if (instance == null)
         {
            instance = new Singleton();
         }
         return instance;
			}
			*/
		//}

		DispatcherTimer visAniTimer = new DispatcherTimer();
		int number_of_channels = 0;

		// build the visualization bars array?
		//	35 elements, with just values

		//ArrayList new System.Collections.ArrayList(listview_mediafolders.SelectedItems)
		float[] plot_values = new float[35];
		private const float sample_size = 7f;

		//private const Brush visualizationBrush = Brushes.DodgerBlue;


		private void createVisAnimation()
		{
			// 1024 samples approx. equal to 0.05 seconds = 50ms
			//visAniTimer.Interval = TimeSpan.FromMilliseconds(50);		// 32 is pretty good, 25 = 1/40 of a second wich also works well
			//visAniTimer.Interval = TimeSpan.FromMilliseconds(45.0d);
			//visAniTimer.Interval = TimeSpan.FromMilliseconds(31.25d);
			visAniTimer.Interval = TimeSpan.FromMilliseconds(23.255d);

			//visAniTimer.Interval = TimeSpan.FromMilliseconds(25d);
			//visAniTimer.Interval = TimeSpan.FromMilliseconds(1000/60);	// trying to do something like 60fps, i think it's a bit too fast though
			visAniTimer.Tick += new EventHandler(visAniTimer_Tick);


			//drawVisGrids();

		}

		private void updateNumberofChannels()
		{
			//  fixme: this should be done on every media load? or just once.
			//		it seems to be getting the sound setup for the audio. so that should only change at startup.
			//		loading this only 1 time so that we don't keep reading it every tick.
			int numchannels = 0;
			int dummy = 0;
			FMOD.SOUND_FORMAT dummyformat = FMOD.SOUND_FORMAT.NONE;
			FMOD.DSP_RESAMPLER dummyresampler = FMOD.DSP_RESAMPLER.LINEAR;

			audioSystem.system.getSoftwareFormat(ref dummy, ref dummyformat, ref numchannels, ref dummy, ref dummyresampler, ref dummy);
			this.number_of_channels = numchannels;


			//media_visualization_2.Children.Add(new Line() DrawLine(whitePen, new Point(count2, 275), new Point(count2, height)););
		}

		void drawVisGrids()
		{
			int display_width = 350;
			int display_height = 200;

			int line_seperation = 5;
			int lines_hori = (display_height / line_seperation);
			int lines_vert = (display_width / line_seperation);

			double y_value = 0;
			double x_value = 0;

			Pen blackPen = new Pen();
			Pen gridPen = new Pen();

			blackPen.Brush = Brushes.Black;
			blackPen.Thickness = 1;
			blackPen.Freeze();

			gridPen.Brush = Brushes.WhiteSmoke;
			gridPen.Thickness = 0.5;
			gridPen.DashStyle = DashStyles.Dash;
			gridPen.DashCap = PenLineCap.Round;
			gridPen.Freeze();
			
			DrawingVisual dv = new DrawingVisual();

			using (DrawingContext ctx = dv.RenderOpen()) {
				
				

				for (int n = 0; n <= lines_hori; n++) {
					y_value = line_seperation * n;
					ctx.DrawLine(whitePen, new Point(0, y_value), new Point(display_width, y_value));
				}

				for (int n = 0; n <= lines_vert; n++) {
					x_value = line_seperation * n;
					ctx.DrawLine(whitePen, new Point(x_value, 0), new Point(x_value, display_height));
				}


				// draw an outline of our grid... this won't be needed if the gridlines are calculated properly. 
				//	however this outline can be a different colour from the rest of the lines.
				//ctx.DrawRectangle(null, blackPen, new Rect(0, 0, display_width, display_height));

	
				/*
				for (int n = 0; n < 21; n++) {
					ctx.DrawLine(gridPen, new Point(0, (1 / 20.0) * 512 * n), new Point(512, (1 / 20.0) * 512 * n));
				}
				 */

				// Vertical Lines
				/*
				for (int n = 0; n < 20; n++) {	// 21 -> 20
					ctx.DrawLine(gridPen, new Point((1 / 20.0) * 512 * n, 0), new Point((1 / 20.0) * 512 * n, 512));
				}
				*/
			}

			RenderTargetBitmap rtb = new RenderTargetBitmap(display_width, display_height, 96, 96, PixelFormats.Pbgra32);
			rtb.Render(dv);

			ImageBrush ib = new ImageBrush(rtb);
			ib.Freeze();

			media_visualization_border.Background = ib;
		}


		void visAniTimer_Tick(object sender, EventArgs e)
		{
			if ((audioSystem.channel == null) || (audioSystem.playing == false)) {
				return;
			}


			/*
			 * FIXME:  the bars are *ALWAYS* being updated because the check wasn't working.
			 *			and the constant was too high for there to be any difference.
			 *			but it looks OK so I'm not sure what to do.
			 *			
			 *			tried a lot of different things but stuff *still* doesn't look right
			 */

			int count = 0;
			/*
			int count2 = 0;

			float max = 0;
			float sample_sum = 0;
			int plot_index = 0;
			float new_sample = 0;

			float height = 0;

			float GRAPHICWINDOW_HEIGHT = 200f;
			int GRAPHICWINDOW_WIDTH = 350;
			 */

			float[] spectrum_a = new float[spectrum_size];
			float[] spectrum_b = new float[spectrum_size];
			float[] spectrum_combined = new float[spectrum_size];
			float[] energy_subband = new float[subband_count];
			float[] energy_subband_average = new float[subband_count];
			float subband_energy_sum = 0;
			float average_energy_sum = 0;
			float energy_subband_multiplier = (float)subband_count / 1024f;
			float height = 0;

			//Queue energy_history_buffer = new Queue(43);


			// getting spectrum data for ONLY channels 1 & 2
			//audioSystem.system.getSpectrum(spectrum_a, spectrum_size, 0, FMOD.DSP_FFT_WINDOW.TRIANGLE);
			//audioSystem.system.getSpectrum(spectrum_a, spectrum_size, 0, FMOD.DSP_FFT_WINDOW.TRIANGLE);
			//audioSystem.system.getSpectrum(spectrum_b, spectrum_size, 1, FMOD.DSP_FFT_WINDOW.BLACKMAN);
			//audioSystem.system.getSpectrum(spectrum_b, spectrum_size, 1, FMOD.DSP_FFT_WINDOW.BLACKMAN);

			audioSystem.system.getSpectrum(spectrum_b, spectrum_size, 1, FMOD.DSP_FFT_WINDOW.HANNING);
			audioSystem.system.getSpectrum(spectrum_b, spectrum_size, 1, FMOD.DSP_FFT_WINDOW.HANNING);

			// grab the max values of our frequencies accross the 2 channels
			for (count = 0; count < spectrum_size; count++) {
				//spectrum_combined[count] = Math.Max(spectrum_a[count], spectrum_b[count]);
				// the instant energy:
				//	sum of the squares of each channel
				//spectrum_combined[count] = ((spectrum_a[count] * spectrum_a[count]) + (spectrum_b[count] * spectrum_b[count])) * 100;
				//spectrum_combined[count] = (float)Math.Sqrt((spectrum_a[count] * spectrum_a[count]) + (spectrum_b[count] * spectrum_b[count]));
				spectrum_combined[count] = spectrum_a[count] + spectrum_b[count];
				//spectrum_combined[count] = 10f * (float)Math.Log10(spectrum_combined[count] / (1f * (float)Math.Pow(10, -12)));
			}

			// empty our old arrays
			spectrum_a = spectrum_b = null;

			//for (int i = 0; i < spectrum_size; (i+1)*32) {
			// double check this, it's easier in the pdf.
			// (i+1)*32
			// could probably do i = 1, then have it i*32 for the step
			// *** or just leave it @ 0 so that we can use it as a proper index.
			//}


			for (int i = 0; i < subband_count; i++) {
				subband_energy_sum = 0;

				int k = i * (spectrum_size / subband_count);
				// a*i + b
				//int k = 4 * i + 4;
				int limit = ((i + 1) * (spectrum_size / subband_count));

				while (k < limit) {
					subband_energy_sum += spectrum_combined[k];
					k++;
				}

				// (float)(subband_count / spectrum_size)
				//energy_subband_multiplier = energy_subband_multiplier;
				// add the value to our current subband energy array, and also add it to our subband energy history array
				energy_history_buffer[i, energy_history_buffer_index] = energy_subband[i] = energy_subband_multiplier * subband_energy_sum;

				// reset our index pointer value
				if (energy_history_buffer_index == 42) {
					energy_history_buffer_index = 0;
				}
				else {
					energy_history_buffer_index++;
				}

				k = 0;
				average_energy_sum = 0;

				while (k < 43) {
					average_energy_sum += energy_history_buffer[i, k];
					k++;
				}
				//
				energy_subband_average[i] = (1f / 43f) * average_energy_sum;

				//if (energy_subband[i] > (250f * energy_subband_average[i])) {
				if (energy_subband[i] > (1.10f * energy_subband_average[i])) {
					// we have a beat !

					// set rectangle i to the new values (dimensions)
					//lines[i].Y2 = 200 - (energy_subband[i] * 20000f);

					//height = energy_subband[i] * 100000f;
					//height = energy_subband[i] * 20000f;
					height = energy_subband[i] * 15000f;
					//height = energy_subband[i] * 1f;	// for the dB attempt

					if (height >= 250) {
						height = 249;
					}

					if (height < 0) {
						height = 0;
					}


					height = 250 - height;
					lines[i].Y2 = height;

					/*

					float height = ((energy_subband[i] * 5f) * GRAPHICWINDOW_HEIGHT) * 10000f;
					
					if (height >= GRAPHICWINDOW_HEIGHT) {
						height = GRAPHICWINDOW_HEIGHT - 1;
					}

					if (height < 0) {
						height = 0;
					}

					lines[i].Y2 = 200 - height;
					*/

				}


			}

		}






		// working VERY WELL
		void visAniTimer2_Tick(object sender, EventArgs e)
		{
			if ((audioSystem.channel == null) || (audioSystem.playing == false)) {
				return;
			}
			
			int count = 0;
			/*
			int count2 = 0;

			float max = 0;
			float sample_sum = 0;
			int plot_index = 0;
			float new_sample = 0;

			float height = 0;

			float GRAPHICWINDOW_HEIGHT = 200f;
			int GRAPHICWINDOW_WIDTH = 350;
			 */

			float[] spectrum_a = new float[spectrum_size];
			float[] spectrum_b = new float[spectrum_size];
			float[] spectrum_combined = new float[spectrum_size];
			float[] energy_subband = new float[subband_count];
			float[] energy_subband_average = new float[subband_count];
			float subband_energy_sum = 0;
			float average_energy_sum = 0;
			float energy_subband_multiplier = 32f / 1024f;

			//Queue energy_history_buffer = new Queue(43);


			// getting spectrum data for ONLY channels 1 & 2
			audioSystem.system.getSpectrum(spectrum_a, spectrum_size, 0, FMOD.DSP_FFT_WINDOW.TRIANGLE);
			audioSystem.system.getSpectrum(spectrum_a, spectrum_size, 0, FMOD.DSP_FFT_WINDOW.TRIANGLE);
			//audioSystem.system.getSpectrum(spectrum_b, spectrum_size, 1, FMOD.DSP_FFT_WINDOW.BLACKMAN);
			//audioSystem.system.getSpectrum(spectrum_b, spectrum_size, 1, FMOD.DSP_FFT_WINDOW.BLACKMAN);

			// grab the max values of our frequencies accross the 2 channels
			for (count = 0; count < spectrum_size; count++) {
				spectrum_combined[count] = Math.Max(spectrum_a[count], spectrum_b[count]);
			}

			// empty our old arrays
			spectrum_a = spectrum_b = null;

			//for (int i = 0; i < spectrum_size; (i+1)*32) {
				// double check this, it's easier in the pdf.
				// (i+1)*32
				// could probably do i = 1, then have it i*32 for the step
				// *** or just leave it @ 0 so that we can use it as a proper index.
			//}


			for (int i = 0; i < subband_count; i++) {
				subband_energy_sum = 0;

				int k = i * 32;
				int limit = ((i + 1) * 32);

				while (k < limit) {
					subband_energy_sum += spectrum_combined[k];
					k++;
				}

				// (float)(subband_count / spectrum_size)
				//energy_subband_multiplier = energy_subband_multiplier;
				// add the value to our current subband energy array, and also add it to our subband energy history array
				energy_history_buffer[i, energy_history_buffer_index] = energy_subband[i] = energy_subband_multiplier * subband_energy_sum;

				// reset our index pointer value
				if (energy_history_buffer_index == 42) {
					energy_history_buffer_index = 0;
				}
				else {
					energy_history_buffer_index++;
				}


				k = 0;
				average_energy_sum = 0;

				while (k < 43) {
					average_energy_sum += energy_history_buffer[i, k];
					k++;
				}

				energy_subband_average[i] = (1 / 43) * average_energy_sum;

				if (energy_subband[i] > (250 * energy_subband_average[i])) {
					// we have a beat !


					// set rectangle i to the new values (dimensions)

					lines[i].Y2 = 200 - (energy_subband[i] * 20000f);

				}


			}

		}

		void visAniTimer_Tick_old(object sender, EventArgs e)
		{
			if ((audioSystem.channel == null) || (audioSystem.playing == false)) {
				return;
			}


			int count = 0;
			int count2 = 0;

			float max = 0;
			float sample_sum = 0;
			int plot_index = 0;
			float new_sample = 0;

			float height = 0;

			float GRAPHICWINDOW_HEIGHT = 200f;
			int GRAPHICWINDOW_WIDTH = 350;
			// try 35 bands max.

			//audioSystem.system.getSoftwareFormat(ref dummy, ref dummyformat, ref numchannels, ref dummy, ref dummyresampler, ref dummy);

			//DrawingVisual dv = new DrawingVisual();
			//using (DrawingContext ctx = dv.RenderOpen()) {
			for (count = 0; count < this.number_of_channels; count++) {
				max = 0;

				audioSystem.system.getSpectrum(spectrum, SPECTRUMSIZE, count, FMOD.DSP_FFT_WINDOW.TRIANGLE);

				// check to see what the highest frequency is.
				//		it is then used later on to just skip processing if there's nothing for the channel
				for (count2 = 0; count2 < 512; count2++) {
					if (max < spectrum[count2]) {
						max = spectrum[count2];
					}
				}


				// frequency range is from 0 to 1 (float) according to the manual
				if (max > 0.0001f) {
					sample_sum = 0;
					plot_index = 0;
					count2 = 0;
					int x = 0;
					//for (count2 = 0; count2 < 512; count2 += 14) {

					//plot_values[

					//}


					while (count2 < 256) {
						for (x = 0; x < sample_size; x++) {
							sample_sum += spectrum[count2];
							count2++;
							if (count2 == 256) {
								break;
							}
						}
						// this way we get the real # of samples, not 14 each time
						new_sample = sample_sum / (x + 1);

						// if the new sample is larger than previous channel size, store it
						if (new_sample > plot_values[plot_index]) {
							//plot_values[plot_index] = sample_sum / sample_size;
							plot_values[plot_index] = new_sample;
							plot_index++;
						}
						if (plot_index > 34) {
							break;
						}

						sample_sum = 0;
					}


				}

				//while (count2 < 512) {
				//
				//}

				/*
				if (max > 0.0001f) {
					//	The upper band of frequencies at 44khz is pretty boring (ie 11-22khz), so we are only going to display the first 256 frequencies, or (0-11khz)						
					//for (count2 = 0; count2 < 255; count2++)
					// part of it might be rendering off screen? or is it scaling the background to fit
					//for (count2 = 0; count2 < 512; count2++)
					for (count2 = 0; count2 < 275; count2++) {
						float height;

						height = spectrum[count2] / max * GRAPHICWINDOW_HEIGHT;

						if (height >= GRAPHICWINDOW_HEIGHT) {
							height = GRAPHICWINDOW_HEIGHT - 1;
						}

						if (height < 0) {
							height = 0;
						}

						height = GRAPHICWINDOW_HEIGHT - height;

						if (height > highest[count2]) {
							// instead of adding to the background in the loop, what about putting the heights into an array,
							// then using that array outside to plot all the lines?
							// that's probably how we will have to do it if we want to work with peak dropoffs
							ctx.DrawLine(whitePen, new Point(count2, 275), new Point(count2, height));
						}
					}
				}
				*/


				#region oldCode

				/*
					 * 
					 * float max = 0;

					audioSystem.system.getSpectrum(spectrum, SPECTRUMSIZE, count, FMOD.DSP_FFT_WINDOW.TRIANGLE);

					//for (count2 = 0; count2 < 255; count2++)
					//{
					for (count2 = 0; count2 < 512; count2++) {
						if (max < spectrum[count2]) {
							max = spectrum[count2];
						}
					}
					//}







					if (max > 0.0001f) {
						//	The upper band of frequencies at 44khz is pretty boring (ie 11-22khz), so we are only going to display the first 256 frequencies, or (0-11khz) 
						//for (count2 = 0; count2 < 255; count2++)
						// part of it might be rendering off screen? or is it scaling the background to fit
						//for (count2 = 0; count2 < 512; count2++)
						for (count2 = 0; count2 < 275; count2++) {
							float height;

							height = spectrum[count2] / max * GRAPHICWINDOW_HEIGHT;

							if (height >= GRAPHICWINDOW_HEIGHT) {
								height = GRAPHICWINDOW_HEIGHT - 1;
							}

							if (height < 0) {
								height = 0;
							}

							height = GRAPHICWINDOW_HEIGHT - height;

							if (height > highest[count2]) {
								// instead of adding to the background in the loop, what about putting the heights into an array,
								// then using that array outside to plot all the lines?
								// that's probably how we will have to do it if we want to work with peak dropoffs
								ctx.DrawLine(whitePen, new Point(count2, 275), new Point(count2, height));
							}
						}
					}
					 * 
					 */

				#endregion
			}


			// fixme: i don't think averaging is a very good method, because then things never really change.
			//		looks like foobar samples certain frequencies every time and then uses those samples, instead of averaging.


			//}

			DrawingVisual dv = new DrawingVisual();
			using (DrawingContext ctx = dv.RenderOpen()) {
				plot_index = 0;
				while (plot_index < 35) {
					height = plot_values[plot_index] / max * GRAPHICWINDOW_HEIGHT;

					if (height >= GRAPHICWINDOW_HEIGHT) {
						height = GRAPHICWINDOW_HEIGHT - 1;
					}

					if (height < 0) {
						height = 0;
					}

					//height = GRAPHICWINDOW_HEIGHT - height;

					ctx.DrawLine(whitePen, new Point(0, 0), new Point(350, 200));
					//Rectangle asdf = new Rectangle();


					//ctx.DrawRectangle(Brushes.Blue, null, new Rect(plot_index, 0, 10d, height));
					//ctx.DrawRectangle(Brushes.Blue, null, new Rect((plot_index * 10), height, 10d, 200-height));
					ctx.DrawRectangle(Brushes.Blue, null, new Rect((plot_index * 10), 200-height, 10d, height));

					//ctx.draw

					plot_index++;
				}



			}

			RenderTargetBitmap rtb = new RenderTargetBitmap(350, 200, 96, 96, PixelFormats.Pbgra32);

			rtb.Render(dv);

			ImageBrush ib = new ImageBrush(rtb);
			ib.Freeze();

			media_visualization_2.Background = ib;
		}


		
	}



}

namespace KineticScrollingPrototype
{
	/// <summary>
	/// This just deals with the row highlighting. It is a quick way for the rows to alternate between colours.
	/// It is used in the various playlist files (filename.xaml)
	/// </summary>
	public sealed class BackgroundConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			ListViewItem item = (ListViewItem)value;
			ListView listView = ItemsControl.ItemsControlFromItemContainer(item) as ListView;
			// Get the index of a ListViewItem
			int index =
				listView.ItemContainerGenerator.IndexFromContainer(item);

			if (index % 2 == 0) {
				//return Brushes.Silver;
				return "#990A0A0A";
			}
			else {
				return "#550A0A0A";
				//return Brushes.Transparent;
				//return System.Windows.Media.Color.
			}
		}
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new Exception("The method or operation is not implemented.");
		}
	}







	/// <summary>
	/// This class stores all of the media details for the audio now playing list.
	/// On initial load of the application, all of the media details are pulled from the database and stored into the _NowPlayingList object.
	/// This makes it much easier, and quicker to obtain information. Benefits include not having to constantly communicate with the database.
	/// The downside is that all of this information has to be stored somewhere... RAM. It would probably be better to cache cover art somewhere on the disk one time, and then just have the application save the path in memory.
	/// </summary>
	public class NowPlaying
	{
		public NowPlaying()
		{
		}

		private int id;
		private string title;
		private string artist;
		private string album;
		private string length;
		private string rating;
		private BitmapImage cover_path;


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

		public string SongRating
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

	}



	#region some sample code
	/*
	public class Product
	{
		public Product()
		{
		}

		private string size;
		private string upc;
		private string description;
		private string mfr;
		private decimal actualPrice;
		private decimal regualarPrice;
		private double quantity;
		private decimal total;
		private string formattedTotal;

		public string Size
		{
			get { return this.size; }
			set { this.size = value; }
		}
		public string UPC
		{
			get { return this.upc; }
			set { this.upc = value; }
		}

		public string Description
		{
			get { return this.description.ToUpper(); }
			set { this.description = value; }
		}

		public string MFR
		{
			get { return this.mfr; }
			set { this.mfr = value; }
		}
		public decimal ActualPrice
		{
			get { return this.actualPrice; }
			set { this.actualPrice = value; }
		}
		public decimal RegularPrice
		{
			get { return this.regualarPrice; }
			set { this.regualarPrice = value; }
		}

		public decimal ValueSavings
		{
			get { return (this.regualarPrice - this.actualPrice); }
		}
		public string ImageUrl
		{
			get;
			set;
		}
		public double Quantity
		{
			get
			{
				return this.quantity;
			}
			set
			{
				if (value <= 0)
					this.quantity = 0;
				else
					this.quantity = value;
			}
		}

		public decimal Total
		{
			get { return ((decimal)this.quantity * this.actualPrice); }
			set { this.total = value; }
		}
		public string FormattedTotal
		{
			get { return ((decimal)this.quantity * this.actualPrice).ToString("0.00"); }
			set { this.formattedTotal = value; }
		}

		public bool IsFood
		{
			get;
			set;
		}
		public bool SizeIsQty
		{
			get;
			set;
		}


	}
	 */
	#endregion


}
