using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diablo2FileFormat
{
    public class Diablo2File
    {
        protected byte[] m_fileData;

        protected readonly Dictionary<Attributes, uint> m_attributes = new Dictionary<Attributes, uint>();

        protected string FilePath { get; }

        protected int FileSize { get; set; }

        protected bool FileChanged { get; set; }

        protected virtual uint Diablo2FileSignature => 0xAA55AA55;

        protected virtual int VersionOffset => 0x04;
        protected virtual int FileSizeOffset => 0x08;
        protected virtual int ChecksumOffset => 0x0C;
        protected virtual int CharacterNameOffset => 0x14;
        protected virtual int CharacterStatusOffset => 0x24;
        protected virtual int CharacterProgressionOffset => 0x25;
        protected virtual int ClassOffset => 0x28;
        protected virtual int UknownInventoryOffset => 0x2A;

        protected virtual int AttributesOffset => 0x2FF;

        public Diablo2File(string filePath)
        {
            FilePath = filePath;
        }

        public void Load()
        {
            try
            {
                m_fileData = File.ReadAllBytes(FilePath);
                FileSize = m_fileData.Length;
                FileChanged = false;

                ValidateData();

                ReadStats();
            }
            catch (Exception)
            {
            }
        }

        public void Save()
        {
            if (FileChanged)
            {
                Checksum.UpdateChecksum(m_fileData, ChecksumOffset);
                File.WriteAllBytes(FilePath, m_fileData);
            }
        }

        protected virtual bool ValidateData()
        {
            // Validate file size
            if (m_fileData.Length < FileSizeOffset + 2 || BitConverter.ToUInt16(m_fileData, FileSizeOffset) != m_fileData.Length)
                return false;

            // Check for the D2S signature
            if (BitConverter.ToUInt32(m_fileData, 0) != Diablo2FileSignature)
                return false;

            // Support only file version 1.14
            if ((FileVersion)BitConverter.ToUInt32(m_fileData, VersionOffset) != FileVersion.V114)
                return false;

            return true;
        }

        public string GetCharacterName()
        {
           return Encoding.ASCII.GetString(m_fileData, CharacterNameOffset, 16).Trim(new [] { '\0' });
        }

        public Classes GetClass()
        {
            return (Classes)m_fileData[ClassOffset];
        }

        public int GetCharacterProgression()
        {
            return m_fileData[CharacterProgressionOffset];
        }

        private void ReadStats()
        {
            // TODO add more validation.

            int i = AttributesOffset;
            int bitOffset = 0;
            while (m_fileData[i] != 0x69 || m_fileData[i + 1] != 0x66)
            {
                var att = (Attributes)BitReader.ReadBits(m_fileData, ref i, ref bitOffset, 9);

                if (att != Attributes.EndOfAttributes)
                {
                    var val = BitReader.ReadBits(m_fileData, ref i, ref bitOffset, AttributesHelper.GetBitsPerAttribute(att));

                    m_attributes[att] = val;
                }
                else
                {
                    ++i;
                    bitOffset = 0;
                }
            }
        }

        public uint GetAttribute(Attributes attribute)
        {
            uint val = 0;

            m_attributes.TryGetValue(attribute, out val);

            return val;
        }
    }
}
