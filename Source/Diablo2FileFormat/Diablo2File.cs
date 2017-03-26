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
        protected byte[] m_sourceBytes;

        protected string FilePath { get; }

        protected int FileSize { get; set; }

        protected bool FileChanged { get; set; }

        protected virtual int ChecksumOffset => 0x0C;

        public Diablo2File(string filePath)
        {
            FilePath = filePath;
        }

        public void Load()
        {
            try
            {
                m_sourceBytes = File.ReadAllBytes(FilePath);
                FileSize = m_sourceBytes.Length;
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
                Checksum.UpdateChecksum(m_sourceBytes, ChecksumOffset);
                File.WriteAllBytes(FilePath, m_sourceBytes);
            }
        }
    }
}
