using Landfill.BAL.Abstract;
using Landfill.DAL.Implementation.Core;
using Landfill.Entities;
using Landfill.Models;
using LandFill.DAL.Abstract;
using Lanfill.BAL.Implementation;
using Lanfill.BAL.Implementation.Mapping;
using Microsoft.AspNet.OData.Query;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;


namespace Lanfill.BAL
{
    public class ContentService : IContentService
    {
        //public ContentDTO GetContentModel(int contentId, ContentType contentType)
        //{
        //    return null;
        //}
        private readonly LandfillContext context;
        private readonly IMappingModel mappingModel;
        private readonly ILogger<ContentService> logger;
        private readonly IContentRepository contentRepository;

        public ContentService(LandfillContext context, IMappingModel mappingModel, ILogger<ContentService> logger, IContentRepository contentRepository)
        {
            this.context = context;
            this.mappingModel = mappingModel;
            this.logger = logger;
            this.contentRepository = contentRepository;
        }
        public IEnumerable<Content> GetAllConntent()
        {
            return context.Contents;
        }

        public IQueryable<ContentDto> GetAllContent(ODataQueryOptions<ContentDto> options)
        {
            var filter = options.GetFilter();

            var map = new ExpressionMap()
                .Add<ContentDto, Content>()
                .Add((ContentDto c) => c.Id, (Content c) => c.Id)
                .Add<FaqModel, FAQ>();
            var mappedPredicate = ((Expression<Func<Content, bool>>)map.Map(filter));
            //var mappedProperty = new MappedProperties<ContentDto, Content>() {};//DtoProperty and EntityProperty
            //var expressionVistor = new BaseExpressionConverter<ContentDto, Content>(
            //    new MappingContainer<ContentDto, Content> { 
            //Mappings = new List<MappedProperties<ContentDto, Content>> {  }
            //});
            //var mappedExpression = expressionVistor.Visit(filter);

            IEnumerable<Content> results = contentRepository.GetContent(mappedPredicate);
            var result = mappingModel.MapToContentDTO(results.AsQueryable());//convert to enumerable//TODO
            return result;
        }


        public IQueryable<ContentDto> GetAllContent()
        {
            var list = context.Contents;
            var result = mappingModel.MapToContentDTO(list);
            return result;
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




