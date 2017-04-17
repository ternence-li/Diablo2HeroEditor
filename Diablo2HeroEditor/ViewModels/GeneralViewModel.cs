using Diablo2FileFormat;
using Diablo2HeroEditor.Common;
using Diablo2HeroEditor.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        private string m_heroClass;
        public string HeroClass
        {
            get { return m_heroClass; }
            set
            {
                m_heroClass = value;
                RaisePropertyChanged(() => HeroClass);
            }
        }

        private string m_stats;
        public string Stats
        {
            get { return m_stats; }
            set
            {
                m_stats = value;
                RaisePropertyChanged(() => Stats);
            }
        }

        public GeneralViewModel()
        {
            Mediator.Register(MediatorMessages.CharacterLoaded, OnCharacterLoaded);
        }

        private void OnCharacterLoaded(object diablo2File)
        {
            m_file = (Diablo2File)diablo2File;

            CharacterName = m_file.CharacterName;
            HeroClass = m_file.HeroClass.ToString();
            Stats = $"Str: {m_file.GetStatistic(CharacterStatistic.Strength)}, Dex: {m_file.GetStatistic(CharacterStatistic.Dexterity)}, Vit: {m_file.GetStatistic(CharacterStatistic.Vitality)}, Ene: {m_file.GetStatistic(CharacterStatistic.Energy)}";
        }
    }
}
