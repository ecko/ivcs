/*
 *		Audio System
 *	This initializes the audio system mainly for use with the FMOD library.
 *	It also handles loading media details and processes media files for cover art.
 *	Future plans include cleaning this section up to use classes/objects properly, and to combine video and audio functions.
 * 
 */


using System;
//using System.Collections.Generic;
using System.Text;

using TagLib.Id3v2;
using System.IO;
using System.Drawing;

namespace wpfBuild
{
    public class audioSystem
    {
        /**
         * Here we will be rewriting all of the media/audio code
         * it will contain all functions for controlling media
         * example: volume control, speaker settings, prev/next song, play/pause
         * all data for the song currently playing (art, title, albumn, artist, etc)
         * this way we can have a class independent of any form.
         */

        // FMOD Setup
        public static FMOD.System system = null;
        public static FMOD.Sound sound = null;
        public static FMOD.Channel channel = null;

        //public string speakermode_set = "";
        //public string speakermode_set_after = "";

        //private bool enable_songtime_update = true;
        //public uint ms_length = 0;

        //public UInt32 now_playing_id = 0;
        //public UInt32 now_playing_maxid = 0;

        //public bool isPlaying = false;
        private static bool isPaused = false;
        //private static bool isPlaying = false;

		//private int number_of_channels = 0;

        // Media Information:
        /*
        public static string album;
        public static string artists;
        public static string band;
        public static string comment;
        public static string composers;
        public static string date;
        public static string genres;
        //public static string publisher;
        public static string title;
        public static string tracknumber;
        public static string bitrate;
        public static string codec;
        public static string encoding;
        public static string channels;
        public static string samplerate;
        public static string steromode;
        public static string codeprofile;
        public static string tagtype;

        public static string filepath;
        public static string albumart;
        */

        public static void Initialize()
        {
			Logging.WriteToLog("AudioSystem: Initialization Started");
			FMOD.RESULT result;
            FMOD.CAPS caps = FMOD.CAPS.NONE;
            FMOD.SPEAKERMODE speakermode = FMOD.SPEAKERMODE.MONO;
            FMOD.GUID guid = new FMOD.GUID();

            int minfrequency = 0, maxfrequency = 0;

            //StringBuilder name = new StringBuilder((int)32, (int)32000);
			// when hardware accel is turned off, this string becomes longer than 32 characters, causing the program to crash. 
			StringBuilder name = new StringBuilder(256);


			try {
				result = FMOD.Factory.System_Create(ref system);
				audioSystem.ERRCHECK(result);
			}
			catch (Exception ex) {
				Logging.WriteToLog("Failed to create audio system!!  : " + ex.Message);
			}

            result = system.getDriverCaps(0, ref caps, ref minfrequency, ref maxfrequency, ref speakermode);
            audioSystem.ERRCHECK(result);

            result = system.setSpeakerMode(speakermode);                             /* Set the user selected speaker mode. */
            audioSystem.ERRCHECK(result);
			Logging.WriteToLog("Speaker mode: " + speakermode.ToString());

            if ((caps & FMOD.CAPS.HARDWARE_EMULATED) == FMOD.CAPS.HARDWARE_EMULATED) /* The user has the 'Acceleration' slider set to off!  This is really bad for latency!. */
            {                                                                        /* You might want to warn the user about this. */
                result = system.setDSPBufferSize(1024, 10);	                     /* At 48khz, the latency between issuing an fmod command and hearing it will now be about 213ms. */
                audioSystem.ERRCHECK(result);

				Logging.WriteToLog("Hardware acceleration is turned off! NOT GOOD!");

            }

			try {
				result = system.getDriverInfo(0, name, 256, ref guid);
				audioSystem.ERRCHECK(result);
				Logging.WriteToLog("Audio device: " + name.ToString());
			}
			catch (Exception ex) {
				Logging.WriteToLog("Failed to get Driver Information " + ex.Message);
			}


			/* Sigmatel sound devices crackle for some reason if the format is pcm 16bit.  pcm floating point output seems to solve it. */
            if (name.ToString().IndexOf("SigmaTel") != -1)
            {
				Logging.WriteToLog("SigmaTel device found.  Adjusting output accordingly.");

                result = system.setSoftwareFormat(48000, FMOD.SOUND_FORMAT.PCMFLOAT, 0, 0, FMOD.DSP_RESAMPLER.LINEAR);
                audioSystem.ERRCHECK(result);
            }

            result = system.init(32, FMOD.INITFLAG.NORMAL, (IntPtr)null);            /* Replace with whatever channel count and flags you use! */


            if (result == FMOD.RESULT.ERR_OUTPUT_CREATEBUFFER)
            {
                result = system.setSpeakerMode(FMOD.SPEAKERMODE.STEREO);             /* Ok, the speaker mode selected isn't supported by this soundcard.  Switch it back to stereo... */
                audioSystem.ERRCHECK(result);

				Logging.WriteToLog("Speaker mode selected isn't supported by this soundcard.  Switching back to stereo");

                result = system.init(32, FMOD.INITFLAG.NORMAL, (IntPtr)null);        /* Replace with whatever channel count and flags you use! */
                audioSystem.ERRCHECK(result);
            }

			Logging.WriteToLog("AudioSystem: Initialization complete");
        }

        //public static void prepare_stream(string fullpath_to_media)
        public static void prepare_stream()
        {
            FMOD.RESULT result;

            if (sound != null)
            {
                if (channel != null)
                {
                    channel.stop();
                    channel = null;
                }
                sound.release();
                sound = null;
            }
            //result = system.createStream(media_info.filepath, FMOD.MODE.SOFTWARE | FMOD.MODE._2D, ref sound);
            // 2D only gives us sound with 2 speakers!

            result = system.createStream(media_info.filepath, FMOD.MODE.SOFTWARE | FMOD.MODE._3D, ref sound);
            audioSystem.ERRCHECK(result);
            // }

            //result = system.playSound(FMOD.CHANNELINDEX.FREE, sound, false, ref channel);

			//  set it to paused at the start so that we can update info, and set the proper volume (done in window1.xaml.cx) so that you don't get a 100% volume spike while it tries to correct itself
			result = system.playSound(FMOD.CHANNELINDEX.FREE, sound, true, ref channel);
            audioSystem.ERRCHECK(result);

            updateMediaInfo();
      //      initSubforms();
        }

        /**
         * This is used when we first start a stream/song
         */
        private static void initSubforms()
        {
            throw new NotImplementedException();
            /*
            if (frontend1.now_playing != null)
            {
                // some code here
            }
            if (frontend1.nowPlayingControl != null)
            {
                // some more
            }
            */

        }

        /*
         * This is used to constantly update some of the fields on subforms
         * For example: song times
         * Probably will be called every timer tick
         */
        private static void updateSubforms()
        {
            throw new NotImplementedException();

            /*
            if (frontend1.now_playing != null)
            {
                // some code here
            }
            if (frontend1.nowPlayingControl != null)
            {
                // some more
            }
            */

        }

        public struct media_info
        {
            // Media Information:
            public static string album;
            public static string artists;
            public static string band;
            public static string comment;
            public static string composers;
            public static uint date;
            public static string genres;
            //public static string publisher;
            public static string title;
            public static uint tracknumber;
            public static int bitrate;
            public static string codec;
            public static string encoding;
            public static int channels;
            public static int samplerate;
            public static string steromode;
            public static string codeprofile;
            public static string tagtype;

			public static int rating;

            public static uint length;
            public static TimeSpan duration;

            public static string filepath;
            //public static string filepath;
            //public static string albumart;
			public static System.Windows.Media.Imaging.BitmapImage albumart;

			public static System.Windows.Media.Imaging.BitmapImage albumart_image;
        }

        public struct media_info_struct
        {
            // Media Information:
            public string album;
            public string artists;
            public string band;
            public string comment;
            public string composers;
            public uint date;
            public string genres;
            //public string publisher;
            public string title;
            public uint tracknumber;
            public int bitrate;
            public string codec;
            public string encoding;
            public int channels;
            public int samplerate;
            public string steromode;
            public string codeprofile;
            public string tagtype;

            public double length;
            public TimeSpan duration;

            public string filepath;
            //public string filepath;
            //public string albumart;
			public System.Windows.Media.Imaging.BitmapImage albumart;

			public System.Windows.Media.Imaging.BitmapImage albumart_image;

        }

        public static void updateMediaInfo()
        {
            TagLib.File media = TagLib.File.Create(media_info.filepath);

            media_info.album = media.Tag.Album;
            media_info.artists = media.Tag.JoinedPerformers;
        //    media_info.band = "";
        //    media_info.comment = media.Tag.Comment;
        //    media_info.composers = media.Tag.JoinedComposers;
            media_info.date = media.Tag.Year;
            media_info.genres = media.Tag.JoinedGenres;
            //publisher = media.Tag.pu;
            media_info.title = media.Tag.Title;
            media_info.tracknumber = media.Tag.Track;

			// use the filename (without the extension) if there is no data in the tags
			if (media_info.title == null) {
				FileInfo real_filepath = new FileInfo(audioSystem.media_info.filepath);
				media_info.title = (real_filepath.Name.Split(new string[] { real_filepath.Extension }, StringSplitOptions.None))[0];
			}

            //FMOD.SOUND_TYPE media_type = FMOD.SOUND_TYPE.UNKNOWN;
            //FMOD.SOUND_FORMAT media_format = FMOD.SOUND_FORMAT.NONE;
            //int media_chans = 0;
            //int media_bits = 0;

            //sound.getFormat(ref media_type, ref media_format, ref media_chans, ref media_bits);
            //media_info.codec = "";
            //media_info.codec = media.MimeType;
			// Get the file type
			// probably fmod has a function built in to do this.
            media_info.codec = media.MimeType.Split('/')[1];
            //media_info.bitrate = media.Tag.BeatsPerMinute.ToString();
            //codec;
            //encoding;
            //channels;
            //samplerate;
            //steromode;
            //codeprofile;
            //tagtype;

			// FIXME: I'm going to change this to use the taglib total MS, which might be slightly different than the FMOD version...
			//		hopefully this clears up some issues with the songs not going forward at the end
			// this actually shouldn't have any effect because I'm using FMOD to get the TOTAL LENGTH, and also the current position...
			media_info.length = audioSystem.getLengthMS();
			//media_info.length = Convert.ToUInt32(media.Properties.Duration.TotalMilliseconds);
			// this doesn't seem to be used: (as of jan 3 2009)
            //media_info.duration = media.Properties.Duration;


            media_info.channels = media.Properties.AudioChannels;
            //media_info.samplerate = media.Properties.AudioSampleRate;
            media_info.bitrate = media.Properties.AudioBitrate;
            //media_info.bitrate = audioSystem.getKBPS();


//			if (media.Tag.Pictures.GetLength(0) > 0) {
				//Console.Write("ITS NULL");
//				media_info.albumart_image = byteArrayToImage(media.Tag.Pictures[0].Data.Data);
//			}



			media_info.albumart = getAlbumArtImage(media_info.filepath);

//            string[] split_array = media_info.filepath.Split('\\');
 //           string str = media_info.filepath.Replace(split_array[split_array.Length - 1], "");
			//Console.WriteLine(media);
			/*
			Console.WriteLine(str);

            for (int x = 0; x < (split_array.Length - 1); x++)
            {
                str += split_array[x] + '\\';
				//str += split_array[x] + 
				//Console.WriteLine(System.IO.Path.Combine(str,split_array[x]));
            }
            str += "folder.jpg";
			 */
//			str = System.IO.Path.Combine(str, "folder.jpg");

 //           if (File.Exists(str) == true)
 //           {
 //               media_info.albumart = @str;
 //           }
 //           else
 //           {
 //               media_info.albumart = null;
 //           }
        }

		public static System.Windows.Media.Imaging.BitmapImage getAlbumArtImage(string filepath)
		{
			TagLib.File media = TagLib.File.Create(filepath);
			System.Windows.Media.Imaging.BitmapImage album_art_bitmap = null;

			try {
				// check if the media has a cover embeded in it
				if (media.Tag.Pictures.GetLength(0) > 0) {
					//Console.Write("ITS NULL");
					//media_info.albumart_image = byteArrayToImage(media.Tag.Pictures[0].Data.Data);

					// this seems to have corrected the problem with some files having incorrect (corrupt?) cover art data
					if (Convert.ToByte(media.Tag.Pictures[0].Data.Data.GetValue(0)) != 0) {
						album_art_bitmap = byteArrayToImage(media.Tag.Pictures[0].Data.Data);
					}
					else {
						//Console.WriteLine("NULL");
						album_art_bitmap = getFolderJPG(filepath);
					}
				}
				else {
					album_art_bitmap = getFolderJPG(filepath);
				}
			}
			catch (Exception ex) {
				Logging.WriteToLog("Album art error (corrupted?)  Defaulting back to folder image badfile: [ " + filepath +" ] \r\n" + ex.TargetSite + " => " + ex.Message);
				album_art_bitmap = getFolderJPG(filepath);
			}

			return album_art_bitmap;
		}

		public static System.Windows.Media.Imaging.BitmapImage getFolderJPG(string filepath)
		{
			// todo: cover is not embedded into the file, so we'll have to do alternative checks.
			//	for now we're going to just check the parent folder for a folder.jpg image... should really use that AlbumArt_{2BACE875-429D-43EB-90CF-155C56EBA2A2}_Large.jpg file
			// use the moby southside song for testing
			//		some people download everything from amazon. I could try doing that as well, and then writing it into the mp3 file directly (or saving it into my db, but that'd only be my second option)


			string[] split_array = filepath.Split('\\');
			string str = null;

			// sometimes we're not passing an mp3, so the last item (dir\\dir\\ : last forward slash) path will be empty, and we can't empty the empty element again...
			//  so, if the last item is empty just ignore it and use that as the root to look for the folder.jpg


			// if the path is a directory, look for an image inside it.
			//	otherwise, it's probably a file that was passed
			if (System.IO.Directory.Exists(filepath) == true) {
				str = filepath;
			}
			else {
				str = filepath.Replace(split_array[split_array.Length - 1], "");
			}

			//if (split_array[split_array.Length - 1].Length > 0) {
			//	
			//}
			//else {
				
			//}

			// doesn't work exactly as I would hope, for some reason it trims off certain directories from the end.
			//	it trims it if there's no \ at the end...
			//str = System.IO.Path.GetDirectoryName(filepath);

			
			//string str = filepath.Replace(split_array[split_array.Length - 1], "");

			str = System.IO.Path.Combine(str, "folder.jpg");
			System.Windows.Media.Imaging.BitmapImage album_art_bitmap = null;

			try {
				if (File.Exists(str) == true) {
					//media_info.albumart = @str;
					album_art_bitmap = new System.Windows.Media.Imaging.BitmapImage(new Uri(@str, UriKind.Absolute));
				}
				else {
					//str = @"C:\ivcs\include\images\Compact_Disc.jpg";
					//BitmapImage folder_image_audio = new BitmapImage(new Uri());
					//album_art_bitmap = new System.Windows.Media.Imaging.BitmapImage(new Uri(@str, UriKind.Absolute));
					album_art_bitmap = new System.Windows.Media.Imaging.BitmapImage(new Uri("pack://application:,,,/Resources/images/Compact_Disc.jpg"));
				}
			}
			catch (Exception ex) {
				Logging.WriteToLog("Album art folder image failure. \r\n" + ex.TargetSite + " => " + ex.Message);
				album_art_bitmap = null;
			}

			return album_art_bitmap;
		}


		//public static Image byteArrayToImage(byte[] byteArrayIn)
		public static System.Windows.Media.Imaging.BitmapImage byteArrayToImage(byte[] byteArrayIn)
		{
			MemoryStream ms = new MemoryStream(byteArrayIn);
			//Image returnImage = Image.FromStream(ms);

			System.Windows.Media.Imaging.BitmapImage bi = new System.Windows.Media.Imaging.BitmapImage();
			bi.BeginInit();
			bi.StreamSource = ms;
			bi.EndInit();


			//return returnImage;
			return bi;
		}

        /// <summary>
        /// This one should only be used by the DB insert syntax to reduce delay
        /// it doesn't grab so much extra junk
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>

		public static media_info_struct getMediaInfo(string filepath)
		{
			TagLib.File media = TagLib.File.Create(filepath);
			media_info_struct file_info = new media_info_struct();

			file_info.album = media.Tag.Album;
			file_info.artists = media.Tag.JoinedPerformers;
			file_info.date = media.Tag.Year;
			file_info.genres = media.Tag.JoinedGenres;
			file_info.title = media.Tag.Title;
			file_info.tracknumber = media.Tag.Track;
			file_info.length = media.Properties.Duration.TotalMilliseconds;

			return file_info;
		}


		#region OLD CODE
		public static media_info_struct getMediaInfo_OLD(string filepath)
        {
            TagLib.File media = TagLib.File.Create(filepath);

			// todo: there should be a test here to see what tag types are supported by the media item
			//		then we can just skip looking for things if there are no tags to begin with.

            media_info_struct file_info = new media_info_struct();
			//TagLib.Id3v2.Tag id3v2 = (TagLib.Id3v2.Tag) media.GetTag (TagLib.TagTypes.Id3v2);
			//TagLib.Id3v2.Tag id3v1 = (TagLib.Id3v2.Tag) media.GetTag (TagLib.TagTypes.Id3v1);

	
			// if there's no tag data, just skip this
			//if (id3v2 != null) {


				file_info.album = media.Tag.Album;
				file_info.artists = media.Tag.JoinedPerformers;
				//file_info.band = "";
				//file_info.comment = media.Tag.Comment;
				//file_info.composers = media.Tag.JoinedComposers;
				file_info.date = media.Tag.Year;
				file_info.genres = media.Tag.JoinedGenres;
				file_info.title = media.Tag.Title;
				file_info.tracknumber = media.Tag.Track;
				//file_info.codec = media.MimeType.Split('/')[1];
			//}
			//else if () {
			//	Console.WriteLine("NULL");
			//}

            file_info.length = media.Properties.Duration.TotalMilliseconds;
            //file_info.duration = media.Properties.Duration;

			/*
			 * being done in the adding sql function since we have access to the file.name instead of having to split the filepath up
			if (file_info.title == null) {
				//filepath.
				
				string[] split_array = filepath.Split('\\');
				string filename = split_array[split_array.Length - 1];
			}
			*/
            return file_info;
		}
		#endregion

		private static string getEncoding()
        {
            // get the file type
            // put the split string into an array, and then use the last element
            string[] file_name_array = media_info.filepath.Split('.');
            //nowPlaying.updateNowPlaying(file_name, file_name_array[file_name_array.Length - 1].ToUpper() + " - " + kbps + " kbps");
            return file_name_array[file_name_array.Length - 1].ToUpper();
        }

        private static uint getLengthMS()
        {
            uint ms_length = 0;
            sound.getLength(ref ms_length, FMOD.TIMEUNIT.MS);
            return ms_length;
        }

        private static uint getKBPS()
        {
            uint lenbytes = 0;
            //uint kbps = 0;
            sound.getLength(ref lenbytes, FMOD.TIMEUNIT.RAWBYTES);
            //kbps = lenbytes / (ms_length / 1000) / 1000 * 8;
            return (lenbytes / (media_info.length / 1000) / 1000 * 8);
        }

        public static bool paused
        {
            //return false;

            // the song has already been created
            // now it's likely just paused.
            get
            {
                //channel.isPlaying(ref isplaying);

                bool l_ispaused = false;

				if (channel != null) {
					channel.getPaused(ref l_ispaused);
				}
                return l_ispaused;
            }

            set
            {
                //channel.setPaused(!ispaused);
                //FMOD.RESULT.
                isPaused = value;
                channel.setPaused(isPaused);
            }
        }

        public static bool playing
        {
            //return false;

            // the song has already been created
            // now it's likely just paused.
            get
            {
                //channel.isPlaying(ref isplaying);

                bool l_ispaused = false;
				if (channel != null) {
					channel.isPlaying(ref l_ispaused);
				}
                return l_ispaused;
            }
            /*
            set
            {
                //channel.setPaused(!ispaused);
                //FMOD.RESULT.
                //isPlaying = value;
                //channel.setp
                //channel.setPaused(isPlaying);
            }
             */
        }

        public static FMOD.SPEAKERMODE getSpeakerMode() {
            FMOD.SPEAKERMODE speakermode = FMOD.SPEAKERMODE.MONO;
            system.getSpeakerMode(ref speakermode);
            return speakermode;
        }
		/*
		public int NumberofChannels
		{
		
			get { return this.number_of_channels; }
			set
			{
				int numchannels = 0;
				int dummy = 0;
				FMOD.SOUND_FORMAT dummyformat = FMOD.SOUND_FORMAT.NONE;
				FMOD.DSP_RESAMPLER dummyresampler = FMOD.DSP_RESAMPLER.LINEAR;

				system.getSoftwareFormat(ref dummy, ref dummyformat, ref numchannels, ref dummy, ref dummyresampler, ref dummy);
				this.number_of_channels = numchannels;
			}
		
		 
		}
		 */

        private static void ERRCHECK(FMOD.RESULT result)
        {
            if (result != FMOD.RESULT.OK)
            {
                //timer.Stop();
            /*    if (media != null)
                {
                    media.timer_25.Stop();
                }*/

				// TODO: add some better error catching... would be nice to write some log files

                Console.WriteLine("FMOD error! " + result + " - " + FMOD.Error.String(result));
				Logging.WriteToLog("FMOD error! " + result + " - " + FMOD.Error.String(result));
                //Environment.Exit(-1);
				System.Windows.MessageBox.Show("FMOD error! " + result + " - " + FMOD.Error.String(result));
            }
        }
    }

    
}
