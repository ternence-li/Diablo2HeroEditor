using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diablo2FileFormat.Sections
{
    public class MercenaryItemSection : IDiablo2FileSection
    {
        private int MercenaryMinimumLength = 5; // 2 markers + empty byte at the end

        private byte HeaderMarkerj = 0x6A;
        private byte HeaderMarkerk = 0x6B;
        private byte HeaderMarkerf = 0x66;

        private ItemListSection m_items;

        public byte[] Data
        {
            get
            {
                var data = new byte[Size];

                // Mercenary marker 'jf'
                data[0] = HeaderMarkerj;
                data[1] = HeaderMarkerf;

                if (m_items != null)
                {
                    Array.Copy(m_items.Data, 0, data, 2, m_items.Size);
                }

                // Mercenary end marker 'kf'
                data[Size - 3] = HeaderMarkerk;
                data[Size - 2] = HeaderMarkerf;
                data[Size - 1] = 0;

                return data;
            }
        }

        public bool IsChanged { get; set; }

        public int Size => MercenaryMinimumLength + (m_items?.Size ?? 0);

        public MercenaryItemSection(byte[] data, int offset)
        {
            if (data.Length - offset < MercenaryMinimumLength)
                throw new Exception("Invalid mercenary data");

            if (data[offset + 2] != HeaderMarkerk || data[offset + 3] != HeaderMarkerf)
            {
                // mercenary has items equipped
                m_items = new ItemListSection(data, offset + 2);
            }
        }
    }
}
