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
    public class ItemsViewModel : NotificationObject
    {
        private Diablo2File m_file;

        public ItemsViewModel()
        {
            Mediator.Register(MediatorMessages.CharacterLoaded, OnCharacterLoaded);
        }

        private void OnCharacterLoaded(object diablo2File)
        {
            m_file = (Diablo2File)diablo2File;
        }
    }
}
