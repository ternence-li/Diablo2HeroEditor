using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diablo2FileFormat.Sections
{
    public class ItemSection : IDiablo2FileSection
    {
        public byte[] Data { get; }
        public bool IsChanged { get; set; }
        public int Size => Data.Length;

        public ItemSection(byte[] data, int offset)
        {
            int size = data.Length - offset;

            if (size > 0)
            {
                Data = new byte[size];
                Array.Copy(data, offset, Data, 0, size);
            }
            else
            {
                Data = new byte[0];
            }
        }
    }
}
