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
        }
    }
}
