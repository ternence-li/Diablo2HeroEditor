using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diablo2FileFormat.Sections
{
    public class SkillSection : IDiablo2FileSection
    {
        public byte[] Data { get; }
        public bool IsChanged { get; set; }
        public int Size => Data.Length;

        public SkillSection(byte[] data, int offset, int size)
        {
            Data = new byte[size];
            Array.Copy(data, offset, Data, 0, size);
        }
    }
}
