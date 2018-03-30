using System;
using System.IO;
namespace AppRopio.Base.Core.Services.Settings
{
    public interface ISettingsService
    {
        string ReadStringFromFile(string path);
    }
}

