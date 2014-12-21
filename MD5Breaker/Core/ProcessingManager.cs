using MD5Breaker.Core.Exceptions;
using MD5Breaker.Networking;
using MD5Breaker.Networking.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace MD5Breaker.Core
{
    public class ProcessingManager
    {
        // Events

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

        public bool Initialized = false;
        public string Hash = "";

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
            BlockSize = length / Convert.ToUInt64(1000 * range.endRange.Length);

            for (ulong plus = 0, id = 0; plus <= length; plus += BlockSize, id++)
                blocks.Add(new ProcessBlock(id, BlockState.Free));

            Random rnd = new Random();
            blocks = blocks.OrderBy(item => rnd.Next()).ToList<ProcessBlock>();

            Initialized = true;
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
            ConnectionManager.Instance.Broadcast(new ProcessingBlockNotifyPacket(block.BlockId, block.State));
        }

        public void Crack(string hash)
        {
            if (!Initialized)
                return;

            ProcessBlock block = GetFreeBlock();
            ConnectionManager.Instance.Broadcast(new ProcessingBlockNotifyPacket(block.BlockId, BlockState.Processing));

            Cracker cracker = new Cracker(hash, block);
            cracker.OnCompleted += OnCompleted;

            if (ProcessingThread != null)
            {
                //MessageBox.Show(ProcessingThread.ThreadState.ToString());
                ProcessingThread = null;
            }

            ProcessingThread = new Thread(new ThreadStart(cracker.Run));
            ProcessingThread.Start();
            //MessageBox.Show("cracking " + block.BlockId);
        }

        void OnCompleted(ProcessBlock block, Exception exc)
        {
            if (exc is HashFoundException)
            {
                this.Initialized = false;
                SetProcessingState(block.BlockId, BlockState.Finished);
                ConnectionManager.Instance.Broadcast(new HashFoundPacket(exc.Message));

                //ConnectionManager.Instance.Broadcast(new ProcessingBlockNotifyPacket(block.BlockId, BlockState.Finished));
                //ConnectionManager.Instance.Broadcast(new MessagePacket(string.Format("{0}: Block {1} - {2}", ConnectionManager.Instance.ListenPort, block.BlockId, BlockState.Finished)));
                MessageBox.Show(exc.Message);
                //ProcessingThread.Abort();
                
                return;
            }
            else if(exc is HashNotFoundException)
            {
                ConnectionManager.Instance.Broadcast(new ProcessingBlockNotifyPacket(block.BlockId, BlockState.Finished));
                //ConnectionManager.Instance.Broadcast(new MessagePacket(string.Format("{0}: Block {1} - {2}", ConnectionManager.Instance.ListenPort, block.BlockId, block.State.ToString())));
                SetProcessingState(block.BlockId, BlockState.Finished);
                
                this.Crack(Hash);
            }
        }

        public void Setup(string hash, DecrypterRange decrypterRange)
        {
            this.Hash = hash;

            ProcessingManager.Instance.InitBlocks(decrypterRange);
            ConnectionManager.Instance.Broadcast(new InitBlocksPacket(hash, decrypterRange));
            ProcessingManager.Instance.Crack(Hash);
        }
    }
}
