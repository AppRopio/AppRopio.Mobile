using System;
using AppRopio.Base.iOS.Models.ThemeConfigs;
using UIKit;

namespace AppRopio.Base.iOS.Models.ValueConverters
{
	public class BoolToUIColorParameter
	{
		public UIColor TrueColor { get; set; }

		public UIColor FalseColor { get; set; }
	}
}
