using System;
using MvvmCross.Core.ViewModels;

namespace AppRopio.Base.Droid.Services.MultipleViewModelCache
{
    public class ARMultipleViewModelCache : MvvmCross.Droid.Views.IMvxMultipleViewModelCache
    {
        public void Cache(IMvxViewModel toCache, string viewModelTag = "singleInstanceCache")
        {
            
        }

        public IMvxViewModel GetAndClear(Type viewModelType, string viewModelTag = "singleInstanceCache")
        {
            return null;
        }

        public T GetAndClear<T>(string viewModelTag = "singleInstanceCache") where T : IMvxViewModel
        {
            return default(T);
        }
    }
}
