using Diablo2FileFormat;
using Diablo2HeroEditor.Common;
using Diablo2HeroEditor.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Diablo2HeroEditor.ViewModels
{
    public class RectItem
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
    }

    public class ItemCanvasViewModel : NotificationObject
    {
        public BitmapImage BackgroundImage
        {
            get; set;
        }

        public ItemLocation Location { get; set; }

        private Diablo2File m_file;

        public ObservableCollection<RectItem> RectItems { get; set; }

        private int BaseOffsetX { get; set; }
        private int BaseOffsetY { get; set; }

        private const int GridSize = 45;

        public ItemCanvasViewModel()
        {
            Mediator.Register(MediatorMessages.CharacterLoaded, OnCharacterLoaded);
            RectItems = new ObservableCollection<RectItem>();
        }

        private void OnCharacterLoaded(object diablo2File)
        {
            m_file = (Diablo2File)diablo2File;

            int offsetX = 0;
            int offsetY = 0;
            switch (Location)
            {
                case ItemLocation.Inventory:
                    offsetX = 25;
                    offsetY = 235;
                    break;
                case ItemLocation.Stash:
                    offsetX = 22;
                    offsetY = 129;
                    break;
                case ItemLocation.Cube:
                    offsetX = 22;
                    offsetY = 24;
                    break;
            }

            var items = m_file.Items.GetItems();
            foreach (var item in items.Where(i => i.Location == Location))
            {
                RectItems.Add(new RectItem { X = (item.PositionX * GridSize) + offsetX, Y = (item.PositionY * GridSize) + offsetY, Width = GridSize, Height = GridSize });
            }
        }
    }
}
