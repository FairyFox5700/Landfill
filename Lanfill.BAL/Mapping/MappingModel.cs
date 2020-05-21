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
    public interface IMappingModel
    {
        public ContentDto MapToContentDTO(Content contentEntity);
        public Content MapToContent(ContentDto contentDto);
        public IQueryable<ContentDto> MapToContentDTO(IQueryable<Content> contents);
        public IQueryable<ContentDto> MapToContentDTO(IQueryable<Content> contents, QueryFilterSet filters);

    }
    public class MappingModel : IMappingModel
    {
        private readonly LandfillContext context;
        public MappingModel(LandfillContext context)
        {
            this.context = context;
        }
        List<ContentDto> resultList = new List<ContentDto>();

        public ContentDto MapToContentDTO(Content contentEntity)
        {
            var model = GetContentByIdWithTranslation(contentEntity.Id, contentEntity.ContentType);
            return model;
        }
        public IQueryable<ContentDto> MapToContentDTO(IQueryable<Content> contents)
        {
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
                Content = GetContentAsync(contentType, contentId),
                Translations = requestTuple.Item2.ConvertToDictTranslations()//.ConvertToDictTranslations()

            };

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
        private JObject GetContentAsync(ContentType contentType, int contentID)
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

        private FaqModel GetFaq(int contentId, ContentType contentType)
        {
            var model = (from content in context.Contents
                         join faq in context.FAQs on content.Id equals faq.ContentId
                         where content.ContentType == ContentType.FAQ
                         && content.Id == contentId
                         select new FaqModel()
                         {
                             ContentId = content.Id,
                             State = content.State,
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
                             State = content.State,
                             Header = announcement.Header,
                             ValiUntil = announcement.ValiUntil
                         })
                        .FirstOrDefault();
            return model;
        }

        public IQueryable<ContentDto> MapToContentDTO(IQueryable<Content> contents, QueryFilterSet filters)
        {
            throw new NotImplementedException();
        }
        //MapBack
        public Content MapToContent(ContentDto contentDto)
        {
            var content = new Content();
           
        }

        /// <summary>
        /// Method to parse JObject
        /// </summary>
        /// <param name="jObject"></param>
        public dynamic GetModel(string jObject)
        {
            var responceJObject = JObject.Parse(jObject);
            dynamic model = null;
            if (responceJObject["ContentType"].ToString()== ContentType.Announcement.ToString())
            {
                model = TryConvertModel<AnnouncementModel>(responceJObject);
            }
            else
            {
                model = TryConvertModel<FaqModel>(responceJObject);
            }
            return model;

        }

        private TContent TryConvertModel<TContent>(JObject content) where TContent : class//where TConcent:IContent
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
