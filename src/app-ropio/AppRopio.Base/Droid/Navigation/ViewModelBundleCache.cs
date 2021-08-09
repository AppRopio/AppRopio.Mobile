using System;
using MvvmCross.ViewModels;

namespace AppRopio.Base.Droid.Navigation
{
    public class ViewModelBundleCache
    {
        public string Key { get; }
        		
        public IMvxViewModel ViewModel { get; }
		
		public ViewModelBundleCache()
		{
            
		}

        public ViewModelBundleCache(string key, IMvxViewModel vm)
            : this()
        {
            Key = key;
			ViewModel = vm;
		}
    }
}
