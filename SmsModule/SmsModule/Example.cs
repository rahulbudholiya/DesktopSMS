using System;
using System.Collections.Generic;
using System.Text;

namespace SmsModule
{
    class Example
    {
        public static void SendSms()
        {
            Sms s = new Sms(16);
            System.Collections.ArrayList arr = new System.Collections.ArrayList();
            Message m = new Message();
            m.PhoneNumber = "+919752757644";
            m.Text = "work has been done rahul budholiya!";
            arr.Add(m);
            s.Send(arr, false, false);

        }
    }
}
