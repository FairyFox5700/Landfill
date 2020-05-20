using Landfill.DAL.Implementation.Core;
using Landfill.Entities;
using Landfill.Models;
using Lanfill.BAL.Implementation.Extensions;
using Lanfill.BAL.Implementation.Mapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using static Landfill.Common.Enums.EnumsContainer;

namespace Lanfill.BAL
{
    public class ContentService: IContentService
    {
        //public ContentDTO GetContentModel(int contentId, ContentType contentType)
        //{
        //    return null;
        //}
        private readonly LandfillContext context;
        private readonly IMappingModel mappingModel;
        private readonly ILogger<ContentService> logger;

        public ContentService(LandfillContext context, IMappingModel mappingModel,ILogger<ContentService> logger)
        {
            this.context = context;
            this.mappingModel = mappingModel;
            this.logger = logger;
        }
        public IEnumerable<Content> GetAllConntent()
        {
            return context.Contents;
        }

        public IQueryable<ContentDto> GetAllContent()
        {
            var list = context.Contents;
            var result = mappingModel.MapToContentDTO(list);
            return result;
        }
        /// <summary>
        /// Method to parse JObject
        /// </summary>
        /// <param name="jObject"></param>
        public void GetModel(string jObject)
        {
            var responceJObject = JObject.Parse(jObject);
            dynamic model = null;
            model = TryConvertModel<FaqModel>(responceJObject);
        }

        private TContent TryConvertModel<TContent>(JObject content) where TContent:class//where TConcent:IContent
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
            catch(Exception ex)
            {
                logger.LogError("JObject not parced properly", ex.Message);
                return default(TContent);
            }

        }
    }

}












//public IEnumerable<Content> GetAllConntentByContentId(int contentId, ContentType contentType)
//{
//    var query = context.Contents
//        .Where(c => c.Id == contentId)
//        .Where(c => c.ContentType == contentType)
//        .Include(c => c.Translations);

//    ///=>Faq type
//    if (contentType == ContentType.FAQ)
//    {
//        var model =
//    }
//    var dataResult = query
//      .Select(resultVal => new ContentDto()
//      { Content = JObject.FromObject( });


//}


/// <summary>
/// get conten by id and type
/// </summary>
/// <param name="contentId"></param>
/// <param name="contentType"></param>
/// <returns></returns>
//public FaqModel GetFaq(int contentId, ContentType contentType)
//{
//    var model = (from content in context.Contents
//                 join translation in context.ContentTranslations on content.Id equals translation.ContentId
//                 join faq in context.FAQs on content.Id equals faq.ContentId
//                 where content.ContentType == contentType
//                 && content.Id==contentId
//                 select new FaqModel()
//                 {
//                     Id = translation.ContentId,
//                     MainTag = faq.Tag
//                 })
//                .FirstOrDefault();
//    return model;
//    // from faqs in context.FAQs
//    //%% content.State == State.Published
//}




