/*
 *		Logging
 * A small class that is used to log messages to a file. This is done manually, it does not automatically catch exceptions and log them.
 * 
 * log path will have to be updated in the future, possible from a settings table in the database.
 */
using System;
using System.Text;
using System.IO;

namespace wpfBuild
{
	public class Logging
	{
		public static string log_path = @"C:/ivcs/error.log";

		/// <summary>
		/// Pass in the fully qualified name of the log file you want to write to
		/// and the message to write
		/// </summary>
		/// <param name="LogPath"></param>
		/// <param name="Message"></param>
		public static void WriteToLog(string Message)
		{
			try {
				using (StreamWriter s = File.AppendText(log_path)) {
					s.WriteLine("[" + DateTime.Now + "]\t" + Message);
				}
			}

			catch (Exception ex) {
				System.Diagnostics.Debug.WriteLine(ex.Message);
			}
		}

	}
}
