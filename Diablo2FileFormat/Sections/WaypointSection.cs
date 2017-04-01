using System;

namespace Diablo2FileFormat.Sections
{
    public class WaypointSection : IDiablo2FileSection
    {
        private const int DataOffset = 8;

        private const int SizePerDifficulty = 27;

        public byte[] Data { get; }
        public bool IsChanged { get; set; }
        public int Size => 81;

        public WaypointSection(byte[] data, int offset)
        {
            Data = new byte[Size];
            Array.Copy(data, offset, Data, 0, Size);
        }

        public void ActivateAllWaypoints()
        {
            for (int difficulty = 0; difficulty < 3; ++difficulty)
            {
                int firstWpOffset = DataOffset + difficulty * SizePerDifficulty + 2;
                Data[firstWpOffset] = 0xFF;
                Data[firstWpOffset + 1] = 0xFF;
                Data[firstWpOffset + 2] = 0xFF;
                Data[firstWpOffset + 3] = 0xFF;
                Data[firstWpOffset + 4] = 0x7F;
            }

            IsChanged = true;
        }
    }
}
