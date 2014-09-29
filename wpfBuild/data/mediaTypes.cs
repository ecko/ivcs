/*
 *		Media Folder Types
 *	Object contains details used for the media management section.
 *	Includes what types of files to search for in a category (filter options).
 *	Also includes a nice class that can be used to compare media types instead of having to remember that 0 = audio, 1 = video, 2 = images
 */

using System;

namespace wpfBuild
{
	/*
	 * this has a problem because it has to initialize...
	 * changed the items to use constants and it seems like it might work...
	 * should be a tiny bit fast than a class
	 */
	struct searchPatterns
	{
		// m4a is not supported ;(
		public const string audio = "mp3,ogg,wav,wma,flac";
		//public const string video = "wmv,avi,mpg,mpeg";
		// new straight from VLC!
		public const string video = "asf,avi,divx,dv,flv,gxf,m1v,m2v,m2ts,m4v,mkv,mov,mp2,mp4,mpeg,mpeg1,mpeg2,mpeg4,mpg,mts,mxf,ogg,ogm,ps,ts,vob,wmv";
		public const string image = "gif,jpeg,jpg,png,bmp";

		public const char seperator = ',';
	}

	public enum mediaType : byte
	{
		audio = 0,
		video = 1,
		image = 2
	}

	public class getSearchPattern
	{
		public static string getPattern(mediaType media_type)
		{
			if (media_type == mediaType.audio) {
				return searchPatterns.audio;
			}
			else if (media_type == mediaType.video) {
				return searchPatterns.video;
			}
			else {
				return searchPatterns.image;
			}
		}
	}


	/*
	 * not working well.
	 * enums don't support strings, VERY good for other things though..
	public enum searchPatterns
	{
		[System.ComponentModel.Description("mp3,ogg,wav,wma")]
		audio,


		
		Low = 1,
		Medium,
		High
	}
	*/

	/// <summary>
	/// Have the constants here for file types we support.
	///		mainly used in the file/folder scans to import supported things.
	/// </summary>
	/// 
	/*
	public class searchPatterns
	{
		public const string audio = "mp3,ogg,wav,wma";
		public const string video = "wmv,avi,mpg,mpeg";
		public const string image = "gif,jpeg,jpg,png,bmp";

		public const char seperator = ',';
	}
	*/
}
