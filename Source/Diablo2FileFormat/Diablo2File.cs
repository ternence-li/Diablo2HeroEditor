using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diablo2FileFormat
{
    public class Diablo2File
    {
        protected string FilePath { get; }

        public Diablo2File(string filePath)
        {
            FilePath = filePath;
        }
    }
}
