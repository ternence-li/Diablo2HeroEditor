using Diablo2FileFormat.Sections;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diablo2FileFormat
{
    public class Diablo2Item : IDiablo2FileSection
    {
        private byte HeaderMarkerJ = 0x4A;
        private byte HeaderMarkerM = 0x4D;

        private readonly BitField m_bitData;

        public byte[] Data { get; }
        public bool IsChanged { get; set; }
        public int Size => Data.Length;

        public Diablo2Item()
        {
            Data = new byte[14];
            m_bitData = new BitField(Data);
            Data[0] = HeaderMarkerJ;
            Data[1] = HeaderMarkerM;

            IsIdentified = true;
        }

        public Diablo2Item(byte[] data, int offset, int size)
        {
            if (size > 0)
            {
                Data = new byte[size];
                Array.Copy(data, offset, Data, 0, size);
            }
            else
            {
                Data = new byte[0];
            }
            m_bitData = new BitField(Data);
        }

        public static Diablo2Item Load(string filepath)
        {
            var data = File.ReadAllBytes(filepath);
            return new Diablo2Item(data, 0, data.Length);
        }

        public void Save(string filepath)
        {
            File.WriteAllBytes(filepath, Data);
        }

        public bool IsStored => m_bitData.Read(58, 3) == 0;

        public ItemLocation Location => (ItemLocation)m_bitData.Read(73, 3);

        public uint PositionX => m_bitData.Read(65, 4);
        public uint PositionY => m_bitData.Read(69, 4);

        public string ItemType
        {
            get
            {
                byte[] str = new byte[4];
                str[0] = (byte)m_bitData.Read(76, 8);
                str[1] = (byte)m_bitData.Read(84, 8);
                str[2] = (byte)m_bitData.Read(92, 8);
                str[3] = (byte)m_bitData.Read(100, 8);

                return Encoding.ASCII.GetString(str);
            }
        }

        public bool IsIdentified
        {
            get { return m_bitData.Read(20, 1) == 1; }
            set { m_bitData.Write((uint)(value ? 1 : 0), 20, 1); }
        }
    }
}
