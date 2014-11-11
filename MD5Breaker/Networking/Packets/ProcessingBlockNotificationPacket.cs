using MD5Breaker.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MD5Breaker.Networking.Packets
{
    public class ProcessingBlockNotificationPacket : Packet
    {
        private ulong _BlockId;
        private BlockState _State;

        public ulong BlockId
        {
            get
            {
                return _BlockId;
            }
            set
            {
                _BlockId = value;
                WriteULong(value, HeaderSize);
            }
        }
        public BlockState State
        {
            get
            {
                return _State;
            }
            set
            {
                _State = value;
                WriteBlockState(value, HeaderSize + sizeof(ulong));
            }
        }

        public ProcessingBlockNotificationPacket(ulong blockId, BlockState state)
            : base((ushort)(HeaderSize + sizeof(ulong) + sizeof(int)), 3)
        {
            this.BlockId = blockId;
            this.State = state;
        }

        public ProcessingBlockNotificationPacket(byte[] buf)
            : base((ushort)buf.Length, 3)
        {
            _BlockId = BitConverter.ToUInt64(buf, HeaderSize);
            _State = (BlockState) Enum.Parse(typeof(BlockState), BitConverter.ToUInt32(buf, HeaderSize + sizeof(ulong)).ToString());
        }

        public void WriteBlockState(BlockState state, int offset)
        {
            Buffer.BlockCopy(BitConverter.GetBytes((int)state), 0, buffer, offset, sizeof(int));
        }
    }
}
