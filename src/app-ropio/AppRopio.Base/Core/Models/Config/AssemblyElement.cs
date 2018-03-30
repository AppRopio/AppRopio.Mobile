using System;
using System.Collections.Generic;

namespace AppRopio.Base.Core.Models.Config
{

    public class AssemblyElement
    {
        /// <summary>
        /// Название полного типа VM (с указанием неймспейса)
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// Имя сборки, в которой находится VM элемента
        /// </summary>
        public string AssemblyName { get; set; }
    }
    
}
