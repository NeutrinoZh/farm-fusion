using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{
    public class GridObjectTypeDictionaryConverter<TValue> 
        : JsonConverter<Dictionary<GameStatistics.GridObjectType, TValue>>
    {
        public override void WriteJson(JsonWriter writer, Dictionary<GameStatistics.GridObjectType, TValue> value, JsonSerializer serializer)
        {
            writer.WriteStartArray();
            foreach (var kv in value)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("Key");
                serializer.Serialize(writer, kv.Key);
                writer.WritePropertyName("Value");
                serializer.Serialize(writer, kv.Value);
                writer.WriteEndObject();
            }
            writer.WriteEndArray();
        }

        public override Dictionary<GameStatistics.GridObjectType, TValue> ReadJson(
            JsonReader reader, Type objectType, 
            Dictionary<GameStatistics.GridObjectType, TValue> existingValue, 
            bool hasExistingValue, JsonSerializer serializer)
        {
            var list = serializer.Deserialize<List<KeyValuePair>>(reader);
            return list?.ToDictionary(x => x.Key, x => x.Value) 
                   ?? new Dictionary<GameStatistics.GridObjectType, TValue>();
        }

        class KeyValuePair
        {
            public GameStatistics.GridObjectType Key { get; set; }
            public TValue Value { get; set; }
        }
    }
}