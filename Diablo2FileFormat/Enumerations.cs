using System;

namespace Diablo2FileFormat
{
    public enum FileVersion : byte
    {
        V100 = 0x47,
        V107 = 0x57,
        V108 = 0x59,
        V109 = 0x5C,
        V110 = 0x60,
    }

    [Flags]
    public enum CharacterStatus : byte
    {
        None = 0x00,
        Unknown1 = 0x01,
        Unknown2 = 0x02,
        Hardcore = 0x04,
        Died = 0x08,
        Unknown3 = 0x10,
        Expansion = 0x20,
        Ladder = 0x40,
        Unknown5 = 0x80,
    }

    public enum HeroClass : byte
    {
        Amazon,
        Sorceress,
        Necromancer,
        Paladin,
        Barbarian,
        Druid,
        Assassin,
    }

    public enum Difficulty
    {
        Normal,
        Nightmare,
        Hell,
    }

    public enum Act
    {
        Act1,
        Act2,
        Act3,
        Act4,
        Act5,
    };

    public enum Quest
    {
        Quest1,
        Quest2,
        Quest3,
        Quest4,
        Quest5,
        Quest6,
    };
}
