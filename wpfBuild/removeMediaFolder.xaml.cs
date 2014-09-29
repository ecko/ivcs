using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data.SQLite;

namespace wpfBuild
{
    /// <summary>
    /// Interaction logic for removeMediaFolder.xaml
    /// </summary>
    public partial class removeMediaFolder : Window
    {
        public static Boolean user_accept = false;

        //public uint[] media_folder_ids_array = new uint[512];

        public removeMediaFolder(uint[] ids_array)
        {
            InitializeComponent();

            // populate our global array
            int i = 0;
            string folder_id_string = "";
            while (i < ids_array.Length)
            {
            //    media_folder_ids_array[i] = ids_array[i];
                folder_id_string += ids_array[i].ToString() + ";";
                i++;
            }
            folder_id_array.Content = folder_id_string;

            label_item_count.Content = get_number_of_items_for_removal(ids_array);

        }

        private void Window_Closed(object sender, EventArgs e)
        {
            // return our value back to the old form so we can proceed or stop.
            //return user_accept;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            user_accept = true;
            //Window1.removeAudio(1);
            //wpfBuild.Window1.
            //MessageBox.Show(wpfBuild.Properties.Settings.Default.datasource);

            removeAudio();

            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public uint get_number_of_items_for_removal(uint[] ids_array)
        {
            // count and return the # of media items that are going to be removed.
            string query = " (ID_mediafolder = " + ids_array[0] + ")";
            uint count = 0;

            for (int x = 1; x < ids_array.Length; x++)
            {
                query += " OR (ID_mediafolder = " + ids_array[x] + ")";
            }

            // Create a connection and a command
            using (SQLiteConnection cnn = new SQLiteConnection(wpfBuild.Properties.Settings.Default.datasource))
            {
                cnn.Open();
                using (SQLiteCommand cmd = cnn.CreateCommand())
                {
                    cmd.CommandText = "SELECT COUNT(*) FROM audio WHERE" + query;
                    count = Convert.ToUInt32(cmd.ExecuteScalar());
                }
                cnn.Close();
            }
            return count;
        }

        private void removeAudio()
        {
            // Create a connection and a command
            using (SQLiteConnection cnn = new SQLiteConnection(wpfBuild.Properties.Settings.Default.datasource))
            {
                cnn.Open();

                /* Read the initial time. */
                DateTime startTime = DateTime.Now;
                Console.WriteLine(startTime);

                using (SQLiteCommand cmd = cnn.CreateCommand())
                {
                    // maybe: delete FROM audio,media_folders WHERE ....

                    //string folder_id_string = folder_id_array.Content;

                    string[] media_folder_ids_array = ((String)folder_id_array.Content).Split(';');

                    remove_progress.Maximum = media_folder_ids_array.Length;
                    remove_progress.Value = 0;

                    for (int x = 0; x < media_folder_ids_array.Length; x++)
                    {
                        // when we split it, the last array item will be "".  so if we are on that item, or any null item, skip to the next
                        if (media_folder_ids_array[x] == "")
                        {
                            continue;
                        }
                        // multi-table deletes are not yet supported in SQLite
                        cmd.CommandText = "DELETE FROM audio WHERE (ID_mediafolder = @mediafolder_id)";
                        cmd.Parameters.AddWithValue("@mediafolder_id", media_folder_ids_array[x]);
                        cmd.ExecuteNonQuery();

                        cmd.CommandText = "DELETE FROM media_folders WHERE (ID = @mediafolder_id)";
                        cmd.Parameters.AddWithValue("@mediafolder_id", media_folder_ids_array[x]);
                        cmd.ExecuteNonQuery();

                        remove_progress.Value++;
                    }

                }
                /* Read the end time. */
                DateTime stopTime = DateTime.Now;
                Console.WriteLine(stopTime);


                /* Compute the duration between the initial and the end time. 
                    * Print out the number of elapsed hours, minutes, seconds and milliseconds. */
                TimeSpan duration = stopTime - startTime;
                Console.WriteLine("seconds:" + duration.TotalSeconds);
                Console.WriteLine("milliseconds:" + duration.TotalMilliseconds);

                cnn.Close();
            }
        }
    }


}
