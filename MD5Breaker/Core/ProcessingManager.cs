using MD5Breaker.Core.Exceptions;
using MD5Breaker.Networking;
using MD5Breaker.Networking.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

        public static ulong BlockSize;

        private DecrypterRange range;
        private List<ProcessBlock> blocks;
        public Thread ProcessingThread;

        private ProcessingManager()
        {
            blocks = new List<ProcessBlock>();
        }

        public void InitBlocks(DecrypterRange range)
        {
            this.range = range;
            ulong length = range.GetNumber();

            blocks.Clear();
            BlockSize = length / Convert.ToUInt64(10000 * range.endRange.Length);

            for (ulong plus = 0, id = 0; plus <= length; plus += BlockSize, id++)
                blocks.Add(new ProcessBlock(id, BlockState.Free));
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

        public void ProcessHash(string hash, DecrypterRange range)
        {
            InitBlocks(range);

        }

        public void ProcessBlock()
        {
            var block = GetFreeBlock();
            block.State = BlockState.Processing;
            ConnectionManager.Instance.Broadcast(new ProcessingBlockNotificationPacket(block.BlockId, block.State));
        }

        public void Crack(string hash)
        {
            ProcessBlock block = GetFreeBlock();
            ConnectionManager.Instance.Broadcast(new ProcessingBlockNotificationPacket(block.BlockId, BlockState.Processing));

            Cracker r = new Cracker(hash, block);
            ProcessingThread = new Thread(new ThreadStart(r.Run));
            ProcessingThread.Start();
        }

        public void SetRange(DecrypterRange range)
        {
            this.range = range;
        }
    }
}
