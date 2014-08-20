using System;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;

using GsmComm.PduConverter;
using GsmComm.PduConverter.SmartMessaging;
using GsmComm.GsmCommunication;
using GsmComm.Interfaces;
using GsmComm.Server;

namespace SmsModule
{
    public class Sms
    {
        private GsmCommMain comm = null;
        public Sms(int port)
        {
            int port1 = GsmCommMain.DefaultPortNumber;
			int baudRate = GsmCommMain.DefaultBaudRate;
			int timeout = GsmCommMain.DefaultTimeout;

			comm = new GsmCommMain(port, baudRate, timeout);
			Cursor.Current = Cursors.Default;
			comm.PhoneConnected += new EventHandler(comm_PhoneConnected);
			comm.PhoneDisconnected += new EventHandler(comm_PhoneDisconnected);
			bool retry;
			retry = false;
				try
				{
					comm.Open();
				}
				catch(Exception)
				{
					if (MessageBox.Show("Unable to open the port.", "Error",
						MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning) == DialogResult.Retry)
						retry = true;
					else
					{
						return;
					}
				}
        }
        private void comm_PhoneConnected(object sender, EventArgs e)
        {
          //  MessageBox.Show("Phone Connected");
          //  lblNotConnected.Visible = true;
        }

        private void comm_PhoneDisconnected(object sender, EventArgs e)
        {
        //    MessageBox.Show("Phone DisConnected");
            //lblNotConnected.Visible = !false;
        }
        public  bool Send(System.Collections.ArrayList messageList, bool chkAlert, bool chkUnicode)
        {
            bool retValue = true;
      
            try
            {
                // If SMS batch mode should be activated, do it before sending the first message
                if (messageList.Count > 1)
                {
                    comm.EnableTemporarySmsBatchMode();
                }

                // Send an SMS message
                SmsSubmitPdu pdu;
                bool alert = chkAlert;
                bool unicode = chkUnicode;
                string smsc = string.Empty;
                SmsModule.Message msg = null;
                for (int count = 0; count < messageList.Count; count++)
                {
                    msg = (Message)messageList[count];
                    if (!alert && !unicode)
                    {
                        // The straightforward version
                        pdu = new SmsSubmitPdu(msg.Text, msg.PhoneNumber, smsc);
                    }
                    else
                    {
                        // The extended version with dcs
                        byte dcs;
                        if (!alert && unicode)
                            dcs = DataCodingScheme.NoClass_16Bit;
                        else if (alert && !unicode)
                            dcs = DataCodingScheme.Class0_7Bit;
                        else if (alert && unicode)
                            dcs = DataCodingScheme.Class0_16Bit;
                        else
                            dcs = DataCodingScheme.NoClass_7Bit; // should never occur here

                        pdu = new SmsSubmitPdu(msg.Text, msg.PhoneNumber, smsc, dcs);
                    }


                    // Send the message the specified number of times


                    comm.SendMessage(pdu);
                    System.Threading.Thread.Sleep(2000);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        
            return retValue;
        }
    }
}
