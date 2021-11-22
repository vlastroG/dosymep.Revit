﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Revit.DB;

using dosymep.Bim4Everyone.ProjectParams;
using dosymep.Bim4Everyone.SharedParams;
using dosymep.Revit;

namespace dosymep.Bim4Everyone {
    /// <summary>
    /// Класс расширения элемента
    /// </summary>
    public static class ElementExtensions {
        #region RevitParam

        /// <summary>
        /// Проверяет на существование параметра в элементе.
        /// </summary>
        /// <param name="element">Элемент.</param>
        /// <param name="revitParam">Параметр Revit.</param>
        /// <returns>Возвращает true - если параметр существует, иначе false.</returns>
        public static bool IsExistsParam(this Element element, RevitParam revitParam) {
            return element.GetParamValueOrDefault(revitParam) != default;
        }

        /// <summary>
        /// Возвращает значение параметра либо значение по умолчанию.
        /// </summary>
        /// <param name="element">Элемент.</param>
        /// <param name="revitParam">Параметр Revit.</param>
        /// <param name="default">Значение по умолчанию.</param>
        /// <returns>Возвращает значение параметра либо значение по умолчанию.</returns>
        public static object GetParamValueOrDefault(this Element element, RevitParam revitParam, object @default = default) {
            if(element is null) {
                throw new ArgumentNullException(nameof(element));
            }

            if(revitParam is null) {
                throw new ArgumentNullException(nameof(revitParam));
            }

            try {
                return element.GetParamValue(revitParam) ?? @default;
            } catch(ArgumentException) {
                return @default;
            }
        }

        /// <summary>
        /// Возвращает значение параметра с единицой измерения либо значение по умолчанию.
        /// </summary>
        /// <param name="element">Элемент.</param>
        /// <param name="revitParam">Параметр Revit.</param>
        /// <param name="default">Значение по умолчанию.</param>
        /// <returns>Возвращает значение параметра с единицой измерения либо значение по умолчанию.</returns>
        public static string GetParamValueStringOrDefault(this Element element, RevitParam revitParam, string @default = default) {
            if(element is null) {
                throw new ArgumentNullException(nameof(element));
            }

            if(revitParam is null) {
                throw new ArgumentNullException(nameof(revitParam));
            }

            try {
                return element.GetParamValueString(revitParam) ?? @default;
            } catch(ArgumentException) {
                return @default;
            }
        }

        /// <summary>
        /// Возвращает значение параметра элемента.
        /// </summary>
        /// <param name="element">Элемент.</param>
        /// <param name="revitParam">Параметр Revit.</param>
        /// <returns>Возвращает значение параметра элемента.</returns>
        public static object GetParamValue(this Element element, RevitParam revitParam) {
            if(element is null) {
                throw new ArgumentNullException(nameof(element));
            }

            if(revitParam is null) {
                throw new ArgumentNullException(nameof(revitParam));
            }

            return element.GetParam(revitParam).AsObject();
        }

        /// <summary>
        /// Возвращает значение параметра элемента c единицами измерения.
        /// </summary>
        /// <param name="element">Элемент.</param>
        /// <param name="revitParam">Параметр Revit.</param>
        /// <returns>Возвращает значение параметра элемента c единицами измерения.</returns>
        public static string GetParamValueString(this Element element, RevitParam revitParam) {
            if(element is null) {
                throw new ArgumentNullException(nameof(element));
            }

            if(revitParam is null) {
                throw new ArgumentNullException(nameof(revitParam));
            }

            return element.GetParam(revitParam).AsValueString();
        }

        /// <summary>
        /// Устанавливает значение параметра.
        /// </summary>
        /// <param name="element">Элемент.</param>
        /// <param name="revitParam">Параметр Revit.</param>
        /// <param name="paramValue">Значение общего параметра.</param>
        public static void SetParamValue(this Element element, RevitParam revitParam, double paramValue) {
            if(element is null) {
                throw new ArgumentNullException(nameof(element));
            }

            if(revitParam is null) {
                throw new ArgumentNullException(nameof(revitParam));
            }

            element.GetParam(revitParam).Set(paramValue);
        }

        /// <summary>
        /// Устанавливает значение параметра.
        /// </summary>
        /// <param name="element">Элемент.</param>
        /// <param name="revitParam">Параметр Revit.</param>
        /// <param name="paramValue">Значение общего параметра.</param>
        public static void SetParamValue(this Element element, RevitParam revitParam, int paramValue) {
            if(element is null) {
                throw new ArgumentNullException(nameof(element));
            }

            if(revitParam is null) {
                throw new ArgumentNullException(nameof(revitParam));
            }

            element.GetParam(revitParam).Set(paramValue);
        }

        /// <summary>
        /// Устанавливает значение параметра.
        /// </summary>
        /// <param name="element">Элемент.</param>
        /// <param name="revitParam">Параметр Revit.</param>
        /// <param name="paramValue">Значение общего параметра.</param>
        public static void SetParamValue(this Element element, RevitParam revitParam, string paramValue) {
            if(element is null) {
                throw new ArgumentNullException(nameof(element));
            }

            if(revitParam is null) {
                throw new ArgumentNullException(nameof(revitParam));
            }

            element.GetParam(revitParam).Set(paramValue);
        }

        /// <summary>
        /// Устанавливает значение параметра.
        /// </summary>
        /// <param name="element">Элемент.</param>
        /// <param name="revitParam">Параметр Revit.</param>
        /// <param name="paramValue">Значение общего параметра.</param>
        public static void SetParamValue(this Element element, RevitParam revitParam, ElementId paramValue) {
            if(element is null) {
                throw new ArgumentNullException(nameof(element));
            }

            if(revitParam is null) {
                throw new ArgumentNullException(nameof(revitParam));
            }

            element.GetParam(revitParam).Set(paramValue);
        }

        /// <summary>
        /// Устанавливает значение параметра по значению другого параметра.
        /// </summary>
        /// <param name="element">Элемент.</param>
        /// <param name="leftRevitParam">Параметр в который присваивается значение.</param>
        /// <param name="rightRevitParam">Параметр значение которого присваивается.</param>
        public static void SetParamValue(this Element element, RevitParam leftRevitParam, RevitParam rightRevitParam) {
            if(element is null) {
                throw new ArgumentNullException(nameof(element));
            }

            if(leftRevitParam is null) {
                throw new ArgumentException($"'{nameof(leftRevitParam)}' cannot be null or empty.", nameof(leftRevitParam));
            }

            if(rightRevitParam is null) {
                throw new ArgumentException($"'{nameof(rightRevitParam)}' cannot be null or empty.", nameof(rightRevitParam));
            }

            element.GetParam(leftRevitParam).Set(element.GetParam(rightRevitParam));
        }

        /// <summary>
        /// Удаляет параметр.
        /// </summary>
        /// <param name="element">Элемент.</param>
        /// <param name="revitParam">Параметр Revit.</param>
        /// <returns>Возвращает признак удаления параметра true - если был удален, иначе false.</returns>
        public static bool RemoveParamValue(this Element element, RevitParam revitParam) {
            try {
                element.GetParam(revitParam).RemoveValue();
                return true;
            } catch {
                return false;
            }
        }

        /// <summary>
        /// Возвращает параметр.
        /// </summary>
        /// <param name="element">Элемент.</param>
        /// <param name="revitParam">Параметр Revit.</param>
        /// <returns>Возвращает параметр.</returns>
        public static Parameter GetParam(this Element element, RevitParam revitParam) {
            return revitParam.GetParam(element);
        }

        #endregion

        #region SharedParam

        /// <summary>
        /// Возвращает значение параметра либо значение по умолчанию.
        /// </summary>
        /// <param name="element">Элемент.</param>
        /// <param name="sharedParam">Общий параметр.</param>
        /// <param name="default">Значение по умолчанию.</param>
        /// <returns>Возвращает значение параметра либо значение по умолчанию.</returns>
        public static object GetParamValueOrDefault(this Element element, SharedParam sharedParam, object @default = default) {
            return element.GetParamValueOrDefault(sharedParam.AsRevitParam(), @default);
        }

        /// <summary>
        /// Возвращает значение параметра элемента.
        /// </summary>
        /// <param name="element">Элемент.</param>
        /// <param name="sharedParam">Общий параметр.</param>
        /// <returns>Возвращает значение параметра элемента.</returns>
        public static object GetParamValue(this Element element, SharedParam sharedParam) {
            return element.GetParamValue(sharedParam.AsRevitParam());
        }

        /// <summary>
        /// Устанавливает значение параметра.
        /// </summary>
        /// <param name="element">Элемент.</param>
        /// <param name="sharedParam">Общий параметр.</param>
        /// <param name="paramValue">Значение общего параметра.</param>
        public static void SetParamValue(this Element element, SharedParam sharedParam, double paramValue) {
            element.SetParamValue(sharedParam.AsRevitParam(), paramValue);
        }

        /// <summary>
        /// Устанавливает значение параметра.
        /// </summary>
        /// <param name="element">Элемент.</param>
        /// <param name="sharedParam">Общий параметр.</param>
        /// <param name="paramValue">Значение общего параметра.</param>
        public static void SetParamValue(this Element element, SharedParam sharedParam, int paramValue) {
            element.SetParamValue(sharedParam.AsRevitParam(), paramValue);
        }

        /// <summary>
        /// Устанавливает значение параметра.
        /// </summary>
        /// <param name="element">Элемент.</param>
        /// <param name="sharedParam">Общий параметр.</param>
        /// <param name="paramValue">Значение общего параметра.</param>
        public static void SetParamValue(this Element element, SharedParam sharedParam, string paramValue) {
            element.SetParamValue(sharedParam.AsRevitParam(), paramValue);
        }

        /// <summary>
        /// Устанавливает значение параметра.
        /// </summary>
        /// <param name="element">Элемент.</param>
        /// <param name="sharedParam">Общий параметр.</param>
        /// <param name="paramValue">Значение общего параметра.</param>
        public static void SetParamValue(this Element element, SharedParam sharedParam, ElementId paramValue) {
            element.SetParamValue(sharedParam.AsRevitParam(), paramValue);
        }

        /// <summary>
        /// Возвращает параметр.
        /// </summary>
        /// <param name="element">Элемент.</param>
        /// <param name="sharedParam">Общий параметр.</param>
        /// <returns>Возвращает параметр.</returns>
        public static Parameter GetParam(this Element element, SharedParam sharedParam) {
            return sharedParam.GetParam(element);
        }

        #endregion

        #region ProjectParam

        /// <summary>
        /// Возвращает значение параметра либо значение по умолчанию.
        /// </summary>
        /// <param name="element">Элемент.</param>
        /// <param name="projectParam">Параметр проекта.</param>
        /// <param name="default">Значение по умолчанию.</param>
        /// <returns>Возвращает значение параметра либо значение по умолчанию.</returns>
        public static object GetParamValueOrDefault(this Element element, ProjectParam projectParam, object @default = default) {
            return element.GetParamValueOrDefault(projectParam.AsRevitParam(), @default);
        }

        /// <summary>
        /// Возвращает значение параметра элемента.
        /// </summary>
        /// <param name="element">Элемент.</param>
        /// <param name="projectParam">Параметр проекта.</param>
        /// <returns>Возвращает значение параметра элемента.</returns>
        public static object GetParamValue(this Element element, ProjectParam projectParam) {
            return element.GetParamValue(projectParam.AsRevitParam());
        }

        /// <summary>
        /// Устанавливает значение параметра.
        /// </summary>
        /// <param name="element">Элемент.</param>
        /// <param name="projectParam">Параметр проекта.</param>
        /// <param name="paramValue">Значение общего параметра.</param>
        public static void SetParamValue(this Element element, ProjectParam projectParam, double paramValue) {
            element.SetParamValue(projectParam.AsRevitParam(), paramValue);
        }

        /// <summary>
        /// Устанавливает значение параметра.
        /// </summary>
        /// <param name="element">Элемент.</param>
        /// <param name="projectParam">Параметр проекта.</param>
        /// <param name="paramValue">Значение общего параметра.</param>
        public static void SetParamValue(this Element element, ProjectParam projectParam, int paramValue) {
            element.SetParamValue(projectParam.AsRevitParam(), paramValue);
        }

        /// <summary>
        /// Устанавливает значение параметра.
        /// </summary>
        /// <param name="element">Элемент.</param>
        /// <param name="projectParam">Параметр проекта.</param>
        /// <param name="paramValue">Значение общего параметра.</param>
        public static void SetParamValue(this Element element, ProjectParam projectParam, string paramValue) {
            element.SetParamValue(projectParam.AsRevitParam(), paramValue);
        }

        /// <summary>
        /// Устанавливает значение параметра.
        /// </summary>
        /// <param name="element">Элемент.</param>
        /// <param name="projectParam">Параметр проекта.</param>
        /// <param name="paramValue">Значение общего параметра.</param>
        public static void SetParamValue(this Element element, ProjectParam projectParam, ElementId paramValue) {
            element.SetParamValue(projectParam.AsRevitParam(), paramValue);
        }

        /// <summary>
        /// Возвращает параметр.
        /// </summary>
        /// <param name="element">Элемент.</param>
        /// <param name="projectParam">Параметр проекта.</param>
        /// <returns>Возвращает параметр.</returns>
        public static Parameter GetParam(this Element element, ProjectParam projectParam) {
            return projectParam.GetParam(element);
        }

        #endregion
    }
}
