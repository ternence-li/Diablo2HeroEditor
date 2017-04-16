using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diablo2FileFormat.Interfaces
{
    public interface IItemList
    {
        ushort NumberOfItems { get; }
        List<Diablo2Item> GetItems();
        void Add(Diablo2Item item);
    }
}
