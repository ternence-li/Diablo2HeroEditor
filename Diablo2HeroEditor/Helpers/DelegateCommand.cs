using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Diablo2HeroEditor.Helpers
{
    public class DelegateCommand : ICommand
    {
        private readonly Action m_command;
        private readonly Func<bool> m_canExecute;
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public DelegateCommand(Action command, Func<bool> canExecute = null)
        {
            if (command == null)
                throw new ArgumentNullException();
            m_canExecute = canExecute;
            m_command = command;
        }

        public void Execute(object parameter)
        {
            m_command();
        }

        public bool CanExecute(object parameter)
        {
            return m_canExecute == null || m_canExecute();
        }
    }
}
