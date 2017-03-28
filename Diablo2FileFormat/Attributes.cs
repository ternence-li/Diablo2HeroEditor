using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diablo2FileFormat
{
    public enum Attributes
    {
        Strength = 0,
        Energy,
        Dexterity,
        Vitality,
        StatsLeft,
        SkillsLeft,
        Life,
        MaxLife,
        Mana,
        MaxMana,
        Stamina,
        MaxStamina,
        Level,
        Experience,
        Gold,
        StashGold,

        EndOfAttributes = 0x1FF,
    }

    public class AttributesHelper
    {
        public static int GetBitsPerAttribute(Attributes attribute)
        {
            switch (attribute)
            {
                case Attributes.Strength:
                case Attributes.Energy:
                case Attributes.Dexterity:
                case Attributes.Vitality:
                    return 12;
                case Attributes.StatsLeft:
                    return 11;
                case Attributes.SkillsLeft:
                    return 9;
                case Attributes.Life:
                case Attributes.MaxLife:
                case Attributes.Mana:
                case Attributes.MaxMana:
                case Attributes.Stamina:
                case Attributes.MaxStamina:
                    return 21;
                case Attributes.Level:
                    return 7;
                case Attributes.Experience:
                    return 32;
                case Attributes.Gold:
                case Attributes.StashGold:
                    return 25;
                default:
                    return 0;
            }
        }
    }
}
