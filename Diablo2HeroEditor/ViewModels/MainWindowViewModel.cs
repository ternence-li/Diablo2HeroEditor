using Diablo2FileFormat;
using Diablo2HeroEditor.Helpers;

namespace Diablo2HeroEditor.ViewModels
{
    public class MainWindowViewModel : NotificationObject
    {
        public MainWindowViewModel()
        {
            Diablo2File file = new Diablo2File(@"C:\Users\Marc\Saved Games\Diablo II\Median.d2s");
            file.Load();

            CharacterName = file.GetCharacterName();
            HeroClass = file.GetClass().ToString();
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
    }
}
