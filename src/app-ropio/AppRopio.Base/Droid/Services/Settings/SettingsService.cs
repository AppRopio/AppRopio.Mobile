using System;
using AppRopio.Base.Core.Services.Settings;
using System.IO;
using Android.Content.Res;
using Android.App;
using System.Reflection;
using Android.Content;

namespace AppRopio.Base.Droid.Services.Settings
{
    public class SettingsService : ISettingsService
    {
        private Context _context = Application.Context;

        public string ReadStringFromFile(string path)
        {
			using (var asset = _context.Assets.Open(path))
			using (var streamReader = new StreamReader(asset))
			{
                return streamReader.ReadToEnd();
			}
        }
    }
}

