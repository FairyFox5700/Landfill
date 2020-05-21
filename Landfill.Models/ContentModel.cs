using Landfill.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
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
     
        public JObject Content { get; set; }
        public IDictionary<string, object> Translations { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public State State { get; set; }

        public static Expression<Func<ContentDto, Content>> ConvertToFaqModel
        {
            get
            {
                return x => new Content()
                {
                    Id = x.Id,
                    Faq = new FAQ(),
                    ContentType = ContentType.FAQ,
                    Translations = new List<ContentTranslation>(),
                    State = x.State
                };
            }
        }

        public static Expression<Func<ContentDto, Content>> ConvertToAnnouncementModel
        {
            get
            {
                return x => new Content()
                {
                    Id = x.Id,
                    ContentType = ContentType.Announcement,
                    Announcement = new Announcement(),
                    Translations = new List<ContentTranslation>()
                };
            }
        }
        //public Dictionary<Language,TranslationDTO> Translations { get; set; }
    }


    //[JsonProperty, JsonConverter(typeof(DictionaryWithSpecialEnumKeyConverter<Language>))]
       // public IDictionary<string,object> Translations { get; set; }
        //public ODataNamedValueDictionary< TranslationDTO> Translations { get; set; }
        //map from EF to this model->client
        //IQuerable sticks to model not to entity
        //map ODATA query to entity


    

    public class TranslationDTO
    {
        public int Id { get; set; }
        public string ContentText { get; set; }
    }

   
}
