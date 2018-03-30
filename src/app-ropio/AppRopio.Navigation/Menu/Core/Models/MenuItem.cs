using System;
using System.Collections.Generic;

namespace AppRopio.Navigation.Menu.Core.Models
{

    public class MenuItem
    {
        /// <summary>
        /// Путь до иконки пункта меню (должен начинаться с "res:", если иконка локальная)
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// Название пункта меню
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Полная информация (включая неймспейс) о типе VM, отображаемой при выборе пункта меню
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Индикатор наличия бейджа у пункта меню
        /// </summary>
        /// <value><c>true</c> если бейдж есть; иначе, <c>false</c>.</value>
        public bool Badge { get; set; }

        /// <summary>
        /// Скрывать ли бейдж, если значение 0
        /// </summary>
        /// <value><c>true</c> if hide badge on null; otherwise, <c>false</c>.</value>
        public bool HideBadgeOnNull { get; set; }

        /// <summary>
        /// Индикатор, показывающий, что VM пункта меню является отображаемым по умолчанию при старте приложения (кроме первого запуска)
        /// </summary>
        /// <value><c>true</c> если нужно выбирать пункт каждый раз при старте; иначе, <c>false</c>.</value>
        public bool Default { get; set; }
    }
}
