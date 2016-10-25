using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISInfo
{
    class ByteToHex
    {
        public static string ByteArrayToStringFastest(byte[] arr)
        {
            if (arr == null)
                return "";

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < arr.Length ; i++)
            {
                sb.Append(arr[i].ToString("X2"));
            }

            return sb.ToString();
        }


    }
}
