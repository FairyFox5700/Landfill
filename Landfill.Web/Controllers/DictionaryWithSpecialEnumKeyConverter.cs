using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Landfill.Web.Controllers
{
    public partial class ContentController
    {
        public class DictionaryWithSpecialEnumKeyConverter<TKey> : JsonConverter
        {
            public override bool CanWrite
            {
                get { return false; }
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                throw new NotSupportedException();
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                if (reader.TokenType == JsonToken.Null)
                    return null;

                var valueType = objectType.GetGenericArguments()[1];
                var intermediateDictionaryType = typeof(Dictionary<,>).MakeGenericType(typeof(string), valueType);
                var intermediateDictionary = (IDictionary)Activator.CreateInstance(intermediateDictionaryType);
                serializer.Populate(reader, intermediateDictionary);

                var finalDictionary = (IDictionary)Activator.CreateInstance(objectType);
                foreach (DictionaryEntry pair in intermediateDictionary)
                    finalDictionary.Add(ToEnum<TKey>(pair.Key.ToString()), pair.Value);

                return finalDictionary;
            }

            private T ToEnum<T>(string str)
            {
                var enumType = typeof(T);
                foreach (var name in Enum.GetNames(enumType))
                {
                    var enumMemberAttribute = ((EnumMemberAttribute[])enumType.GetField(name).GetCustomAttributes(typeof(EnumMemberAttribute), true)).Single();
                    if (enumMemberAttribute.Value == str) return (T)Enum.Parse(enumType, name);
                }
                return default(T);
            }

            public override bool CanConvert(Type objectType)
            {
                return true;
            }
        }
























    }
    }



