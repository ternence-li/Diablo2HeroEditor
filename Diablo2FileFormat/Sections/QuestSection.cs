using System;

namespace Diablo2FileFormat.Sections
{
    public class QuestSection : IDiablo2FileSection
    {
        public byte[] Data { get; }
        public bool IsChanged { get; set; }
        public int Size => 298;

        public QuestSection(byte[] data, int offset)
        {
            Data = new byte[Size];
            Array.Copy(data, offset, Data, 0, Size);
        }
    }
}
