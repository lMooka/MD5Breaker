using MD5Breaker.Networking;
using MD5Breaker.Networking.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MD5Breaker.Core
{
    public class ProcessingManager
    {
        // Singleton
        private static ProcessingManager _instance;
        public static ProcessingManager Instance
        {
            get
            {
                return (_instance ?? (_instance = new ProcessingManager()));
            }
        }

        public static uint BlockSize = (uint)(MD5Decrypter.CharRange.Length * MD5Decrypter.CharRange.Length * MD5Decrypter.CharRange.Length * MD5Decrypter.CharRange.Length);

        private List<ProcessBlock> blocks;

        private ProcessingManager()
        {
            blocks = new List<ProcessBlock>();
        }

        public void InitBlocks(DecrypterRange range)
        {
            blocks = new List<ProcessBlock>();

            ulong length = 1;

            foreach (int element in range.endRange)
                length *= Convert.ToUInt64(element);

            for (ulong plus = 0, id = 0; plus <= length; plus += BlockSize, id++)
            {
                range.Plus(plus);
                blocks.Add(new ProcessBlock(id, range.Clone(), BlockState.Free));
            }
        }

        public ProcessBlock GetFreeBlock()
        {
            return blocks.Find(b => b.State == BlockState.Free);
        }

        public void SetProcessingState(ulong id, BlockState state)
        {
            var block = blocks.Find(b => b.BlockId == id);
            block.State = state;
        }

        public void ProcessBlock()
        {
            var block = GetFreeBlock();
            ConnectionManager.Instance.Broadcast(new ProcessingBlockNotificationPacket(block.BlockId, BlockState.Processing));
        }
    }
}
