﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MD5Breaker.Networking.Packets
{
    public class MessagePacket : Packet
    {
        private string _message;

        public string Message
        {
            get
            {
                return _message;
            }
            private set
            {
                _message = value;
                WriteString(value, HeaderSize);
            }
        }

        public MessagePacket(string msg)
            : base((ushort)(HeaderSize + msg.Length), 1)
        {
            this.Message = msg;
        }

        public MessagePacket(byte[] buf)
            : base(buf)
        {
            _message = Encoding.UTF8.GetString(buf, HeaderSize, buf.Length - Packet.HeaderSize);
        }
    }
}
