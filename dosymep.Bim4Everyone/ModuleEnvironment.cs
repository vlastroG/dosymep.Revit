﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace dosymep.Bim4Everyone {
    /// <summary>
    /// Класс окружения модуля Bim4Everyone
    /// </summary>
    public class ModuleEnvironment {
        /// <summary>
        /// Статический конструктор
        /// </summary>
        static ModuleEnvironment() {
            CurrentLibraryPath =
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    @"pyRevit\Extensions\BIM4Everyone.lib\dosymep_libs\libs", RevitVersion);
            
            TemplatesPath = Path.Combine(CurrentLibraryPath, "templates");
            EmptyTemplatePath = Path.Combine(TemplatesPath, "empty_project.rte");
            ParametersTemplatePath = Path.Combine(TemplatesPath, "project_parameters.rvt");
        }

        /// <summary>
        /// Текущая версия Revit.
        /// </summary>
#if REVIT_2020
        public static string RevitVersion { get; set; } = "2020";
#elif REVIT_2021
        public static string RevitVersion { get; set; } = "2021";
#elif REVIT_2022
        public static string RevitVersion { get; set; } = "2022";
#elif REVIT_2023
        public static string RevitVersion { get; set; } = "2023";
#elif REVIT_2024
        public static string RevitVersion { get; set; } = "2024";
#else
        public static string RevitVersion => throw new NotImplementedException("Версия Revit не установлена.");
#endif

        /// <summary>
        /// Путь до текущей библиотеки
        /// </summary>
        public static string CurrentLibraryPath { get; }

        #region Templates

        /// <summary>
        /// Путь до папки с шаблонами
        /// </summary>
        public static string TemplatesPath { get; }

        /// <summary>
        /// Путь до пустого шаблона документа
        /// </summary>
        public static string EmptyTemplatePath { get; }

        /// <summary>
        /// Путь до шаблона параметров
        /// </summary>
        public static string ParametersTemplatePath { get; }

        #endregion
    }
}
