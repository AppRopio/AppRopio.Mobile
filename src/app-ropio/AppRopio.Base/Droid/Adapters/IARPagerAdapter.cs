using System;
using System.Collections;
using System.Windows.Input;

namespace AppRopio.Base.Droid.Adapters
{
    public interface IARPagerAdapter : IDisposable
    {
        IEnumerable ItemsSource { get; set; }
        ICommand ItemClick { get; set; }
        int ItemTemplateId { get; set; }

        int GetDataContextPosition(object dataContext);

        System.Object GetRawItem(int position);
    }
}
