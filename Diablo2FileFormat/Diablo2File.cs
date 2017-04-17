using Diablo2FileFormat.Interfaces;
using Diablo2FileFormat.Sections;
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
        private const int FixedLengthSectionSize = 765;

        protected byte[] m_fileData;

        protected readonly List<IDiablo2FileSection> m_sections = new List<IDiablo2FileSection>();
        protected HeaderSection m_headerSection;
        protected QuestSection m_questSection;
        protected WaypointSection m_waypointSection;
        protected NpcSection m_npcSection;
        protected StatsSection m_statsSection;
        protected SkillSection m_skillSection;
        protected ItemListSection m_itemSection;
        protected ItemListSection m_corpseSection;
        protected MercenaryItemSection m_mercenarySection;

        protected readonly Dictionary<CharacterStatistic, uint> m_attributes = new Dictionary<CharacterStatistic, uint>();

        protected string FilePath { get; }

        protected bool FileChanged => m_sections.Any(s => s.IsChanged);

        public IBasicCharacterData CharacterData => m_headerSection;
        public IQuestData QuestData => m_questSection;
        public IStatisticData Statistics => m_statsSection;
        public ISkillData Skills => m_skillSection;
        public IItemList Items => m_itemSection;

        protected virtual uint Diablo2FileSignature => 0xAA55AA55;

        protected virtual int VersionOffset => 0x04;
        protected virtual int FileSizeOffset => 0x08;
        protected virtual int ChecksumOffset => 0x0C;

        public Diablo2File(string filePath)
        {
            FilePath = filePath;
        }

        public void Load()
        {
            try
            {
                m_fileData = File.ReadAllBytes(FilePath);

                LoadFileSections();

                ValidateData();
            }
            catch (Exception)
            {
            }
        }

        private void LoadFileSections()
        {
            m_sections.Clear();

            int offset = 0;
            m_headerSection = new HeaderSection(m_fileData);
            m_sections.Add(m_headerSection);
            offset += m_headerSection.Size;

            m_questSection = new QuestSection(m_fileData, offset);
            m_sections.Add(m_questSection);
            offset += m_questSection.Size;

            m_waypointSection = new WaypointSection(m_fileData, offset);
            m_sections.Add(m_waypointSection);
            offset += m_waypointSection.Size;

            m_npcSection = new NpcSection(m_fileData, offset);
            m_sections.Add(m_npcSection);
            offset += m_npcSection.Size;

            m_statsSection = new StatsSection(m_fileData, offset);
            m_sections.Add(m_statsSection);
            offset += m_statsSection.Size;

            m_skillSection = new SkillSection(m_fileData, offset, m_headerSection.SkillSectionLength);
            m_sections.Add(m_skillSection);
            offset += m_skillSection.Size;

            m_itemSection = new ItemListSection(m_fileData, offset);
            m_sections.Add(m_itemSection);
            offset += m_itemSection.Size;

            m_corpseSection = new ItemListSection(m_fileData, offset);
            m_sections.Add(m_corpseSection);
            offset += m_corpseSection.Size;

            m_mercenarySection = new MercenaryItemSection(m_fileData, offset);
            m_sections.Add(m_mercenarySection);
            offset += m_mercenarySection.Size;

            if (offset != m_fileData.Length)
                throw new Exception("Failed to parse character file.");
        }

        public void Save()
        {
            if (FileChanged)
            {
                if (m_statsSection.IsChanged)
                {
                    m_statsSection.SaveStats();
                }

                m_headerSection.FileSize = m_sections.Sum(s => s.Size);

                m_fileData = new byte[m_headerSection.FileSize];

                int offset = 0;
                foreach (var section in m_sections)
                {
                    Array.Copy(section.Data, 0, m_fileData, offset, section.Size);
                    offset += section.Size;
                }

                Checksum.UpdateChecksum(m_fileData, ChecksumOffset);

                File.WriteAllBytes(FilePath, m_fileData);

                m_sections.ForEach(s => s.IsChanged = false);
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

            var version = (FileVersion)BitConverter.ToUInt32(m_fileData, VersionOffset);
            // Support only file version 1.14
            if (version != FileVersion.V114 || version != FileVersion.V109)
                return false;

            return true;
        }

        //public uint GetStatistic(CharacterStatistic stat)
        //{
        //    return m_statsSection.GetStatistic(stat);
        //}

        //public void SetStatistic(CharacterStatistic stat, uint value)
        //{
        //    m_statsSection.SetStatistic(stat, value);
        //}

        public void ActivateAllWaypoints()
        {
            m_waypointSection.ActivateAllWaypoints();
        }

        public void SetCharacterProgression(Difficulty difficulty, Act act)
        {
            m_headerSection.SetCharacterProgression(difficulty, act);
        }

        public void SetCharacterLevel(int level)
        {
            int exp = 0;
            if (level > 1)
            {
                exp = 1000;
                for (int i = 2; i < level; ++i)
                {
                    exp += (i - 1) * 1000;
                }
            }

            CharacterData.Level = level;
            Statistics.SetStatistic(CharacterStatistic.Level, (uint)level);
            Statistics.SetStatistic(CharacterStatistic.Experience, (uint)exp);
        }

        public void PimpCharacter()
        {
            SetCharacterLevel(120);
            SetCharacterProgression(Difficulty.Hell, Act.Act5);

            Skills.SetAllSkillsLevel(120);

            QuestData.ChangeQuests(true);
            ActivateAllWaypoints();

            Statistics.SetStatistic(CharacterStatistic.Strength, 4095);
            Statistics.SetStatistic(CharacterStatistic.Dexterity, 4095);
            Statistics.SetStatistic(CharacterStatistic.Vitality, 4095);
            Statistics.SetStatistic(CharacterStatistic.Energy, 4095);
            Statistics.SetStatistic(CharacterStatistic.StatsLeft, 4095);
            Statistics.SetStatistic(CharacterStatistic.MaxLife, 2097151);
            Statistics.SetStatistic(CharacterStatistic.MaxMana, 2097151);
        }
    }
}
