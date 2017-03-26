using System;
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

        protected string FilePath { get; }

        protected int FileSize { get; set; }

        protected bool FileChanged { get; set; }

        protected virtual int ChecksumOffset => 0x0C;
        protected virtual int CharacterNameOffset => 0x14;
        protected virtual int ClassOffset => 0x28;

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

        public string GetCharacterName()
        {
           return Encoding.ASCII.GetString(m_fileData, CharacterNameOffset, 16).Trim(new [] { '\0' });
        }

        public Classes GetClass()
        {
            return (Classes)m_fileData[ClassOffset];
        }
    }
}
