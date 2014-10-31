using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MD5Breaker.Core
{
    public class DecrypterRange
    {
        public int charOffset { get; private set; }

        public int[] startRange;
        public int[] endRange;
        public int[] currentRange;

        public DecrypterRange(int charOffset)
        {
            this.charOffset = charOffset;
        }

        public void setStartRange(params int[] range)
        {
            startRange = range;

            if (currentRange == null)
                currentRange = (int[])startRange.Clone();
        }

        public void setEndRange(params int[] range)
        {
            endRange = range;
        }

        public int[] Next()
        {
            int[] current = currentRange;
            int i;

            for (i = currentRange.Length-1; i >= 0; i--)
            {
                currentRange[i]++;

                if (currentRange[i] < charOffset)
                    break;
                else
                {
                    currentRange[i] = 0;

                    if (i == 0)
                    {
                        int[] newRange = new int[currentRange.Length + 1];
                        newRange[0] = 0;

                        int j = 1;
                        foreach (int cr in currentRange)
                            newRange[j++] = cr;

                        currentRange = newRange;
                    }
                }
            }

            return current;
        }
    }
}
