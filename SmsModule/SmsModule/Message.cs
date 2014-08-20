using System;
using System.Collections.Generic;
using System.Text;

namespace SmsModule
{
    class Message
    {
        private string _phoneNumber = "";

        private string _message = "";

        public string PhoneNumber
        {
            set
            {
                _phoneNumber = value;
            }
            get
            {
                return _phoneNumber;
            }
        }

        public string Text
        {
            set
            {
                _message = value;
            }
            get
            {
                return _message;
            }
        }

    }
}
