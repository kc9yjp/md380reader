using System;
using System.Collections.Generic;
using System.Text;

namespace MD380FileIO
{
    internal static class SubArrayExt
    {
        internal static T[] SubArray<T>(this T[] data, int index, int length)
        {
            T[] result = new T[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }

        internal static byte[] SubArray(this byte[] data, int index, int length)
        {
            return SubArray<byte>(data, index, length);
        }
    }
}
