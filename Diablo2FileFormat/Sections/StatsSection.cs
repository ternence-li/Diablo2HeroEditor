using Diablo2FileFormat.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diablo2FileFormat.Sections
{
    public class StatsSection : IDiablo2FileSection, IStatisticData
    {
        public byte[] Data { get; private set; }
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
                var stat = (CharacterStatistic)BitOperations.ReadBits(Data, ref i, ref bitOffset, 9);

                if (stat != CharacterStatistic.EndOfAttributes)
                {
                    var val = BitOperations.ReadBits(Data, ref i, ref bitOffset, StatisticsHelper.GetBitsPerStat(stat));

                    m_stats[stat] = val;
                }
                else
                {
                    ++i;
                    bitOffset = 0;
                }
            }
        }

        public void SaveStats()
        {
            var newData = new byte[100];
            newData[0] = 0x67;
            newData[1] = 0x66;

            int offset = 2;
            int bitOffset = 0;
            foreach (CharacterStatistic stat in Enum.GetValues(typeof(CharacterStatistic)))
            {
                uint value;
                if (stat == CharacterStatistic.EndOfAttributes)
                {
                    BitOperations.WriteBits(newData, (uint)stat, ref offset, ref bitOffset, 9);
                    if (bitOffset != 0)
                    {
                        BitOperations.WriteBits(newData, 0xFF, ref offset, ref bitOffset, 8 - bitOffset);
                    }
                }
                else if (m_stats.TryGetValue(stat, out value) && value != 0)
                {
                    BitOperations.WriteBits(newData, (uint)stat, ref offset, ref bitOffset, 9);
                    BitOperations.WriteBits(newData, value, ref offset, ref bitOffset, StatisticsHelper.GetBitsPerStat(stat));
                }
            }

            Data = new byte[offset];
            Array.Copy(newData, Data, offset);
        }

        public uint GetStatistic(CharacterStatistic stat)
        {
            uint val = 0;

            m_stats.TryGetValue(stat, out val);

            return val;
        }

        public void SetStatistic(CharacterStatistic stat, uint value)
        {
            m_stats[stat] = value;
            IsChanged = true;
        }
    }
}
