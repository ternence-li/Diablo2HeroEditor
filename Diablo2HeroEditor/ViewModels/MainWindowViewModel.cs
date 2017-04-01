using Diablo2FileFormat;
using Diablo2HeroEditor.Helpers;
using System.Configuration;
using System.IO;
using System.Windows.Input;

namespace Diablo2HeroEditor.ViewModels
{
    public class MainWindowViewModel : NotificationObject
    {
        Diablo2File m_file;

        public MainWindowViewModel()
        {
            
        }

        public ICommand OpenCommand { get { return new DelegateCommand(OnOpen); } }
        public ICommand SaveCommand { get { return new DelegateCommand(OnSave); } }

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

        private string m_filePath;
        public string FilePath
        {
            get { return m_filePath; }
            set
            {
                m_filePath = value;
                RaisePropertyChanged(() => FilePath);
            }
        }

        private string m_status;
        public string Status
        {
            get { return m_status; }
            set
            {
                m_status = value;
                RaisePropertyChanged(() => Status);
            }
        }

        public void OnOpen()
        {
            string savefile = ConfigurationManager.AppSettings["savefile"];

            if (!string.IsNullOrEmpty(savefile) && File.Exists(savefile))
            {
                Status = "Loaded";
                FilePath = savefile;
                m_file = new Diablo2File(savefile);
                m_file.Load();

                CharacterName = m_file.CharacterName;
                HeroClass = m_file.HeroClass.ToString();
                Stats = $"Str: {m_file.GetStatistic(CharacterStatistic.Strength)}, Dex: {m_file.GetStatistic(CharacterStatistic.Dexterity)}, Vit: {m_file.GetStatistic(CharacterStatistic.Vitality)}, Ene: {m_file.GetStatistic(CharacterStatistic.Energy)}";
            }
        }

        public void OnSave()
        {
            m_file?.Save();

            Status = "Saved";
        }
    }
}
