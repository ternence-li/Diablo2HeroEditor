using Diablo2FileFormat.Interfaces;
using System;

namespace Diablo2FileFormat.Sections
{
    public class SkillSection : IDiablo2FileSection, ISkillData
    {
        public byte[] Data { get; }
        public bool IsChanged { get; set; }
        public int Size => Data.Length;

        public SkillSection(byte[] data, int offset, int size)
        {
            Data = new byte[size];
            Array.Copy(data, offset, Data, 0, size);
        }

        public void SetSkillLevel(int skillNum, byte level)
        {
            if (skillNum >= 0 && skillNum < Data.Length)
            {
                Data[skillNum + 2] = level;
                IsChanged = true;
            }
        }

        public void SetAllSkillsLevel(byte level)
        {
            for (int i = 0; i < Data.Length - 2; ++i)
            {
                SetSkillLevel(i, level);
            }
        }
    }
}
