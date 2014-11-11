using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MD5Breaker.Core
{
    public class DecrypterRange
    {
        public uint charOffset { get; private set; }

        public uint[] startRange;
        public uint[] endRange;
        public uint[] currentRange;

        public DecrypterRange(uint[] startRange, uint[] endRange, uint charOffset)
        {
            this.charOffset = charOffset;
            this.startRange = startRange;
            this.endRange = endRange;

            if (currentRange == null)
                currentRange = (uint[])startRange.Clone();
        }

        public void Next()
        {
            uint[] current = currentRange;
            int i;

            for (i = currentRange.Length - 1; i >= 0; i--)
            {
                currentRange[i]++;

                if (currentRange[i] < charOffset)
                    break;
                else
                {
                    currentRange[i] = 0;

                    if (i == 0)
                    {
                        uint[] newRange = new uint[currentRange.Length + 1];
                        newRange[0] = 0;

                        int j = 1;
                        foreach (uint cr in currentRange)
                            newRange[j++] = cr;

                        currentRange = newRange;
                    }
                }
            }
        }

        /* Plus(ulong value)
         * Adiciona um valor ao Range.
         */
        public void Plus(ulong value)
        {
            int i;
            var result = RecursivePlus(value, currentRange);

            uint[] current = currentRange;

            for (i = currentRange.Length - 1; i >= 0; i--)
            {
                result[i] += currentRange[i];

                if (result[i] < charOffset)
                    break;
                else
                {
                    result[i] = 0;

                    if (i == 0)
                    {
                        uint[] newRange = new uint[result.Length + 1];
                        newRange[0] = 0;

                        int j = 1;
                        foreach (uint cr in result)
                            newRange[j++] = cr;

                        result = newRange;
                    }
                }
            }

            currentRange = result;
        }

        public uint[] RecursivePlus(ulong value, uint[] array)
        {
            ulong plusNext = (value / charOffset);
            ulong lastValue = array[array.Length - 1] + (value - (plusNext * charOffset));

            if (plusNext > 0)
            {
                uint[] nArray;
                if (plusNext > charOffset && array.Length == 1)
                {
                    nArray = new uint[array.Length];
                    nArray[0] = 0;

                    for (int i = 1, j = 0; i < nArray.Length - 2; i++, j++)
                        nArray[i] = array[j];
                }
                else if (array.Length > 1)
                {
                    nArray = new uint[array.Length - 1];
                    for (int i = 0; i < array.Length - 2; i++)
                        nArray[i] = array[i];
                }
                else
                {
                    return new uint[] { Convert.ToUInt32(lastValue) };
                }

                uint[] recursiveResult = RecursivePlus(plusNext, nArray);
                uint[] result = new uint[recursiveResult.Length + 1];

                for (int i = 0; i < result.Length - 1; i++)
                    result[i] = recursiveResult[i];

                result[result.Length - 1] = Convert.ToUInt32(lastValue);

                return result;
            }
            else
            {
                array[array.Length - 1] = Convert.ToUInt32(lastValue);
                return array;
            }
        }

        public DecrypterRange Clone()
        {
            DecrypterRange range = new DecrypterRange(startRange, endRange, charOffset);
            range.currentRange = currentRange;

            return range;
        }
    }
}
