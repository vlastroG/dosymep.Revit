﻿using System;
using System.Collections.Generic;
using System.Reflection;

using Autodesk.Revit.DB;

using dosymep.SimpleServices;

using pyRevitLabs.Json;
using pyRevitLabs.Json.Serialization;

namespace dosymep.Bim4Everyone.SimpleServices {
    internal class JsonSerializationService : ISerializationService {
        private readonly JsonSerializerSettings _settings;

        public JsonSerializationService(JsonSerializerSettings settings, ISerializationBinder serializationBinder) {
            _settings = settings ?? new JsonSerializerSettings();
            _settings.SerializationBinder = serializationBinder;
            _settings.Converters = new List<JsonConverter>() {new ElementIdConverter()};
        }

        public string FileExtension => ".json";

        public string Serialize<T>(T @object) {
            if(@object == null) {
                throw new ArgumentNullException(nameof(@object));
            }

            return JsonConvert.SerializeObject(@object, _settings);
        }

        public T Deserialize<T>(string text) {
            if(string.IsNullOrEmpty(text)) {
                throw new ArgumentException("Value cannot be null or empty.", nameof(text));
            }

            return JsonConvert.DeserializeObject<T>(text, _settings);
        }
    }

    internal class PluginSerializationBinder : ISerializationBinder {
        private readonly IPluginInfoService _pluginInfoService;
        private readonly DefaultSerializationBinder _defaultBinder = new DefaultSerializationBinder();

        public PluginSerializationBinder(IPluginInfoService pluginInfoService) {
            _pluginInfoService = pluginInfoService;
        }

        public Type BindToType(string assemblyName, string typeName) {
            if(_pluginInfoService.PluginAssembly != null
               && assemblyName.Equals(_pluginInfoService.PluginAssembly.GetName().Name)) {
                return _pluginInfoService.PluginAssembly.GetType(typeName);
            } else {
                return _defaultBinder.BindToType(assemblyName, typeName);
            }
        }

        public void BindToName(Type serializedType, out string assemblyName, out string typeName) {
            if(_pluginInfoService.PluginAssembly != null
               && serializedType.Assembly.GetName().Name.Equals(_pluginInfoService.PluginAssembly.GetName().Name)) {
                typeName = serializedType.FullName;
                assemblyName = _pluginInfoService.PluginAssembly.GetName().Name;
            } else {
                _defaultBinder.BindToName(serializedType, out assemblyName, out typeName);
            }
        }
    }

    internal class ElementIdConverter : JsonConverter<ElementId> {
        public override void WriteJson(JsonWriter writer, ElementId value, JsonSerializer serializer) {
#if REVIT_2023_OR_LESS
            writer.WriteValue(value.IntegerValue.ToString());
#else
            writer.WriteValue(value.Value.ToString());
#endif
        }

        public override ElementId ReadJson(
            JsonReader reader,
            Type objectType,
            ElementId existingValue,
            bool hasExistingValue,
            JsonSerializer serializer) {
#if REVIT_2023_OR_LESS
            return new ElementId((int) reader.Value);
#else
            return new ElementId((long) reader.Value);
#endif
        }
    }
}