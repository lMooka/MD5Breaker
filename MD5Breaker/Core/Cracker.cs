using MD5Breaker.Core.Exceptions;
using MD5Breaker.Networking;
using MD5Breaker.Networking.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MD5Breaker.Core
{
    public class Cracker
    {
        public event BlockProcessed OnCompleted;

        public string currentString;
        private MD5Decrypter decrypter;
        private ProcessBlock block;
        private ulong stopNumber;

        public Cracker(string hash, ProcessBlock block)
        {
            this.block = block;
            decrypter = new MD5Decrypter(hash, new DecrypterRange(block.BlockId, ProcessingManager.BlockSize, Convert.ToUInt32(MD5Decrypter.CharRange.Length)));
            stopNumber = decrypter.Range.GetCombinationsAmount();
        }

        public void Run()
        {
            try
            {
                decrypter.Crack(decrypter.Range.GetCombinationsAmount());
            }
            catch (HashFoundException e)
            {
                OnCompleted(block, e);
            }
            catch (HashNotFoundException e)
            {
                OnCompleted(block, e);
            }
        }
    }
}
