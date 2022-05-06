﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;

using dosymep.Revit;

using pyRevitLabs.Json;

namespace dosymep.Bim4Everyone.SystemParams {
    /// <summary>
    /// Класс системного параметра.
    /// </summary>
    public class SystemParam : RevitParam {
#if D2020 || R2020 || D2021 || R2021
        /// <summary>
        /// Создает экземпляр класса системного параметра.
        /// </summary>
        /// <param name="languageType">Язык системы.</param>
        /// <param name="id">Наименование свойства системного параметра.</param>
        [JsonConstructor]
        internal SystemParam(LanguageType? languageType, string id) {
            LanguageType = languageType;

            Id = id ?? throw new ArgumentNullException(nameof(id));
            SystemParamId = (BuiltInParameter) Enum.Parse(typeof(BuiltInParameter), id);
        }
        
        /// <summary>
        /// Системное наименование параметра.
        /// </summary>
        [JsonIgnore]
        public BuiltInParameter SystemParamId { get; }

        /// <inheritdoc/>
        [JsonIgnore]
        public override string Name {
            get {
                if(LanguageType.HasValue) {
                    return LabelUtils.GetLabelFor(SystemParamId, LanguageType.Value);
                }

                return LabelUtils.GetLabelFor(SystemParamId);
            }
            set => throw new NotSupportedException(
                $"Для установки имени параметра нужно использовать свойство \"{nameof(SystemParamId)}\".");
        }
#else
        /// <summary>
        /// Создает экземпляр класса системного параметра.
        /// </summary>
        /// <param name="languageType">Язык системы.</param>
        /// <param name="id">Наименование свойства системного параметра.</param>
        [JsonConstructor]
        internal SystemParam(LanguageType? languageType, string id) {
            LanguageType = languageType;

            Id = id ?? throw new ArgumentNullException(nameof(id));
            SystemParamId = (ForgeTypeId) typeof(ParameterTypeId)
                .GetProperty(id, BindingFlags.Public | BindingFlags.Static)
                ?.GetValue(null);
        }

        /// <summary>
        /// Системное наименование параметра.
        /// </summary>
        [JsonIgnore]
        public ForgeTypeId SystemParamId { get; }
        
        /// <inheritdoc/>
        [JsonIgnore]
        public override string Name {
            get {
                if(LanguageType.HasValue) {
                    return LabelUtils.GetLabelForBuiltInParameter(SystemParamId, LanguageType.Value);
                }

                return LabelUtils.GetLabelForBuiltInParameter(SystemParamId);
            }

            set => throw new NotSupportedException(
                $"Для установки имени параметра нужно использовать свойство \"{nameof(SystemParamId)}\".");
        }
#endif
        
        /// <summary>
        /// Язык параметра.
        /// </summary>
        public LanguageType? LanguageType { get; }

        /// <inheritdoc/>
        [JsonIgnore]
        public override string Description => null;
        
        /// <inheritdoc/>
        [JsonIgnore]
        public override StorageType StorageType => SystemParamId.GetStorageType();

        /// <inheritdoc/>
        public override bool IsExistsParam(Document document) {
            return true;
        }

        /// <inheritdoc/>
        public override (Definition Definition, Binding Binding) GetParamBinding(Document document) {
            return document.GetSystemParamBinding(Name);
        }

        /// <inheritdoc/>
        public override ParameterElement GetRevitParamElement(Document document) {
            return null;
        }

        /// <summary>
        /// Проверяет является ли определение параметра внутренним параметром.
        /// </summary>
        /// <param name="document">Документ.</param>
        /// <param name="definition">Определение параметра.</param>
        /// <returns>Возвращает true - если определение параметра является внутренним параметром, иначе false.</returns>
        public override bool IsRevitParam(Document document, Definition definition) {
            if(document is null) {
                throw new ArgumentNullException(nameof(document));
            }

            return base.IsRevitParam(document, definition) && document.IsSystemParamDefinition(definition);
        }

        /// <inheritdoc/>
        public override Parameter GetParam(Element element) {
            return element.GetParam(SystemParamId);
        }
    }
}