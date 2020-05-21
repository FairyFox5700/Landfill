using Landfill.Entities;
using System;
using System.Linq.Expressions;

namespace Landfill.Models
{
    //map from EF to this model->client
    //IQuerable sticks to model not to entity
    //map ODATA query to entity
    public class TranslationDTO
    {
        public int Id { get; set; }
        public string ContentText { get; set; }

        public static Expression<Func<TranslationDTO, ContentTranslation>> ConvertToTransaltionEntity
        {
            get
            {
                return x => new ContentTranslation()
                {
                    Id = x.Id,
                    Text= x.ContentText
                };
            }
        }

        public static ContentTranslation FromTranslationModel(TranslationDTO model)
        {
            return ConvertToTransaltionEntity.Compile().Invoke(model);
        }
    }

   
}
