using Diablo2FileFormat.Interfaces;
using System;

namespace Diablo2FileFormat.Sections
{
    public class QuestSection : IDiablo2FileSection, IQuestData
    {
        public byte[] Data { get; }
        public bool IsChanged { get; set; }
        public int Size => 298;

        public QuestSection(byte[] data, int offset)
        {
            Data = new byte[Size];
            Array.Copy(data, offset, Data, 0, Size);
        }

        private int GetQuestOffset(Difficulty difficulty, Act act, Quest quest)
        {
            int offset = -1;

            if (act != Act.Act4 || quest < Quest.Quest4)
            {
                offset = 12; // 10 bytes for the quest header, 2 bytes for the act introduction

                offset += (int)difficulty * 96; // choose to the right difficulty
                offset += (int)act * 16;        // choose to the right act
                offset += (int)quest * 2;       // choose the right quest

                if (act == Act.Act5)
                {
                    offset += 4; // there are additional bytes in act 4
                }
            }

            return offset;
        }

        public void ChangeQuest(Difficulty difficulty, Act act, Quest quest, bool complete)
        {
            int offset = GetQuestOffset(difficulty, act, quest);

            if (offset == -1) return;

            if (complete)
            {
                Data[offset] = 0x01;     // Quest complete
                Data[offset + 1] = 0x10; // Quest log animation viewed

                if (act == Act.Act5 && quest == Quest.Quest3)
                {
                    // Scroll of resist
                    Data[offset] += 0xC0;
                }
            }
            else
            {
                Data[offset] = 0;
                Data[offset + 1] = 0;
            }

            // Allow travel to the next act.
            // For Act4, the diablo quest is quest2
            if (complete && (quest == Quest.Quest6 || (act == Act.Act4 && quest == Quest.Quest2)))
            {
                if (act != Act.Act4)
                    Data[offset + 2] = 1;
                else
                    Data[offset + 4] = 1;
            }

            IsChanged = true;
        }

        public void ChangeQuests(Difficulty difficulty, Act act, bool complete)
        {
            ChangeQuest(difficulty, act, Quest.Quest1, complete);
            ChangeQuest(difficulty, act, Quest.Quest2, complete);
            ChangeQuest(difficulty, act, Quest.Quest3, complete);
            if (act != Act.Act4)
            {
                ChangeQuest(difficulty, act, Quest.Quest4, complete);
                ChangeQuest(difficulty, act, Quest.Quest5, complete);
                ChangeQuest(difficulty, act, Quest.Quest6, complete);
            }
        }

        public void ChangeQuests(Difficulty difficulty, bool complete)
        {
            ChangeQuests(difficulty, Act.Act1, complete);
            ChangeQuests(difficulty, Act.Act2, complete);
            ChangeQuests(difficulty, Act.Act3, complete);
            ChangeQuests(difficulty, Act.Act4, complete);
            ChangeQuests(difficulty, Act.Act5, complete);
        }

        public void ChangeQuests(bool complete)
        {
            ChangeQuests(Difficulty.Normal, complete);
            ChangeQuests(Difficulty.Nightmare, complete);
            ChangeQuests(Difficulty.Hell, complete);
        }
    }
}
