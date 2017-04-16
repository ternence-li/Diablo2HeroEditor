using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diablo2FileFormat
{
    public class BitField
    {
        private byte[] m_data;

        public BitField(byte[] data)
        {
            m_data = data;
        }

        public uint Read(int bitPosition, int bitLength)
        {
            int offset = bitPosition / 8;
            int bitOffset = bitPosition % 8;

            return BitOperations.ReadBits(m_data, ref offset, ref bitOffset, bitLength);
        }

        public void Write(uint val, int bitPosition, int bitLength)
        {
            int offset = bitPosition / 8;
            int bitOffset = bitPosition % 8;

            BitOperations.WriteBits(m_data, val, ref offset, ref bitOffset, bitLength);
        }
    }
}
