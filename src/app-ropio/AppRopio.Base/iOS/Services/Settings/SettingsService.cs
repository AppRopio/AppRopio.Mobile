using System;
using AppRopio.Base.Core.Services.Settings;
using System.IO;
using MvvmCross.Platform;
using MvvmCross.Core.Views;

namespace AppRopio.Base.iOS.Services.Settings
{
    public class SettingsService : ISettingsService
    {
        public byte[] ReadBytesFromFile(string path)
        {
            if (File.Exists(path))
                return File.ReadAllBytes(path);

            throw new FileNotFoundException($"Не найден файл по пути {path}");
        }

        public string ReadStringFromFile(string path)
        {
            if (File.Exists(path))
                return File.ReadAllText(path, System.Text.Encoding.UTF8);
            
            throw new FileNotFoundException($"Не найден файл по пути {path}");
        }
    }
}

