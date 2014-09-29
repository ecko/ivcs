/*
 *		Media Management
 *	This section deals with how the user can add media to the IVCS. They are required to select folders which contain audio or video.
 *	The IVCS will then scan through these folders and import the media into its database.
 *	
 * This is an updated version of the media managment section. Previous versions were included in the Window1.xaml.cs file and were quite buggy.
 * This version corrects a large issue concerning folder rescanning re-creating media indexes, which would then break the playlists.
 * It also incorporates the new UI to make managing media folders easier on the touch screen.
 * 
 * 
 */


using System;
using System.Collections.Generic;
//using System.Linq;
//using System.Text;
using System.Windows;
using System.Windows.Controls;
//using System.Windows.Data;
//using System.Windows.Documents;
using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using System.Windows.Navigation;
//using System.Windows.Shapes;

//using System.Windows.Threading;
using System.Data.SQLite;
//using System.Data.Common;
//using System.IO;
using System.Collections.ObjectModel;
using System.IO;

namespace wpfBuild
{
	/// <summary>
	/// Interaction logic for Window1.xaml
	/// </summary>
	public partial class Window1 : Window
	{
		protected ObservableCollection<playlistData> _mediaList;
		protected ObservableCollection<playlistData> _playList;
		
		
		private void management_enqueue_Click(object sender, RoutedEventArgs e)
		{
			// build the list of items to add, then send it to the now_playing_enque function for adding.
			//management_listview.SelectedItems

			if (management_listview.SelectedItems.Count > 0) {
				/*
				uint[] management_listview_ids_array = new uint[management_listview.SelectedItems.Count];
				int i = 0;
				foreach (Object item in new System.Collections.ArrayList(management_listview.SelectedItems)) {
					ListViewItem item_object = (ListViewItem)item;
					management_listview_ids_array[i] = Convert.ToUInt32(item_object.Tag);
					i++;
				}
				*/
				now_playing_enqueue(buildManagementListSelections());

			}


		}

		private void management_enqueue_all_Click(object sender, RoutedEventArgs e)
		{
			management_listview.SelectAll();
			now_playing_enqueue(buildManagementListSelections());
		}

		/// <summary>
		/// Builds a list of all the items selected in management_listview
		/// </summary>
		/// <returns>Returns an array of the media ids</returns>
		private uint[] buildManagementListSelections()
		{
			uint[] management_listview_ids_array = new uint[management_listview.SelectedItems.Count];
			int i = 0;

			foreach (Object item in new System.Collections.ArrayList(management_listview.SelectedItems)) {
				ListViewItem item_object = (ListViewItem)item;
				management_listview_ids_array[i] = Convert.ToUInt32(item_object.Tag);
				i++;
			}

			return management_listview_ids_array;
		}

		/// <summary>
		/// Similar to the buildManagementListSelections() function, however this will return a dictionary instead of uint[]
		/// </summary>
		/// <returns>Returns a Dictionary(int,uint) which contains the listitem index(int), and the media_id(uint)</returns>
		private Dictionary<int, uint> buildManagementListSelectionsDictionary()
		{
			Dictionary<int, uint> selected_media_items = new Dictionary<int, uint>();
			int index = 0;

			//foreach (Object item in new System.Collections.ArrayList(management_listview.SelectedItems)) {

			//management_listview.ItemsSource

			foreach (ListViewItem item in management_listview.Items) {
				if (item.IsSelected == true) {
					//ListView listView = ItemsControl.ItemsControlFromItemContainer(item) as ListView;
					// Get the index of the ListViewItem
					//index = listView.ItemContainerGenerator.IndexFromContainer(item);

					selected_media_items.Add(index, Convert.ToUInt32(item.Tag));
				}
				index++;


				//Console.WriteLine(objDataRowView.Row.ToString());
			}

			/*
			foreach (ListViewItem item in new System.Collections.ArrayList(management_listview.SelectedItems)) {
				//ListViewItem item_object = (ListViewItem)item;
				//selected_media_items.Add(management_listview.


				ListViewItem item_object = (ListViewItem)item;
				ListView listView2 = ItemsControl.ItemsControlFromItemContainer(item_object) as ListView;
				ListView listView = ItemsControl.ItemsControlFromItemContainer(item) as ListView;
				// Get the index of a ListViewItem
				int index = listView.ItemContainerGenerator.IndexFromContainer(item);

				selected_media_items.Add(index, Convert.ToUInt32(item.Tag));

			}
			*/
			return selected_media_items;


		}

		// enque the selected media items to the "now playing" playlist.
		private void now_playing_enqueue(uint[] enqueue_list)
		{

			/* Read the initial time. */
			DateTime startTime = DateTime.Now;

			// Create a connection and a command
            using (SQLiteConnection cnn = new SQLiteConnection(sqldatasource))
            {
                cnn.Open();
                using (SQLiteTransaction dbTrans = cnn.BeginTransaction())
                {
                    using (SQLiteCommand cmd = cnn.CreateCommand())
                    {
						foreach (uint song_id in enqueue_list) {

							cmd.CommandText = @"INSERT INTO now_playing (position_seconds, media_type, media_ID) VALUES(@position_seconds, @media_type, @media_ID)";

                            cmd.Parameters.AddWithValue("@position_seconds", 0);
                            cmd.Parameters.AddWithValue("@media_type", 1);
							cmd.Parameters.AddWithValue("@media_ID", song_id);

                            cmd.ExecuteNonQuery();
                        }
                    }
                    dbTrans.Commit();
                }

                cnn.Close();
            }

			/* Read the end time. */
			DateTime stopTime = DateTime.Now;

			/* Compute the duration between the initial and the end time. 
				* Print out the number of elapsed hours, minutes, seconds and milliseconds. */
			Duration(stopTime, startTime);


			// refresh that new nowplaying list.
			// or just add the song to the list manually....
			refresh_nowplaying();

		}

		/// <summary>
		/// Delete the selected media items from the corresponding table
		///		if we are in the audio tab prompt to physically remove the media item?
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void management_delete_Click(object sender, RoutedEventArgs e)
		{
			// need to check which playlist we're in and do the corresponding item
			//	ex. if under audio, this will actually remove the song from the list (should probalby remove it from all other lists as well).
			//	possibly make a db trigger that will do this?

			//TreeViewItem selectedNode = (TreeViewItem)management_treeview.SelectedItem;
			// when we change the selection, we're also updating the content of a label so we can just read from the label:
			//	todo: going to have to check the parent treeitem value if it's playlists.  because we want to be able to remove items from playlists as well!
			string item_selection = management_label.Content.ToString().ToLowerInvariant();
			string sql_commandtext = null;
			Dictionary<int, uint> selected_media_items = buildManagementListSelectionsDictionary();

			if (management_listview.SelectedItems.Count <= 0) {
				return;
			}

			if (item_selection == "audio") {
				sql_commandtext = "DELETE FROM audio WHERE (ID = @media_ID)";
			}
			else if (item_selection == "now playing") {
				sql_commandtext = "DELETE FROM now_playing WHERE (ID = @media_ID)";
			}
			// option 3 will be a check against the parent if the selection is under Playlists

			// Read the initial time. //
			//DateTime startTime = DateTime.Now;

			if (sql_commandtext != null) {
				// Create a connection and a command
				using (SQLiteConnection cnn = new SQLiteConnection(sqldatasource)) {
					cnn.Open();
					using (SQLiteTransaction dbTrans = cnn.BeginTransaction()) {
						using (SQLiteCommand cmd = cnn.CreateCommand()) {
							//foreach (uint song_id in selected_media_items) {
							foreach (KeyValuePair<int, uint> entry in selected_media_items) {
								//do something with entry.Value or entry.Key
								cmd.CommandText = sql_commandtext;
								cmd.Parameters.AddWithValue("@media_ID", entry.Value);
								cmd.ExecuteNonQuery();
							}
						}
						dbTrans.Commit();
					}
					cnn.Close();
				}

				// refresh the proper lists
				if (item_selection == "audio") {
					// refresh just the audio tab.
				}
				else if (item_selection == "now playing") {
					// refresh the 2 now playing lists.

					using (management_listview.Items.DeferRefresh()) {
						// need to keep track of how many items were removed so that we can subtract it from the key's value to get the proper index value
						//	once you start removing items from the collection, it throws off your indexes I believe.
						int removed_count = 0;

						foreach (KeyValuePair<int, uint> entry in selected_media_items) {
							(management_listview.ItemsSource as System.Collections.IList).RemoveAt(entry.Key - removed_count);

							// since we're removing just remove from our list and then refresh the display
							_nowPlayingList.RemoveAt(entry.Key - removed_count);

							removed_count++;
						}
					}

					management_listview.Items.Refresh();
					custJournal.UpdateItemList(_nowPlayingList);
				}

			}
			// Read the end time. //
			//DateTime stopTime = DateTime.Now;

			/* Compute the duration between the initial and the end time. 
					* Print out the number of elapsed hours, minutes, seconds and milliseconds. */
			//Duration(stopTime, startTime);
		}

		/*
		void now_playingPlaySelected(UInt16 song_id, bool wipe_now_playing)
		{
			if (wipe_now_playing == false) {
				// just start playing the selected song...
				if (song_id == 0) {
					media_nowplaying_id = 1;
					// because of the way media_skip works, we have to pass the next song AFTER the one we want, then tell it to backtrack to play
					// our correctly selected item
					media_skip(-1);
				}
				else {
					song_id--;
					media_nowplaying_id = song_id;
					media_skip(1);
				}
			}
			// otherwise we have to wipe the now playing list and then start from 0 with this song

			

		}
		*/


		private void management_medialist_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			//ComboBoxItem combo_item = (ComboBoxItem)management_medialist.SelectedItem;
			//string combo_item_header = combo_item.Content.ToString().ToLowerInvariant();
			string combo_item_header = getComboItemHeader(sender);

			string filepath = null;
			uint ms_length = 0;
			uint minute = 0;
			uint second = 0;

			_mediaList = new System.Collections.ObjectModel.ObservableCollection<playlistData>();

			/* Read the initial time. */
			//DateTime startTime = DateTime.Now;
			//Console.WriteLine(startTime);

			if (combo_item_header == "audio") {
				using (SQLiteConnection cnn = new SQLiteConnection(sqldatasource))
				using (SQLiteCommand cmd = cnn.CreateCommand()) {
					cnn.Open();
					cmd.CommandText = "SELECT ID,path,title,artists,album,length FROM audio";

					using (SQLiteDataReader reader = cmd.ExecuteReader()) {
						while (reader.Read()) {
							// idea: look into saving the length as an int the class, and then when "get"ing the data back, properly format it as a string
							ms_length = Convert.ToUInt32(reader[5]);
							minute = (ms_length / 1000 / 60);
							second = (ms_length / 1000 % 60);
							filepath = reader[1].ToString();

							// todo: see if reader.getString(i) is faster
							// getting(i) 
							//		= 1.28125
							//		= 1.234
							//		= 1.265

							// tostring()
							//		= 1.296
							//		= 1.281
							//		= 1.265

							// getting(i).tostring()
							//		= 1.234
							//		= 1.203
							//		= 1.265



							// todo: the check should be done when we're trying to play the file?
							//if (File.Exists(filepath)) {
								_mediaList.Add(new playlistData()
								{
									ID = Convert.ToInt32(reader[0]),
									// causes an EXTREME delay
									//SongCoverImage = audioSystem.getAlbumArtImage(filepath),
									SongTitle = reader[2].ToString(),
									SongAlbum = reader[4].ToString(),
									SongArtist = reader[3].ToString(),
									SongLength = minute.ToString() + ":" + second.ToString("00"),
									MediaPath = filepath
								});
							//}
						}
						
					}
				}
			}

			// takes ~1 second for the database to be read.
			/* Read the end time. */
			//DateTime stopTime = DateTime.Now;
			//Console.WriteLine(stopTime);


			/* Compute the duration between the initial and the end time. 
				* Print out the number of elapsed hours, minutes, seconds and milliseconds. */
			//TimeSpan duration = stopTime - startTime;
			//Console.WriteLine("Datebase reading duration: ");
			//Console.WriteLine("seconds:" + duration.TotalSeconds);
			//Console.WriteLine("milliseconds:" + duration.TotalMilliseconds);


			//startTime = DateTime.Now;

			management_media_customview.UpdateItemList(_mediaList);


			//stopTime = DateTime.Now;
			//duration = stopTime - startTime;
			//Console.WriteLine("Rendering control duration: ");
			//Console.WriteLine("seconds:" + duration.TotalSeconds);
			//Console.WriteLine("milliseconds:" + duration.TotalMilliseconds);


		}

		private void management_playlists_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			// grab the objects to find out what was selected
			//ComboBox combobox = (ComboBox)sender;
			//ComboBoxItem combo_item = (ComboBoxItem)combobox.SelectedItem;
			//string combo_item_header = combo_item.Content.ToString().ToLowerInvariant();
			string combo_item_header = getComboItemHeader(sender);

			if (combo_item_header == "now playing") {

				// update our management playlist custom control with the now playing items
				_playList = _nowPlayingList;
			}

			management_playlist_customview.UpdateItemList(_playList);
		}

		private void createNewPlaylist(object sender, MouseButtonEventArgs e)
		{
			TreeViewItem current_node = (TreeViewItem)management_treeview.Items[2];
			//TreeViewItem parent_node = (TreeViewItem)current_node.Parent;

			TreeViewItem new_playlist = new TreeViewItem();
			new_playlist.Header = "Playlist " + current_node.Items.Count;
			current_node.Items.Add(new_playlist);

			new_playlist.IsSelected = true;

		}

		private string getComboItemHeader(object sender)
		{
			// grab the objects to find out what was selected
			ComboBox combobox = (ComboBox)sender;
			ComboBoxItem combo_item = (ComboBoxItem)combobox.SelectedItem;
			return combo_item.Content.ToString().ToLowerInvariant();
		}



		#region Media Folders Version 2 - TabItem

		private void mediafolder_browse_Click(object sender, RoutedEventArgs e)
		{
			Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
			dlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

			dlg.CheckFileExists = false;
			dlg.FileName = "[Select a folder]";
			dlg.Filter = "Media folder|no.files";

			// Show open file dialog box
			Nullable<bool> result = dlg.ShowDialog();

			// Process open file dialog box results
			if (result == true) {
				string dir_path = System.IO.Path.GetDirectoryName(dlg.FileName);

				if (dir_path != null && dir_path.Length > 0) {
					
					// unselect any currently selected media folder in the list
					mediafolderlist.listItems.SelectedIndex = -1;
					
					//handle the folder path selected here
					mediafolder_folderpath.Text = dir_path;

					// re-enable the add button, disable the remove button and change the folder selection to be empty? 
					//	then I can see if there's a folder image for this new folder and display it
					mediafolder_add.IsEnabled = true;
					mediafolder_remove.IsEnabled = false;
					mediafolder_preview.ImageSource = audioSystem.getFolderJPG(dir_path);

					// set some defaults
					mediafolder_search_subfolders.IsChecked = false;
					mediafolder_type_audio.IsChecked = true;
					mediafolder_type_video.IsChecked = false;
					mediafolder_type_image.IsChecked = false;

				}
			}

		}

		private void mediafolder_add_Click(object sender, RoutedEventArgs e)
		{
			// check that the path actually exists. if not just empty it and stop
			//	todo: proper error checking, and a nice error message.
			// todo: check to see if the folder already exists? (even as a subdirectory?) so we don't add duplicate files...
			if (System.IO.Directory.Exists(mediafolder_folderpath.Text) == false) {
				mediafolder_folderpath.Text = "";
				return;
			}


			// todo: add some sort of throbber image so the client knows that the files are being added.

			// scan the files.
				// check what type of media we're going to add
				//	then check what if we're adding subfolders.
				//	build 2 different scans if we're going recursive or not using the proper filters for the selected media type
			// add the info to the DB.
			// then force a refresh of the mediafolder list.
			// then select the last item in the list (and move the view) so that the user can see it has been added.

			string search_patterns;
			string mediafolder_path = mediafolder_folderpath.Text;
			bool? recursive = mediafolder_search_subfolders.IsChecked;
			int mediafolder_subfolder_count = 0;
			mediaType mediafolder_type;
			int ID_mediafolder_lastinsert = 0;

			//searchPatterns searchPatterns;

			if (mediafolder_type_audio.IsChecked == true) {
				search_patterns = searchPatterns.audio;
				mediafolder_type = mediaType.audio;
			}
			else if (mediafolder_type_video.IsChecked == true) {
				search_patterns = searchPatterns.video;
				mediafolder_type = mediaType.video;
			}
			else {
				search_patterns = searchPatterns.image;
				mediafolder_type = mediaType.image;
			}


			FileInfo[] media_items = mediaFolderScanFiles(mediafolder_path, recursive, search_patterns);

			if (recursive == true) {
				// count the number of directories.
				mediafolder_subfolder_count = mediaFolderSubDirCount(mediafolder_path);
			}

			// ****
			// i have to insert the media folder into the MF table so I can use that ID to tie all the media being added
			// ****

			// find the number of folders if applicable, or just use 0.
			// then create the folder in the DB and return the last inserted index to use in a moment.

			//path, recursive, num_folders, num_files, type

			ID_mediafolder_lastinsert = insertNewFolderIntoDB(mediafolder_path, recursive, mediafolder_subfolder_count, media_items.Length, mediafolder_type);

			// return a structure back.
			// loop through the structure.

			mediafolder_progress.Maximum = media_items.Length;
			mediafolder_progress.Value = 0;

			if (mediafolder_type == mediaType.audio) {
				insertNewAudioIntoDB(media_items, ID_mediafolder_lastinsert);
			}
			else if (mediafolder_type == mediaType.video) {
				insertNewVideoIntoDB(media_items, ID_mediafolder_lastinsert);
			}
			// do else for video and else for images

			//
			//
			//
			//
			//
			// TODO: !!!!
			//

			//insertNewMediaFilesIntoDB(media_items, ID_mediafolder_lastinsert);

			// refresh, move to the last item, and scroll into view
			refreshMediaFolderList();
			mediafolderlist.listItems.SelectedIndex = mediafolderlist.listItems.Items.Count - 1;
			// FIXME: can't scroll for some reason.
			//mediafolderlist.listItems.ScrollIntoView((ListViewItem)mediafolderlist.listItems.SelectedItem);

		}

		private void mediafolder_remove_Click(object sender, RoutedEventArgs e)
		{
			if (mediafolderlist.listItems.SelectedIndex >= 0) {

				// Configure the message box to be displayed
				string messageBoxText = "Are you sure you would like to remove this media folder? \nClicking Yes will remove all the media belonging to this folder.";
				string caption = "IVCS - Media management";
				// note, only 1 folder can be selected at a time with this new custom control.
				int mediafolder_db_id = mediaFolderList[mediafolderlist.listItems.SelectedIndex].ID;

				// Display message box
				MessageBoxResult result = MessageBox.Show(messageBoxText, caption, MessageBoxButton.YesNo, MessageBoxImage.Exclamation);

				if (result == MessageBoxResult.Yes) {
					// remove the mediaFolder
					// remove the media belonging to the folder.
					//System.Diagnostics.Debug.WriteLine(mediafolder_db_id);
					removeMediaFolderAndFiles(mediafolder_db_id);
					refreshMediaFolderList();
				}
			}
			else {
				// disable the button until they select something again.
				Button remove_button = (Button)sender;
				remove_button.IsEnabled = false;
			}
		}

		private void mediafolder_rescan_selected_Click(object sender, RoutedEventArgs e)
		{
			rescanMediaFolder(mediafolderlist.listItems.SelectedIndex);
		}

		private void mediafolder_rescan_all_Click(object sender, RoutedEventArgs e)
		{
			int limit = mediafolderlist.listItems.Items.Count;

			for (int i = 0; i < limit; i++) {
				rescanMediaFolder(i);
			}

		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="listitem_id">ID of the *item in the LIST*, not of the actual folder id!</param>
		private void rescanMediaFolder(int listitem_id)
		{
			// use the listitem id to grab the additional needed information
			int folder_db_id = mediaFolderList[listitem_id].ID;
			Boolean recursive = mediaFolderList[listitem_id].SearchSubfolders;
			mediaType type = (mediaType)mediaFolderList[listitem_id].FolderType;
			string folder_path = mediaFolderList[listitem_id].FolderPath;
			string search_patterns = getSearchPattern.getPattern(type);
			int mediafolder_subfolder_count = 0;
			int insert_count = 0;

			// check that all the files still exist.
			// remove any that don't.
			removeMissingFiles(folder_db_id, type);

			// get all the files in the directory now.
			// SELECT the file that has a path == to a file in the directory now.
			// if there is no ID returned, then that file has to be added.

			FileInfo[] media_items = mediaFolderScanFiles(folder_path, recursive, search_patterns);
			//insertNew
			FileInfo[] new_media_list = buildNewMediaList(folder_db_id, media_items, type);

			if (recursive == true) {
				// count the number of directories.
				mediafolder_subfolder_count = mediaFolderSubDirCount(folder_path);
			}

			// todo: have 1 insertNewMedia function that takes a FileInfo array for the new media.
			//	then have that function branch off into seperate sections for audio,video, images, etc
			if (type == mediaType.audio) {
				insertNewAudioIntoDB(new_media_list, folder_db_id);
			}
			else if (type == mediaType.video) {
				insertNewVideoIntoDB(new_media_list, folder_db_id);
			}



			// get count of all the files belonging to this folder
			insert_count = countMediaBelongingToFolder(folder_db_id, type);

			// refresh the file and folder counts
			updateMediaFolderCounts(folder_db_id, insert_count, mediafolder_subfolder_count);

			// finally, refresh the mediafolder listing and put the selection back to the folder being rescanned
			refreshMediaFolderList();
			mediafolderlist.listItems.SelectedIndex = listitem_id;
			//mediafolderlist.listItems.ScrollIntoView((wpfBuild.data.mediaFolderListData)mediafolderlist.listItems.Items[listitem_id]);

		}

		private int countMediaBelongingToFolder(int folder_db_id, mediaType media_folder_type)
		{
			int record_count = 0;

			/* Read the initial time. */
			DateTime startTime = DateTime.Now;

			// Create a connection and a command
			using (SQLiteConnection cnn = new SQLiteConnection(sqldatasource)) {
				cnn.Open();

				using (SQLiteCommand cmd = cnn.CreateCommand()) {
					// FIXME: 3 table support needed

					if (media_folder_type == mediaType.audio) {
						cmd.CommandText = @"SELECT COUNT(*) FROM audio WHERE (ID_mediafolder = @ID_mediafolder)";
					}
					else if (media_folder_type == mediaType.video) {
						cmd.CommandText = @"SELECT COUNT(*) FROM video WHERE (ID_mediafolder = @ID_mediafolder)";
					}
					else {
						//cmd.CommandText = @"SELECT COUNT(*) FROM audio WHERE (ID_mediafolder = @ID_mediafolder)";
					}
					
					cmd.Parameters.AddWithValue("@ID_mediafolder", folder_db_id);

					record_count = Convert.ToInt32(cmd.ExecuteScalar());
				}
				cnn.Close();
			}

			/* Read the end time. */
			DateTime stopTime = DateTime.Now;

			/* Compute the duration between the initial and the end time. 
				* Print out the number of elapsed hours, minutes, seconds and milliseconds. */
			Duration(stopTime, startTime);

			return record_count;
		}

		private void updateMediaFolderCounts(int media_folder_ID, int num_files, int num_folders)
		{
			using (SQLiteConnection cnn = new SQLiteConnection(this.sqldatasource)) {
				cnn.Open();
				using (SQLiteCommand cmd = cnn.CreateCommand()) {
					cmd.CommandText = "UPDATE media_folders SET num_folders = @num_folders, num_files = @num_files WHERE (ID = @ID)";
					cmd.Parameters.AddWithValue("@ID", media_folder_ID);
					cmd.Parameters.AddWithValue("@num_folders", num_folders);
					cmd.Parameters.AddWithValue("@num_files", num_files);
					cmd.ExecuteNonQuery();
				}
				cnn.Close();
			}

		}

		private FileInfo[] buildNewMediaList(int folder_db_id, FileInfo[] media_items, mediaType media_type)
		{
			// SELECT the file that has a path == to a file in the directory now.
			// if there is no ID returned, then that file has to be added.

			// foreach media item. do a select to see if it exists in the DB
			// if not, add it to our array and send back for addition.

			// set it to something, hopefully it'll work with all the extras at the end
			FileInfo[] new_media_list = new FileInfo[media_items.Length];
			//result = new FileInfo[tlength];
			int does_exist_id = 0;
			int new_media_count = 0;
			string sql_commandtext = "";

			mediafolder_progress.Maximum = media_items.Length;
			mediafolder_progress.Value = 0;

			if (media_type == mediaType.audio) {
				sql_commandtext = @"SELECT ID FROM audio WHERE (path = @path) AND (ID_mediafolder = @ID_mediafolder)";
			}
			else if (media_type == mediaType.video) {
				sql_commandtext = @"SELECT ID FROM video WHERE (path = @path) AND (ID_mediafolder = @ID_mediafolder)";
			}
			else {
				sql_commandtext = @"SELECT ID FROM image WHERE (path = @path) AND (ID_mediafolder = @ID_mediafolder)";
			}


			/* Read the initial time. */
			DateTime startTime = DateTime.Now;

			// Create a connection and a command
			using (SQLiteConnection cnn = new SQLiteConnection(sqldatasource)) {
				cnn.Open();

				using (SQLiteCommand cmd = cnn.CreateCommand()) {

					foreach (FileInfo file in media_items) {

						//cmd.CommandText = @"SELECT ID FROM audio WHERE (path = @path) AND (ID_mediafolder = @ID_mediafolder)";
						cmd.CommandText = sql_commandtext;

						cmd.Parameters.AddWithValue("@path", file.FullName);
						cmd.Parameters.AddWithValue("@ID_mediafolder", folder_db_id);

						does_exist_id = Convert.ToInt32(cmd.ExecuteScalar());

						// if it's 0 that means the file does not exist in the DB.
						if (does_exist_id == 0) {
							new_media_list[new_media_count] = file;
							new_media_count++;
						}

						//System.Diagnostics.Debug.WriteLine(does_exist_id);

						mediafolder_progress.Value++;
					}
				}
				cnn.Close();
			}

			/* Read the end time. */
			DateTime stopTime = DateTime.Now;

			/* Compute the duration between the initial and the end time. 
				* Print out the number of elapsed hours, minutes, seconds and milliseconds. */
			Duration(stopTime, startTime);

			return new_media_list;
		}

		private void removeMissingFiles(int ID_mediafolder, mediaType mediafolder_type)
		{
			// check to see all media files exist.
			//	remove any that do not.

			string media_path;
			int media_id;
			Dictionary<int, string> media_that_doesnot_exit = new Dictionary<int, string>();
			string sql_commandtext = "";
			string sql_remove_commandtext = "";

			if (mediafolder_type == mediaType.audio) {
				sql_commandtext = @"SELECT ID,path FROM audio WHERE (ID_mediafolder = @ID_mediafolder)";
				sql_remove_commandtext = @"DELETE FROM audio WHERE ID = @ID";
			}
			else if (mediafolder_type == mediaType.video) {
				sql_commandtext = @"SELECT ID,path FROM video WHERE (ID_mediafolder = @ID_mediafolder)";
				sql_remove_commandtext = @"DELETE FROM video WHERE ID = @ID";
			}
			else {
				//sql_commandtext = @"SELECT ID,path FROM audio WHERE (ID_mediafolder = @ID_mediafolder)";
				//sql_remove_commandtext = @"";
			}

			using (SQLiteConnection cnn = new SQLiteConnection(sqldatasource)) {
				using (SQLiteCommand cmd = cnn.CreateCommand()) {
					cnn.Open();
					cmd.CommandText = sql_commandtext;
					cmd.Parameters.AddWithValue("@ID_mediafolder", ID_mediafolder);

					using (SQLiteDataReader reader = cmd.ExecuteReader()) {
						while (reader.Read()) {
							media_path = reader["path"].ToString();
							media_id = Convert.ToInt32(reader["ID"]);

							if (File.Exists(media_path) == false) {
								media_that_doesnot_exit.Add(media_id, media_path);
							}
						}
					}
				}
				cnn.Close();
			}


			// now remove the invalid entries.

			mediafolder_progress.Maximum = media_that_doesnot_exit.Count;
			mediafolder_progress.Value = 0;

			/* Read the initial time. */
			DateTime startTime = DateTime.Now;

			// Create a connection and a command
			using (SQLiteConnection cnn = new SQLiteConnection(sqldatasource)) {
				cnn.Open();

				using (SQLiteTransaction dbTrans = cnn.BeginTransaction()) {
					using (SQLiteCommand cmd = cnn.CreateCommand()) {

						foreach (KeyValuePair<int, string> entry in media_that_doesnot_exit) {
							//System.Diagnostics.Debug.WriteLine(entry.Key + " => " + entry.Value);

							//cmd.CommandText = @"DELETE FROM audio WHERE ID = @ID";
							cmd.CommandText = sql_remove_commandtext;
							cmd.Parameters.AddWithValue("@ID", entry.Key);
							cmd.ExecuteNonQuery();

							mediafolder_progress.Value++;
						}
					}
					dbTrans.Commit();
				}
				cnn.Close();
			}

			/* Read the end time. */
			DateTime stopTime = DateTime.Now;

			/* Compute the duration between the initial and the end time. 
				* Print out the number of elapsed hours, minutes, seconds and milliseconds. */
			Duration(stopTime, startTime);

		}

		private void removeMediaFolderAndFiles(int mediafolder_db_id)
		{
			// this is getting done in 1 shot so I don't do 2 calls to the DB.
			using (SQLiteConnection cnn = new SQLiteConnection(this.sqldatasource)) {
				cnn.Open();

				using (SQLiteCommand cmd = cnn.CreateCommand()) {
					// multi-table deletes are not yet supported in SQLite
					// FIXME: going to need to seperate this for each table
					//cmd.CommandText = "DELETE FROM audio,video WHERE (*.ID_mediafolder = @mediafolder_id); DELETE FROM media_folders WHERE (ID = @mediafolder_id)";
					cmd.CommandText = "DELETE FROM audio WHERE (ID_mediafolder = @mediafolder_id); DELETE FROM video WHERE (ID_mediafolder = @mediafolder_id); DELETE FROM media_folders WHERE (ID = @mediafolder_id)";
					cmd.Parameters.AddWithValue("@mediafolder_id", mediafolder_db_id);
					cmd.ExecuteNonQuery();

					// idea: maybe have a check to do a file not found in the playlist anymore?
				}

				cnn.Close();
			}
		}

		private void insertNewVideoIntoDB(FileInfo[] media_items, int ID_media_folder)
		{
			TagLib.File media = null;

			/* Read the initial time. */
			DateTime startTime = DateTime.Now;

			// Create a connection and a command
			using (SQLiteConnection cnn = new SQLiteConnection(sqldatasource)) {
				cnn.Open();

				using (SQLiteTransaction dbTrans = cnn.BeginTransaction()) {
					using (SQLiteCommand cmd = cnn.CreateCommand()) {
						foreach (FileInfo file in media_items) {
							// with the rescanning we get some nulls at the end, so skip adding if we run into them
							if (file != null) {
								cmd.CommandText = @"INSERT INTO video (path, ID_mediafolder, title, length) VALUES (@path, @ID_mediafolder, @title, @length)";


								// put this in a try/catch and then log any errors

								try {
									media = TagLib.File.Create(file.FullName);

									cmd.Parameters.AddWithValue("@path", file.FullName);
									cmd.Parameters.AddWithValue("@ID_mediafolder", ID_media_folder);
									cmd.Parameters.AddWithValue("@title", (file.Name.Split(new string[] { file.Extension }, StringSplitOptions.None))[0]);
									cmd.Parameters.AddWithValue("@length", media.Properties.Duration.TotalMilliseconds);

									cmd.ExecuteNonQuery();
								}
								catch (Exception ex) {
									Logging.WriteToLog("Adding: '" + file.FullName + "' failed. Error returned: '" + ex.ToString() + "'");
								}
								
							}
						}
					}
					dbTrans.Commit();
				}
				cnn.Close();
			}

			/* Read the end time. */
			DateTime stopTime = DateTime.Now;

			/* Compute the duration between the initial and the end time. 
				* Print out the number of elapsed hours, minutes, seconds and milliseconds. */
			Duration(stopTime, startTime);
		}


		// todo: have 1 insertNewMedia function that takes a FileInfo array for the new media.
		//	then have that function branch off into seperate sections for audio,video, images, etc
		private void insertNewAudioIntoDB(FileInfo[] media_items, int ID_media_folder)
		{
			audioSystem.media_info_struct item_details = new audioSystem.media_info_struct();
			//int insert_count = 0;

			/* Read the initial time. */
			DateTime startTime = DateTime.Now;

			// Create a connection and a command
			using (SQLiteConnection cnn = new SQLiteConnection(sqldatasource)) {
				cnn.Open();

				using (SQLiteTransaction dbTrans = cnn.BeginTransaction()) {
					using (SQLiteCommand cmd = cnn.CreateCommand()) {
						foreach (FileInfo file in media_items) {

							// with the rescanning we get some nulls at the end, so skip adding if we run into them
							if (file != null) {
								item_details = audioSystem.getMediaInfo(file.FullName);

								cmd.CommandText = @"INSERT INTO audio (path, ID_mediafolder, album, artists, date, genres, title, tracknumber, length) VALUES(@path, @ID_mediafolder, @album, @artists, @date, @genres, @title, @tracknumber, @length)";

								cmd.Parameters.AddWithValue("@path", file.FullName);
								cmd.Parameters.AddWithValue("@ID_mediafolder", ID_media_folder);
								cmd.Parameters.AddWithValue("@album", item_details.album);
								cmd.Parameters.AddWithValue("@artists", item_details.artists);
								cmd.Parameters.AddWithValue("@date", item_details.date);
								cmd.Parameters.AddWithValue("@genres", item_details.genres);


								if (item_details.title == null) {
									// use the filename (minus extension) if there is no title in the tags
									cmd.Parameters.AddWithValue("@title", (file.Name.Split(new string[] { file.Extension }, StringSplitOptions.None))[0]);
								}
								else {
									cmd.Parameters.AddWithValue("@title", item_details.title);
								}
								cmd.Parameters.AddWithValue("@tracknumber", item_details.tracknumber);
								cmd.Parameters.AddWithValue("@length", item_details.length);

								cmd.ExecuteNonQuery();
								//insert_count++;
							}

							// update our progress bar
							// fixme: going to have to make this threaded so that the UI can update while the media gets added in the background...
							mediafolder_progress.Value++;
						}
					}
					dbTrans.Commit();
				}
				cnn.Close();
			}

			/* Read the end time. */
			DateTime stopTime = DateTime.Now;

			/* Compute the duration between the initial and the end time. 
				* Print out the number of elapsed hours, minutes, seconds and milliseconds. */
			Duration(stopTime, startTime);

			//return insert_count;
		}

		private int insertNewFolderIntoDB(string path, bool? recursive, int num_folders, int num_files, mediaType mediafolder_type)
		{
			//Console.WriteLine(Convert.ToInt32(recursive));

			int ID_mediafolder_lastinsert = 0;

			using (SQLiteConnection cnn = new SQLiteConnection(this.sqldatasource))
			using (SQLiteCommand cmd = cnn.CreateCommand()) {
				cnn.Open();

				cmd.CommandText = "INSERT INTO media_folders (path, recursive, num_folders, num_files, type) VALUES (@path, @recursive, @num_folders, @num_files, @type)";
				cmd.Parameters.AddWithValue("@path", path);
				cmd.Parameters.AddWithValue("@recursive", Convert.ToInt32(recursive));
				cmd.Parameters.AddWithValue("@num_folders", num_folders);
				cmd.Parameters.AddWithValue("@num_files", num_files);
				cmd.Parameters.AddWithValue("@type", mediafolder_type);

				cmd.ExecuteNonQuery();

				// grab the last insert id and stuff it into our var to return to parent
				cmd.CommandText = "SELECT last_insert_rowid()";
				ID_mediafolder_lastinsert = Convert.ToInt32(cmd.ExecuteScalar());

				cmd.Dispose();
				cnn.Close();
			}

			return ID_mediafolder_lastinsert;
		}

		private int mediaFolderSubDirCount(string path)
		{
			DirectoryInfo[] directories = new DirectoryInfo(path).GetDirectories("*",SearchOption.AllDirectories);

			// return the number of sub folders
			return directories.Length;
		}

		private FileInfo[] mediaFolderScanFiles(string path, bool? recursive, string search_patterns)
		{
			char separator = searchPatterns.seperator;
			string[] patterns = search_patterns.Split(separator);
			int i = 0;
			int tlength = 0;
			int j = 0;
			int rindex = 0;
			DirectoryInfo di = new DirectoryInfo(path);
			FileInfo[][] rgFiles = new FileInfo[patterns.Length][];
			FileInfo[] result;

			SearchOption search_method = SearchOption.TopDirectoryOnly;


			if (recursive == true) {
				search_method = SearchOption.AllDirectories;
			}

			// idea: would be cool to report some stats for the selected folder. ie: how many files of type x,y,z are being added.
			//	very low priority.
			foreach (string pattern in patterns) {
				// builds an array element for each different media type
				rgFiles[i] = di.GetFiles("*." + pattern, search_method);
				i++;
			}

			// count the total number of elements accross the whole array (includes sub elements)
			for (i = 0; i < rgFiles.Length; i++) {
				tlength += rgFiles[i].Length;
			}

			result = new FileInfo[tlength];
			
			// now put it together into one big array
			for (i = 0; i < rgFiles.Length; i++) {
				for (j = 0; j < rgFiles[i].Length; j++) {
					result[rindex++] = rgFiles[i][j];
				}
			}

			// return our array of media items
			return result;
		}


		#endregion



		#region Browse Videos Sub-tab

		private void videos_thumbs_refresh_Click(object sender, RoutedEventArgs e)
		{
			// get vlc to build the thumbnails for us.

		}

		#endregion



	}

}
