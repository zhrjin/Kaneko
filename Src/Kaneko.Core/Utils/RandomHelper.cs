using System;
namespace Kaneko.Core.Utils
{
    public class RandomHelper
    {
        public static int GetRandomNumber(int min, int max)
        {
            byte[] buffer = Guid.NewGuid().ToByteArray();
            int iSeed = BitConverter.ToInt32(buffer, 0);
            Random r = new Random(iSeed);
            int rtn = r.Next(min, max + 1);
            return rtn;
        }
    }
}
