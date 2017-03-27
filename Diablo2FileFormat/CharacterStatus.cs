using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diablo2FileFormat
{
    [Flags]
    public enum CharacterStatus : byte
    {
        None      = 0x00,
        Unknown1  = 0x01,
        Unknown2  = 0x02,
        Hardcore  = 0x04,
        Died      = 0x08,
        Unknown3  = 0x10,
        Expansion = 0x20,
        Ladder    = 0x40,
        Unknown5  = 0x80,
    }
}
