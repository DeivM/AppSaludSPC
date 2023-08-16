using System;
using System.IO;

namespace App.Models
{
    public static class FromFile
    {
        public static byte[] ToArray(Stream input)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                input.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }
    }
}
