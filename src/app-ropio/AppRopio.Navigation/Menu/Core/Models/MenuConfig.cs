using System;
using System.Collections.Generic;
using AppRopio.Base.Core.Models.Config;

namespace AppRopio.Navigation.Menu.Core.Models
{
    public class MenuConfig
    {
        /// <summary>
        /// Полная информация (включая неймспейс) о типе VM, показываемой при первом запуске
        /// </summary>
        public string FirstLaunchType { get; set; }

        /// <summary>
        /// Конфигурация хедера меню
        /// </summary>
        public AssemblyElement Header { get; set; }

        /// <summary>
        /// Список секций, отображаемых в меню
        /// </summary>
        public List<MenuSection> Sections { get; set; }

        /// <summary>
        /// Конфигурация футера меню
        /// </summary>
        public AssemblyElement Footer { get; set; }
    }
}

