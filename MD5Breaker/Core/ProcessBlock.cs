using MD5Breaker.Networking;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;


namespace MD5Breaker.Core
{
    public enum BlockState
    {
        Free = 0,
        Processing,
        Finished
    }

    public class ProcessBlock
    {
        public ulong BlockId { get; set; }
        public BlockState State;

        public ProcessBlock(ulong blockId, BlockState state)
        {
            this.BlockId = blockId;
            this.State = state;
        }
    }
}
