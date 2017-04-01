using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diablo2FileFormat.Interfaces
{
    public interface ISkillData
    {
        void SetSkillLevel(int skillNum, byte level);
        void SetAllSkillsLevel(byte level);
    }
}
