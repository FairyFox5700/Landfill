using Landfill.Entities;
using Landfill.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Landfill.Common.Enums.EnumsContainer;

namespace Lanfill.BAL.Implementation.Extensions
{
    public static class EnumerableTranslationExtension
    {

        public static Dictionary<Language, TranslationDTO> ConvertToDictTranslations(this IEnumerable<ContentTranslation> contentTranslations)
        {
            Dictionary<Language, TranslationDTO> dictionary = contentTranslations
                        .Select(p => new
                        {
                            Key = p.Language,
                            Value = new TranslationDTO() { ContentText = p.Text }
                        })
                      .ToDictionary(x => x.Key, x => x.Value);
            return dictionary;
        }
        //TODo cast or more
        public static ODataNamedValueDictionary<TranslationDTO> ConvertToOdataDictTranslations(this IEnumerable<ContentTranslation> contentTranslations)
        {
            ODataNamedValueDictionary< TranslationDTO> dictionary = new ODataNamedValueDictionary<TranslationDTO>();
            foreach (var translation in contentTranslations)
            {
                dictionary.Add(translation.Language.ToString(), new TranslationDTO() { ContentText = translation.Text });
            }
            return dictionary;
        }

        public static ODataNamedValueDictionary< TranslationDTO> ConvertToOdataDictTranslations(this Dictionary<Language, TranslationDTO> contentTranslations)
        {
            ODataNamedValueDictionary< TranslationDTO> dictionary = new ODataNamedValueDictionary<TranslationDTO>();
            foreach (var translation in contentTranslations)
            {
                dictionary.Add(translation.Key.ToString(), translation.Value );
            }
            return dictionary;
        }

        public static IDictionary<string, object>ConvertToDictTranslations(this Dictionary<Language, TranslationDTO> contentTranslations)
        {
            IDictionary<string, object> dictionary = new Dictionary<string, object>();
            foreach (var translation in contentTranslations)
            {
                dictionary.Add(translation.Key.ToString(), (object)translation.Value);
            }
            return dictionary;
        }


      
    }
}
