/*
 *		OBD tab section
 *	This section reads the diagnostic information from the OBD board connected to the OBD-II port of the vehicle.
 *	The board emulates a serial connection which is why this code opens, closes and communicates over a COM port.
 *	
 */

using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
using System.Windows;
using System.Windows.Controls;
//using System.Windows.Data;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using System.Windows.Navigation;
//using System.Windows.Shapes;

//using System.Windows.Threading;
//using System.Data.SQLite;
//using System.Data.Common;
using System.IO.Ports;
using System.Windows.Threading;

//using System.Windows.Media.Animation;
//using System.Collections.ObjectModel;

//using System.Net;
//using System.Windows.Navigation;

//using KineticScrollingPrototype;
//using System.Globalization;
//using System.Collections;

namespace wpfBuild
{
	public partial class Window1 : Window
	{
		Boolean obd_connected = false;
		SerialPort obd_com = null;

		DispatcherTimer obd_timer = new DispatcherTimer();

		private void obd_com_rescan_Click(object sender, RoutedEventArgs e)
		{
			// check how many obd devices there are

			obdLogWrite("Scanning for COM ports");

			if (scanForComPorts(this.obd_com_list) == true) {
				obd_com_connect.IsEnabled = true;
			}
			else {
				Logging.WriteToLog("No COM ports found");
				obd_com_connect.IsEnabled = false;
			}

		}

		private Boolean scanForComPorts(ComboBox com_list_box)
		{
			com_list_box.Items.Clear();

			foreach (string com_name in SerialPort.GetPortNames()) {
				ComboBoxItem new_combo_item = new ComboBoxItem();
				new_combo_item.Content = com_name;
				com_list_box.Items.Add(new_combo_item);
			}

			if (com_list_box.Items.Count > 0) {
				com_list_box.SelectedIndex = 0;
				return true;
			}

			return false;
		}

		private void obd_com_connect_Click(object sender, RoutedEventArgs e)
		{
			Button me = (Button)sender;

			if (obd_com_list.SelectedIndex != -1) {

				string com_selected = ((ComboBoxItem)obd_com_list.SelectedItem).Content.ToString();

				if (this.obd_connected == false) {
					//connectOBD(
					//Console.WriteLine(obd_com_list.SelectedValue);

					try {
						if (obdConnect(com_selected) == true) {
							me.Content = "Disconnect";
						}
					}
					catch (Exception ex) {
						MessageBox.Show("obdConnect problem: " + com_selected + ex.Message);
					}
					
				}
				else {

					try {
						obd_com.Close();
						obd_timer.IsEnabled = false;
						//obd_timer = null;
					}
					catch (Exception ex) {
						Logging.WriteToLog(ex.Message);
						MessageBox.Show(ex.Message);
					}
					
					obd_com_list.IsEnabled = true;
					this.obd_connected = false;
					me.Content = "Connect";
				}
			}
		}

		private Boolean obdConnect(string com_selected)
		{
			try {
				//this.obd_com = new SerialPort(com_selected, 38400, Parity.None, 8, StopBits.One);
				this.obd_com = new SerialPort(com_selected, 115200, Parity.None, 8, StopBits.One);
			}
			catch (Exception ex) {
				Logging.WriteToLog("Error creating COM object: " + com_selected + ex.Message);
				MessageBox.Show("Error creating COM object: " + com_selected + ex.Message);
			}

			this.obd_com.ErrorReceived += new SerialErrorReceivedEventHandler(obd_com_ErrorReceived);
			//this.obd_com.NewLine = "\r\n";
			// FIXME: check if this is going to work.  it should work for the rpm communications but others might break
			this.obd_com.ReceivedBytesThreshold = 2;
			//this.obd_com.ReceivedBytesThreshold = 4;
			
			try {
				this.obd_com.Open();
			}
			catch (Exception ex) {
				Logging.WriteToLog("Error opening OBD COM port: " + com_selected + ex.Message);
				MessageBox.Show("Error opening OBD COM port: " + com_selected + ex.Message);
			}

			try {
				this.obd_com.DataReceived += new SerialDataReceivedEventHandler(obd_com_DataReceived);
			}
			catch {
				Logging.WriteToLog("Error attching OBD data receive listner");
			}

			if (this.obd_com.IsOpen == true) {
				obd_com_list.IsEnabled = false;
				this.obd_connected = true;

				obd_timer.Interval = TimeSpan.FromMilliseconds(750);
				//obd_timer.Tick = null;
				obd_timer.Tick += new EventHandler(obd_timer_Tick);
				obd_timer.IsEnabled = true;



				return true;
			}

			return false;	
		}

		void obd_timer_Tick(object sender, EventArgs e)
		{
			//throw new NotImplementedException();
			// port.Write(new byte[] {0x0A, 0xE2, 0xFF}, 0, 3)

			//return;

			/*

			if (obd_com.IsOpen == true) {
				//obd_com.Write(Convert.ToBoolean(obd_toggle_led.IsChecked).ToString());
				if (obd_toggle_led.IsChecked == true) {
					obd_com.Write("1");
				}
				else {
					obd_com.Write("0");
				}
			}
			*/

			string message;

			if (obd_com.IsOpen == true) {

				if (obd_toggle.IsChecked == false) {
					obd_com.Write(new byte[] { 0x01, 0x0C, 0x0D, 0x0A }, 0, 4);
					message = System.Text.ASCIIEncoding.ASCII.GetString(new byte[] { 0x01, 0x0C, 0x0D, 0x0A }, 0, 4);

					//obd_com.Write(new byte[] { 0x01, 0x0C}, 0, 2);
					//message = System.Text.ASCIIEncoding.ASCII.GetString(new byte[] { 0x01, 0x0C}, 0, 2);
					//message = "010c";
					//obd_com.Write(message);

				}
				else {
					obd_com.Write(obd_message.Text);
					message = obd_message.Text;
				}

				obdLogWrite("> " + message);
			}

		}

		void obd_com_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
		{
			string message = e.EventType.ToString() + " " + e.ToString() + " " + e.GetType().ToString();
			Logging.WriteToLog(message);
			obdLogWrite("ERROR: " + message);
		}

		void obd_com_DataReceived(object sender, SerialDataReceivedEventArgs e)
		{
			// FIXME:  might be a problem if we don't know if we're at the start of the frame
			
			try {
				// to do, might have to change this to a read bytes or readline and then parse.
				string received_data = obd_com.ReadExisting();
				//string received_data = obd_com.ReadLine();

				//if (obd_com.

				this.Dispatcher.BeginInvoke(
						DispatcherPriority.Normal,
						new NextPrimeDelegate((Action)(() =>
						{
							obd_log.Text += received_data;

							// only print out a newline if there's no more data to read.
							//	don't want it to spit out newlines in the middle of receives.
							// STILL DOESN'T WORK PROPERLY
							//if (obd_com.BytesToRead == 0) {
							//	obd_log.Text += "\r\n";
							//}
							obd_log.ScrollToEnd();
							
							
							updateTestRPM(received_data);
							
						}))
				);
			}
			catch (Exception ex) {
				Logging.WriteToLog("Receive error: " + ex.Message);
				MessageBox.Show("Receive error: " + ex.Message);
			}

			

		}

		private void updateTestRPM(string received_data)
		{

			//((A*256)+B)/4

			string[] received_bytes = received_data.Split(' ');

			// should return 01 41 XX XX for RPM

			if (received_bytes.Length > 2) {

				string result = (((Convert.ToByte(received_bytes[2]) * 256) + Convert.ToByte(received_bytes[3])) / 4).ToString();
				obd_rpm.Text = result + " RPM";
			}
			else {
				// I think we get something like "NO DATA" if there's a problem / it doesn't understand the command we sent.
				//	so, if we don't have more than 2 items in the array there's likely a problem reading the RPM.
				obdLogWrite("incorrect receive bytes: " + received_bytes.Length);



			}

			// maybe just call the write at this point?
			// probably not such a good idea because we can't control the update speed then.

		}

		private void obd_toggle_Checked(object sender, RoutedEventArgs e)
		{

		}

		private void obdLogWrite(string message)
		{
			obd_log.Text += message + "\r\n";
			obd_log.ScrollToEnd();
		}


	}

}
