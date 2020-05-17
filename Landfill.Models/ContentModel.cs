using Landfill.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.Json;
using static Landfill.Common.Enums.EnumsContainer;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace Landfill.Models
{
    public class ContentDto                            
    {
        public int Id { get; set; }
        public JObject Content { get; set; }//
        //[JsonProperty, JsonConverter(typeof(DictionaryWithSpecialEnumKeyConverter<Language>))]
        public IDictionary<string,object> Translations { get; set; }
        //public ODataNamedValueDictionary< TranslationDTO> Translations { get; set; }
        //map from EF to this model->client
        //IQuerable sticks to model not to entity
        //map ODATA query to entity


    }

    public class TranslationDTO
    {
        public string ContentText { get; set; }
    }

   
}
