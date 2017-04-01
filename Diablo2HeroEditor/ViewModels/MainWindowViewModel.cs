using Diablo2FileFormat;
using Diablo2HeroEditor.Helpers;

namespace Diablo2HeroEditor.ViewModels
{
    public class MainWindowViewModel : NotificationObject
    {
        public MainWindowViewModel()
        {
            Diablo2File file = new Diablo2File(@"C:\Users\Marc\Saved Games\Diablo II\Zon.d2s");
            file.Load();
            //file.Save();

            CharacterName = file.CharacterName;
            HeroClass = file.HeroClass.ToString();
            Stats = $"Str: {file.GetStatistic(CharacterStatistic.Strength)}, Dex: {file.GetStatistic(CharacterStatistic.Dexterity)}, Vit: {file.GetStatistic(CharacterStatistic.Vitality)}, Ene: {file.GetStatistic(CharacterStatistic.Energy)}";
        }

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
    }
}
