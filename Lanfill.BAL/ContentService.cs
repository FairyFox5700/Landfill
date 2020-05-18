using Landfill.DAL.Implementation.Core;
using Landfill.Entities;
using Landfill.Models;
using Lanfill.BAL.Implementation.Extensions;
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
        private readonly ILogger<ContentService> logger;

        public ContentService(LandfillContext context,ILogger<ContentService> logger)
        {
            this.context = context;
            this.logger = logger;
        }
        public IEnumerable<Content> GetAllConntent()
        {
            return context.Contents;
        }

        public IQueryable<ContentDto> GetAllContent()
        {
            List<ContentDto> resultList = new List<ContentDto>();
            var list = context.Contents;
            foreach( var entityContent in list)
            {
                var model = GetContentByIdWithTranslation(entityContent.Id, entityContent.ContentType);
                resultList.Add(model);
            }
            return resultList.AsQueryable();
           // return context.Contents.Select(entityContent => );

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

        //The collection type 'System.Collections.Generic.Dictionary`2[Landfill.Common.Enums.EnumsContainer+Language,Landfill.Models.TranslationDTO]' 
        //    on 'Landfill.Models.ContentDto.Translations' is not supported.
        public ContentDto GetContentByIdWithTranslation(int contentId, ContentType contentType)
        {
            var requestTuple = GetContentAndRelatedListOfTransactionById(contentId);
            return new ContentDto
            {
                Content = GetContentAsync(contentType, contentId),
               // Translations =
                //requestTuple.Item2.ConvertToDictTranslations()

            };

        }
        private JObject GetContentAsync(ContentType contentType, int contentID)
        {
            var settings = new JsonSerializerSettings { Converters = new JsonConverter[] { new StringEnumConverter() } };
            if (contentType == ContentType.FAQ)
            {
                var faq = GetFaq(contentID, contentType);
                return JObject.FromObject(faq,JsonSerializer.Create(settings));
            }
            if (contentType == ContentType.Announcement)
            {
                var annoncement = GetAnnouncement(contentID, contentType);
                return JObject.FromObject(annoncement, JsonSerializer.Create(settings));
            }
            else
                return null;
        }
        public Tuple<Content, Dictionary<Language, TranslationDTO>> GetContentAndRelatedListOfTransactionById(int contentId)
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
        public IQueryable<AnnouncementModel> GetAnnouncement()
        {
            var model = from content in context.Contents
                            //join translation in context.ContentTranslations on content.Id equals translation.ContentId
                        join announcement in context.Announcements on content.Id equals announcement.ContentId
                        where content.ContentType == ContentType.Announcement
                        select new AnnouncementModel()
                        {
                            ContentId = content.Id,
                            State = content.State,
                            Header = announcement.Header,
                            ValiUntil = announcement.ValiUntil
                        };
            return model;
        }



        public FaqModel GetFaq(int contentId, ContentType contentType)
        {
            var model = (from content in context.Contents
                         //join translation in context.ContentTranslations on content.Id equals translation.ContentId
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
        public AnnouncementModel GetAnnouncement(int contentId, ContentType contentType)
        {
            var model = (from content in context.Contents
                        // join translation in context.ContentTranslations on content.Id equals translation.ContentId
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



    }
   
}
