using Diablo2FileFormat;
using Diablo2HeroEditor.Helpers;
using Diablo2HeroEditor.Properties;
using Microsoft.Win32;
using System.IO;
using System.Windows.Input;

namespace Diablo2HeroEditor.ViewModels
{
    public class MainWindowViewModel : NotificationObject
    {
        Diablo2File m_file;

        public MainWindowViewModel()
        {
            Reload();
        }

        public ICommand OpenCommand { get { return new DelegateCommand(OnOpen); } }
        public ICommand SaveCommand { get { return new DelegateCommand(OnSave, () => m_file != null); } }
        public ICommand ReloadCommand { get { return new DelegateCommand(Reload, () => m_file != null); } }

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
            var dlg = new OpenFileDialog();
            dlg.Filter = "Diablo 2 save file|*.d2s";
            if (dlg.ShowDialog() == true)
            {
                LoadCharacterFile(dlg.FileName);
            }
        }

        public void OnSave()
        {
            m_file?.Save();

            Status = "Saved";
        }

        public void Reload()
        {
            LoadCharacterFile(Settings.Default.LastLoadedCharacterFile);
        }

        private bool LoadCharacterFile(string path)
        {
            bool success = false;
            if (string.IsNullOrEmpty(path))
            {
                Status = "Click Open to load a character.";
            }
            else if (File.Exists(path))
            {
                Status = "Loaded";
                FilePath = path;
                m_file = new Diablo2File(path);
                m_file.Load();

                CharacterName = m_file.CharacterName;
                HeroClass = m_file.HeroClass.ToString();
                Stats = $"Str: {m_file.GetStatistic(CharacterStatistic.Strength)}, Dex: {m_file.GetStatistic(CharacterStatistic.Dexterity)}, Vit: {m_file.GetStatistic(CharacterStatistic.Vitality)}, Ene: {m_file.GetStatistic(CharacterStatistic.Energy)}";

                Settings.Default.LastLoadedCharacterFile = path;
                Settings.Default.Save();

                success = true;
            }
            else
            {
                Status = "Failed to load character file because it doesn't exist.";
            }

            return success;
        }
    }
}
