/*
 *		Control tab
 *	This section communicates with the external control board using a USB connection. The control board however, is programmed to emulate a RS-232 connection to help make this code easier.
 *	
 * Very similar setup to the OBD section. However, sending commands to the external board are ONLY done when the button is clicked (to toggle its output).
 * Reading is continuously.
 *	
 */

using System;
using System.Windows;
using System.Windows.Controls;
using System.IO.Ports;
using System.Windows.Threading;

namespace wpfBuild
{
	public partial class Window1 : Window
	{
		Boolean control_connected = false;
		SerialPort control_com = null;

		//DispatcherTimer control_talk_timer = new DispatcherTimer();

		private void control_com_rescan_Click(object sender, RoutedEventArgs e)
		{
			// check how many obd devices there are

			controlLogWrite("Scanning for COM ports");

			if (scanForComPorts(this.control_com_list) == true) {
				control_com_connect.IsEnabled = true;
				controlLogWrite("Found " + this.control_com_list.Items.Count.ToString() + " COM ports");
			}
			else {
				Logging.WriteToLog("No COM ports found");
				controlLogWrite("No COM ports found");
				control_com_connect.IsEnabled = false;
			}

		}

		/*
		 * Can be found in tab_OBD.cs
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
		 */

		private void control_com_connect_Click(object sender, RoutedEventArgs e)
		{
			Button me = (Button)sender;

			if (control_com_list.SelectedIndex != -1) {

				string com_selected = ((ComboBoxItem)control_com_list.SelectedItem).Content.ToString();

				if (this.control_connected == false) {

					controlLogWrite("Establishing connection to " + com_selected + "...");
					this.control_status_message.Text = "Connecting...";

					//try {
						if (controlConnect(com_selected) == true) {
							me.Content = "Disconnect";

							controlLogWrite("Connection established");
							this.control_status_message.Text = "Connected";
						}
					//}
					//catch (Exception ex) {
					//	MessageBox.Show("obdConnect problem: " + com_selected + ex.Message);

					//	controlLogWrite("Could not connect: " + ex.Message);
					//	this.control_status_message.Text = "FAILED";
					//}

				}
				else {

					// remove some of the old event handlers
					try {
						this.control_com.DataReceived -= control_com_DataReceived;
						this.control_com.ErrorReceived -= control_com_ErrorReceived;
					}
					catch (Exception ex) {
						controlLogWrite("Error removing event handlers: " + ex.Message);
					}
					
					
					try {
						control_com.Close();
						//control_talk_timer.IsEnabled = false;
						//obd_timer = null;
					}
					catch (Exception ex) {
						Logging.WriteToLog(ex.Message);
						//MessageBox.Show(ex.Message);
						controlLogWrite(ex.Message);
						this.control_status_message.Text = "FAILED";
					}

					control_com_list.IsEnabled = true;
					this.control_connected = false;
					me.Content = "Connect";
					controlLogWrite("Disconnected from COM port");
					this.control_status_message.Text = "Disconnected";
				}
			}
			else {
				controlLogWrite("Please select a COM port to connect to");
			}
		}

		private bool controlConnect(string com_selected)
		{
			string message = "";
			
			try {
				//this.control_com = new SerialPort(com_selected, 38400, Parity.None, 8, StopBits.One);
				this.control_com = new SerialPort(com_selected, 115200, Parity.None, 8, StopBits.One);
			}
			catch (Exception ex) {
				message = "Error creating COM object: " + com_selected + " " + ex.Message;
				Logging.WriteToLog(message);
				//MessageBox.Show("Error creating COM object: " + com_selected + ex.Message);

				controlLogWrite(message);
				this.control_status_message.Text = "FAILED";
			}

			this.control_com.ErrorReceived += new SerialErrorReceivedEventHandler(control_com_ErrorReceived);
			this.control_com.NewLine = "\r\n";

			try {
				this.control_com.Open();
			}
			catch (Exception ex) {
				message = "Error opening COM port: " + com_selected + " " + ex.Message;

				//if (ex. == System.UnauthorizedAccessException
				//MessageBox.Show("Error opening OBD COM port: " + com_selected + ex.Message);

				Logging.WriteToLog(message);
				controlLogWrite(message);
				this.control_status_message.Text = "FAILED";
			}

			try {
				this.control_com.DataReceived += new SerialDataReceivedEventHandler(control_com_DataReceived);
			}
			catch {
				message = "Error attching data receive listner";
				Logging.WriteToLog(message);
				controlLogWrite(message);
			}

			message = null;

			
			if (this.control_com.IsOpen == true) {
				control_com_list.IsEnabled = false;
				this.control_connected = true;

				/*
				control_talk_timer.Interval = TimeSpan.FromMilliseconds(1000);
				//obd_timer.Tick = null;
				control_talk_timer.Tick += new EventHandler(control_talk_timer_Tick);
				control_talk_timer.IsEnabled = true;
				*/


				return true;
			}
			 

			return false;
		}

		private void control_toggle_Click(object sender, RoutedEventArgs e)
		{
			string message;

			if (control_com != null) {
				if (control_com.IsOpen == true) {
					// do we need to check something to see if we're clear to send?
					if (control_toggle.IsChecked == true) {
						message = "1";
						control_com.Write(message);
					}
					else {
						message = "0";
						control_com.Write(message);
					}
					controlLogWrite("> " + message);
				}
			}
		}
		/*

		void control_talk_timer_Tick(object sender, EventArgs e)
		{
			string message;

			if (control_com.IsOpen == true) {
				if (control_toggle.IsChecked == true) {
					message = "1";
					control_com.Write(message);
				}
				else {
					message = "0";
					control_com.Write(message);
				}
				controlLogWrite("> " + message);
			}

		}

		 * */
		void control_com_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
		{
			string message = e.EventType.ToString() + " " + e.ToString() + " " + e.GetType().ToString();
			Logging.WriteToLog(message);
			controlLogWrite("ERROR: " + message);
		}

		void control_com_DataReceived(object sender, SerialDataReceivedEventArgs e)
		{
			string message;
			try {
				// to do, might have to change this to a read bytes or readline and then parse.
				//string received_data = control_com.ReadExisting();
				string received_data = control_com.ReadLine();

				this.Dispatcher.BeginInvoke(
						DispatcherPriority.Normal,
						new NextPrimeDelegate((Action)(() =>
						{
							control_log.Text += received_data;

							// only print out a newline if there's no more data to read.
							//	don't want it to spit out newlines in the middle of receives.
							// STILL DOESN'T WORK PROPERLY
							//if (control_com.BytesToRead == 0) {
							//	obd_log.Text += "\r\n";
							//}
							control_log.ScrollToEnd();
						}))
				);
			}
			catch (Exception ex) {
				message = "Receive error: " + ex.Message;
				Logging.WriteToLog(message);
				controlLogWrite(message);
			}

		}

		private void controlLogWrite(string message)
		{
			control_log.Text += message + "\r\n";
			control_log.ScrollToEnd();
		}


	}

}