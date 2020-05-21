using Landfill.BAL.Abstract;
using Landfill.DAL.Implementation.Core;
using Landfill.Entities;
using Landfill.Models;
using Lanfill.BAL.Implementation.Extensions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Landfill.Common.Enums.EnumsContainer;

namespace Lanfill.BAL.Implementation.Mapping
{
    
    public class MappingModel : IMappingModel
    {
        private readonly LandfillContext context;
        public MappingModel(LandfillContext context)
        {
            this.context = context;
        }

        public ContentDto MapToContentDTO(Content contentEntity)
        {
            var model = GetContentByIdWithTranslation(contentEntity.Id, contentEntity.ContentType);
            model.State = contentEntity.State;
            return model;
        }
        public IQueryable<ContentDto> MapToContentDTO(IQueryable<Content> contents)
        {
            List<ContentDto> resultList = new List<ContentDto>();
            foreach (var entityContent in contents)
            {
                var model = MapToContentDTO(entityContent);
                resultList.Add(model);
            }
            return resultList.AsQueryable();
        }


        //The collection type 'System.Collections.Generic.Dictionary`2[Landfill.Common.Enums.EnumsContainer+Language,Landfill.Models.TranslationDTO]' 
        //    on 'Landfill.Models.ContentDto.Translations' is not supported.
        private ContentDto GetContentByIdWithTranslation(int contentId, ContentType contentType)
        {
            var requestTuple = GetContentAndRelatedListOfTransactionById(contentId);
            return new ContentDto
            {
                Content = GetContent(contentType, contentId),
                Translations = requestTuple.Item2.ConvertToDictTranslations(),//////////IDICT<string,OBJECT>
            };

        }
        private JObject GetContent(ContentType contentType, int contentID)
        {
            var settings = new JsonSerializerSettings { Converters = new JsonConverter[] { new StringEnumConverter() } };
            if (contentType == ContentType.FAQ)
            {
                var faq = GetFaq(contentID, contentType);
                return JObject.FromObject(faq, JsonSerializer.Create(settings));
            }
            if (contentType == ContentType.Announcement)
            {
                var annoncement = GetAnnouncement(contentID, contentType);
                return JObject.FromObject(annoncement, JsonSerializer.Create(settings));
            }
            else
                return null;
        }
        private Tuple<Content, Dictionary<Language, TranslationDTO>> GetContentAndRelatedListOfTransactionById(int contentId)
        {
            var query =
                from content in context.Contents
                join translation in context.ContentTranslations on content.Id equals translation.ContentId
                where content.Id == contentId
                select new Tuple<Content, ContentTranslation>(content, translation);
            if (query != null)
                return new Tuple<Content, Dictionary<Language, TranslationDTO>>(query.FirstOrDefault()?.Item1,
                    query.Select(p => p.Item2)?.ConvertToDictTranslations());
            else
                return null;//tuple is empty
        }


        //System.Runtime.Serialization.SerializationException: ODataResourceSerializer cannot write an object of type 'Collection(Newtonsoft.Json.Linq.JToken)'. 
        //at Microsoft.AspNet.OData.Formatter.Serialization.ODataResourceSerializer.GetResourceType(Object graph, ODataSerializerContext writeContext) at


        private FaqModel GetFaq(int contentId, ContentType contentType)
        {
            var model = (from content in context.Contents
                         join faq in context.FAQs on content.Id equals faq.ContentId
                         where content.ContentType == ContentType.FAQ
                         && content.Id == contentId
                         select new FaqModel()
                         {
                             ContentId = content.Id,
                             //State = content.State,
                             MainTag = faq.Tag
                         })
                        .FirstOrDefault();
            return model;
        }

        private AnnouncementModel GetAnnouncement(int contentId, ContentType contentType)
        {
            var model = (from content in context.Contents
                         join announcement in context.Announcements on content.Id equals announcement.ContentId
                         where content.ContentType == ContentType.Announcement
                         && content.Id == contentId
                         select new AnnouncementModel()
                         {
                             ContentId = content.Id,
                             Header = announcement.Header,
                             ValiUntil = announcement.ValiUntil
                             // State = content.State,
                         })
                        .FirstOrDefault();
            return model;
        }

        //MapBack need another model
        public Content MapToContent(ContentDto contentDto, ContentType contentType)
        {
            Content content;
            if (contentType == ContentType.Announcement)
                content = ContentDto.ConvertToAnnouncement(contentDto);
            else
                content = ContentDto.ConvertToFaq(contentDto);
            return content;


        }
    }
}

       

//private IQueryable<AnnouncementModel> GetAnnouncement()
//{
//    var model = from content in context.Contents
//                join announcement in context.Announcements on content.Id equals announcement.ContentId
//                where content.ContentType == ContentType.Announcement
//                select new AnnouncementModel()
//                {
//                    ContentId = content.Id,
//                    State = content.State,
//                    Header = announcement.Header,
//                    ValiUntil = announcement.ValiUntil
//                };
//    return model;
//}
