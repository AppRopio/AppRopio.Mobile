using System;
using UIKit;

namespace AppRopio.Base.iOS.UIExtentions
{
	public static class Device
	{
		public static bool IsPhone
		{
			get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
		}

		public static bool IsPad
		{
			get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad; }
		}

        public static float If_Retina(this float self, float retina)
        {
            return (UIScreen.MainScreen.Scale == 2 ? retina : self);
        }

        public static string SystemVersion
        {
            get { return UIDevice.CurrentDevice.SystemVersion; }
        }
	}
}

