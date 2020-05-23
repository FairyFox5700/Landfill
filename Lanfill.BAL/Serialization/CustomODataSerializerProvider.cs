using Landfill.Models;
using Microsoft.AspNet.OData.Formatter.Serialization;
using Microsoft.OData.Edm;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using static Landfill.Common.Enums.EnumsContainer;

namespace Lanfill.BAL.Implementation.Serialization
{
    public class CustomODataSerializerProvider : DefaultODataSerializerProvider
    {
        private ContentSerializer contentSerializer;


        public CustomODataSerializerProvider(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            contentSerializer = new ContentSerializer(this);
        }

        public override ODataEdmTypeSerializer GetEdmTypeSerializer(IEdmTypeReference edmType)
        {
            var types = edmType.FullName();
            if (edmType.FullName() == typeof(ContentDto).FullName)//"Collection(Newtonsoft.Json.Linq.JToken)"// typeof(JToken).FullName
            {
                return contentSerializer;//Newtonsoft.Json.Linq.JToken
            }
            var isAsm = (edmType.FullName() == typeof(Dictionary<Language, TranslationDTO>).FullName);
            return base.GetEdmTypeSerializer(edmType);
        }
    }

}
