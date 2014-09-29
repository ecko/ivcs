/*
 *		Media Folder List Data
 *	Object contains all of the information about the media folders.
 */

using System;
// using System.Collections.Generic;
using System.Text;
//using System.Windows.Media.Imaging;

namespace wpfBuild.data
{
	public class mediaFolderListData
	{
		public mediaFolderListData()
		{
		}

		private int id;
		//private string title;
		//private string artist;
		//private string album;
		//private string length;
		//private string rating;
		//private uint playcount;
		private System.Windows.Media.Imaging.BitmapImage cover_path;
		private bool using_alt_template;


		//private string filepath;

		// proper stuff
		private string folderpath;
		private uint file_count;
		private uint folder_count;
		private UInt16 folder_type;
		private Boolean folder_searchrecursive;
		private System.Windows.Media.Imaging.BitmapImage img_folder_type;
		// something for the image type, possible one for the image type's IMAGE as well.




		public string FolderPath
		{
			get { return this.folderpath; }
			set { this.folderpath = value; }
		}

		public uint NumFiles
		{
			get { return this.file_count; }
			set { this.file_count = value; }
		}

		public uint NumFolders
		{
			get { return this.folder_count; }
			set { this.folder_count = value; }
		}

		public UInt16 FolderType
		{
			get { return this.folder_type; }
			set { this.folder_type = value; }
		}

		public System.Windows.Media.Imaging.BitmapImage FolderTypeImage
		{
			get { return this.img_folder_type; }
			set { this.img_folder_type = value; }
		}

		public Boolean SearchSubfolders
		{
			get { return this.folder_searchrecursive; }
			set { this.folder_searchrecursive = value; }
		}












		//public string SongCoverImage
		public System.Windows.Media.Imaging.BitmapImage SongCoverImage
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

		public bool UseTemplateB
		{
			get { return this.using_alt_template; }
			set { this.using_alt_template = value; }
		}

	}
}
