using Diablo2FileFormat;
using Diablo2HeroEditor.Common;
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
            Title = "Diablo 2 Hero Editor";
            Reload();
        }

        public ICommand OpenCommand { get { return new DelegateCommand(OnOpen); } }
        public ICommand SaveCommand { get { return new DelegateCommand(OnSave, () => m_file != null); } }
        public ICommand ReloadCommand { get { return new DelegateCommand(Reload, () => m_file != null); } }

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

        private string m_title;
        public string Title
        {
            get { return m_title; }
            set
            {
                m_title = value;
                RaisePropertyChanged(() => Title);
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
                var file = new Diablo2File(path);
                var validity = file.Load();
                success = validity == FileValidity.Valid;

                if (success)
                {
                    m_file = file;
                    FilePath = path;
                    Title = $"Diablo 2 Hero Editor - {m_file.CharacterData.CharacterName}";
                    Status = "Loaded successfully";

                    Settings.Default.LastLoadedCharacterFile = path;
                    Settings.Default.Save();

                    Mediator.NotifyColleagues(MediatorMessages.CharacterLoaded, m_file);
                }
                else
                {
                    Status = $"Failed to load character file because {FileValidityReson(validity)}.";
                }

                success = true;
            }
            else
            {
                Status = "Failed to load character file because it doesn't exist.";
            }

            return success;
        }

        private string FileValidityReson(FileValidity validity)
        {
            switch (validity)
            {
                case FileValidity.Valid:
                    return "it is valid";
                case FileValidity.NotDiablo2File:
                    return "it is not a diablo 2 file";
                case FileValidity.WrongSize:
                    return "it has the wrong size";
                case FileValidity.WrongVersion:
                    return "it is the wrong version";
                case FileValidity.UnknownError:
                default:
                    return "an unknown error occured";
            }
        }
    }
}
