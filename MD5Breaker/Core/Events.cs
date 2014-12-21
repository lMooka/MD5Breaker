using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MD5Breaker.Core
{
    public delegate void ChatMessage(string message);
    public delegate void BlockProcessed(ProcessBlock block, Exception exc);
}
