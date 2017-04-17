using Diablo2FileFormat;
using Diablo2HeroEditor.Common;
using Diablo2HeroEditor.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Diablo2HeroEditor.ViewModels
{
    public class GeneralViewModel : NotificationObject
    {
        private Diablo2File m_file;

        private string m_characterName;
        public string CharacterName
        {
            get { return m_characterName; }
            set
            {
                m_characterName = value;
                RaisePropertyChanged(() => CharacterName);
            }
        }

        private HeroClass m_heroClass;
        public HeroClass HeroClass
        {
            get { return m_heroClass; }
            set
            {
                m_heroClass = value;
                m_file.CharacterData.HeroClass = value;
                RaisePropertyChanged(() => HeroClass);
            }
        }

        public IEnumerable<HeroClass> HeroClasses => Enum.GetValues(typeof(HeroClass)).Cast<HeroClass>();

        private uint m_strength;
        public uint Strength
        {
            get { return m_strength; }
            set
            {
                m_strength = value;
                m_file.Statistics.SetStatistic(CharacterStatistic.Strength, value);
                RaisePropertyChanged(() => Strength);
            }
        }

        private uint m_dexterity;
        public uint Dexterity
        {
            get { return m_dexterity; }
            set
            {
                m_dexterity = value;
                m_file.Statistics.SetStatistic(CharacterStatistic.Dexterity, value);
                RaisePropertyChanged(() => Dexterity);
            }
        }

        private uint m_vitality;
        public uint Vitality
        {
            get { return m_vitality; }
            set
            {
                m_vitality = value;
                m_file.Statistics.SetStatistic(CharacterStatistic.Vitality, value);
                RaisePropertyChanged(() => Vitality);
            }
        }

        private uint m_energy;
        public uint Energy
        {
            get { return m_energy; }
            set
            {
                m_energy = value;
                m_file.Statistics.SetStatistic(CharacterStatistic.Energy, value);
                RaisePropertyChanged(() => Energy);
            }
        }

        private uint m_points;
        public uint Points
        {
            get { return m_points; }
            set
            {
                m_points = value;
                m_file.Statistics.SetStatistic(CharacterStatistic.StatsLeft, value);
                RaisePropertyChanged(() => Points);
            }
        }

        public GeneralViewModel()
        {
            Mediator.Register(MediatorMessages.CharacterLoaded, OnCharacterLoaded);
        }

        private void OnCharacterLoaded(object diablo2File)
        {
            m_file = (Diablo2File)diablo2File;

            CharacterName = m_file.CharacterData.CharacterName;
            HeroClass = m_file.CharacterData.HeroClass;

            Strength = m_file.Statistics.GetStatistic(CharacterStatistic.Strength);
            Dexterity = m_file.Statistics.GetStatistic(CharacterStatistic.Dexterity);
            Vitality = m_file.Statistics.GetStatistic(CharacterStatistic.Vitality);
            Energy = m_file.Statistics.GetStatistic(CharacterStatistic.Energy);
            Points = m_file.Statistics.GetStatistic(CharacterStatistic.StatsLeft);
        }
    }
}
