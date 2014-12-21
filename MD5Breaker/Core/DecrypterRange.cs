using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MD5Breaker.Core
{   
    [Serializable()]
    public class DecrypterRange
    {
        public uint charCount { get; private set; }

        public uint[] startRange { get; set; }
        public uint[] endRange { get; set; }
        public uint[] currentRange { get; set; }

        public DecrypterRange(uint[] startRange, uint[] endRange, uint charCount)
        {
            this.charCount = charCount;
            this.startRange = startRange;
            this.endRange = endRange;

            if (currentRange == null)
                currentRange = (uint[])startRange.Clone();
        }

        public DecrypterRange(ulong blockid, ulong blocksize, uint charCount)
        {
            this.charCount = charCount;

            this.startRange = RecursivePlus(blockid * blocksize, new uint[] { 0 });
            this.endRange = RecursivePlus(blockid * blocksize + blocksize, new uint[] { 0 });

            this.currentRange = (uint[]) startRange.Clone();
        }

        public ulong GetNumber()
        {
            return DecrypterRange.GetNumber(endRange);
        }

        public ulong GetCurrentNumber()
        {
            return DecrypterRange.GetNumber(currentRange);
        }

        public ulong GetCombinationsAmount()
        {
            ulong startCombinations = 1;
            ulong endCombinations = 1;

            foreach (uint value in startRange)
                startCombinations *= value;

            foreach (uint value in endRange)
                startCombinations *= value;

            return endCombinations - startCombinations;
        }

        public static ulong GetNumber(uint[] range)
        {
            ulong length = 1;

            foreach (ulong value in range)
                length *= value;

            return length;
        }

        public ulong Next()
        {
            uint[] current = currentRange;
            int i;

            for (i = currentRange.Length - 1; i >= 0; i--)
            {
                currentRange[i]++;

                if (currentRange[i] < charCount)
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

            return DecrypterRange.GetNumber(currentRange);
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

                if (result[i] < charCount)
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
            ulong plusNext = (value / charCount);
            ulong lastValue = array[array.Length - 1] + (value - (plusNext * charCount));

            if (plusNext > 0)
            {
                uint[] nArray;
                if (plusNext > charCount && array.Length == 1)
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
            DecrypterRange range = new DecrypterRange(startRange, endRange, charCount);
            range.currentRange = currentRange;

            return range;
        }
    }
}
