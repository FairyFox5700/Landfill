using Landfill.BAL.Abstract;
using Landfill.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using static Landfill.Common.Enums.EnumsContainer;

namespace Lanfill.BAL.Implementation.Mapping
{

   
    public class JObjectConverter: IJObjectConverter
    {
        private readonly ILogger<JObjectConverter> logger;

        public JObjectConverter(ILogger<JObjectConverter> logger)
        {
            this.logger = logger;
        }
        /// <summary>
        /// Method to parse JObject
        /// </summary>
        /// <param name="jObject"></param>
        public dynamic GetModel(string jObject, ContentType contentType)
        {
            var responceJObject = JObject.Parse(jObject);
            dynamic model = null;
            if (contentType== ContentType.Announcement)//responceJObject["ContentType"].ToString() 
            {
                model = TryConvertModel<AnnouncementModel>(responceJObject);
            }
            else
            {
                model = TryConvertModel<FaqModel>(responceJObject);
            }
            return model;
        }

        public dynamic GetModel(JObject jObject, ContentType contentType)
        {
            dynamic model = null;
            if ( contentType== ContentType.Announcement)//responceJObject["ContentType"].ToString()
            {
                model = TryConvertModel<AnnouncementModel>(jObject);
            }
            else
            {
                model = TryConvertModel<FaqModel>(jObject);
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
