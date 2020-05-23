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
using Landfill.Models.Extension;

namespace Landfill.Models
{
    public class ContentDto
    {
        public int Id { get; set; }
     
        public JObject Content { get; set; }
        public IDictionary<string, object> Translations { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public State State { get; set; }

        [NotMapped]
        public static Expression<Func<ContentDto, Content>> ConvertToFaqModel
        {
            get
            {
                return x => new Content()
                {
                    Id = x.Id,
                    Faq = FaqModel.FromFaqModel(GetModel(x.Content,ContentType.FAQ) as FaqModel),//HERE convertion to derivative type
                    ContentType = ContentType.FAQ,
                    Translations = x.Translations.ConvertToListTranslations().ToList(),
                   // State = x.State
                };
            }
        }

        [NotMapped]
        public static Expression<Func<ContentDto, Content>> ConvertToAnnouncementModel
        {
            get
            {
                return x => new Content()
                {
                    Id = x.Id,
                    ContentType = ContentType.Announcement,
                    Announcement =  AnnouncementModel.FromAnnouncementModel(GetModel(x.Content, ContentType.Announcement) as AnnouncementModel),//Jobject
                    Translations = x.Translations.ConvertToListTranslations().ToList(),
                    //State = x.State
                };
            }
        }
      
        public static Content ConvertToFaq(ContentDto model)
        {
            return ConvertToFaqModel.Compile().Invoke(model);
        }
       
        public static Content ConvertToAnnouncement(ContentDto model)
        {
            return ConvertToAnnouncementModel.Compile().Invoke(model);
        }
       
        #region move to services
        public static  dynamic GetModel(JObject jObject, ContentType contentType)
        {
            dynamic model = null;
            if (contentType == ContentType.Announcement)//responceJObject["ContentType"].ToString()
            {
                model = TryConvertModel<AnnouncementModel>(jObject);
            }
            else
            {
                model = TryConvertModel<FaqModel>(jObject);
            }
            return model;
        }

      
        private static TContent TryConvertModel<TContent>(JObject content) where TContent : class//where TConcent:IContent
        {
            if (content == null)
                throw new ArgumentNullException();
            try
            {
                var validatedModel = content.ToObject<TContent>();
                if (validatedModel == null)
                    return null;
                return validatedModel;
            }
            catch (Exception ex)
            {
                //logger.LogError("JObject not parced properly", ex.Message);
                return default(TContent);
            }

        }
        #endregion


        //public Dictionary<Language,TranslationDTO> Translations { get; set; }
    }


}
