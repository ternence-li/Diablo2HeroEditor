using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diablo2FileFormat
{
    public enum CharacterStatistic
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

    public class StatisticsHelper
    {
        public static int GetBitsPerAttribute(CharacterStatistic attribute)
        {
            switch (attribute)
            {
                case CharacterStatistic.Strength:
                case CharacterStatistic.Energy:
                case CharacterStatistic.Dexterity:
                case CharacterStatistic.Vitality:
                    return 12;
                case CharacterStatistic.StatsLeft:
                    return 11;
                case CharacterStatistic.SkillsLeft:
                    return 9;
                case CharacterStatistic.Life:
                case CharacterStatistic.MaxLife:
                case CharacterStatistic.Mana:
                case CharacterStatistic.MaxMana:
                case CharacterStatistic.Stamina:
                case CharacterStatistic.MaxStamina:
                    return 21;
                case CharacterStatistic.Level:
                    return 7;
                case CharacterStatistic.Experience:
                    return 32;
                case CharacterStatistic.Gold:
                case CharacterStatistic.StashGold:
                    return 25;
                default:
                    return 0;
            }
        }
    }
}
