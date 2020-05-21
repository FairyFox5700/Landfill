using Landfill.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using static Landfill.Common.Enums.EnumsContainer;

namespace Landfill.Models.Extension
{
    public static class DictionaryExtensions
    {
        public static IEnumerable<ContentTranslation> ConvertToListTranslations(this IDictionary<string, object> contentDictTranslations)
        {
            List<ContentTranslation> contentTranslations = new List<ContentTranslation>();
            foreach (var keyValue in contentDictTranslations)
            {
                //Maybe Check Here
                var language = (Language)Enum.Parse(typeof(Language), keyValue.Key);
                var translation = keyValue.Value as TranslationDTO;
                if (translation == null) throw new ArgumentNullException(nameof(translation));
                var translationEntity = TranslationDTO.FromTranslationModel(translation);
                translationEntity.Language = language;
                contentTranslations.Add(translationEntity);
            }
            return contentTranslations;
        }
    }
}
