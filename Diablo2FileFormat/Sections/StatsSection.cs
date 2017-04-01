using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diablo2FileFormat.Sections
{
    public class StatsSection : IDiablo2FileSection
    {
        public byte[] Data { get; }
        public bool IsChanged { get; set; }
        public int Size => Data.Length;

        protected readonly Dictionary<CharacterStatistic, uint> m_stats = new Dictionary<CharacterStatistic, uint>();

        public StatsSection(byte[] data, int offset)
        {
            int skillsTagOffset = offset;
            while (skillsTagOffset < data.Length && (data[skillsTagOffset] != 0x69 || data[skillsTagOffset + 1] != 0x66))
            {
                ++skillsTagOffset;
            }

            if (skillsTagOffset < data.Length)
            {
                int size = skillsTagOffset - offset;
                Data = new byte[size];
                Array.Copy(data, offset, Data, 0, size);

                ReadStats();
            }
            else
            {
                Data = new byte[0];
            }
        }

        private void ReadStats()
        {
            int i = 2; // Start at 2 to skip section delimiter
            int bitOffset = 0;
            while (i < Data.Length)
            {
                var stat = (CharacterStatistic)BitReader.ReadBits(Data, ref i, ref bitOffset, 9);

                if (stat != CharacterStatistic.EndOfAttributes)
                {
                    var val = BitReader.ReadBits(Data, ref i, ref bitOffset, StatisticsHelper.GetBitsPerAttribute(stat));

                    m_stats[stat] = val;
                }
                else
                {
                    ++i;
                    bitOffset = 0;
                }
            }
        }

        public uint GetStatistic(CharacterStatistic stat)
        {
            uint val = 0;

            m_stats.TryGetValue(stat, out val);

            return val;
        }
    }
}
