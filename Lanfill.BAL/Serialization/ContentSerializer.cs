using Landfill.Models;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Formatter.Serialization;
using Microsoft.OData;
using Microsoft.OData.Edm;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace Lanfill.BAL.Implementation.Serialization
{
    public class ContentSerializer : ODataResourceSerializer
    {
        public ContentSerializer(ODataSerializerProvider sp) : base(sp) { }

        public override ODataResource CreateResource(SelectExpandNode selectExpandNode, ResourceContext resourceContext)
        {
            var resource = base.CreateResource(selectExpandNode, resourceContext);
            var res = resourceContext.ResourceInstance as ContentDto;

            if (resource != null && res.Content != null)
                resource = WriteJobjectData(res.Content);
            return resource;
        }

        private ODataResource WriteJobjectData(JObject content)
        {
            var contentODataResource = new ODataResource
            {
                Properties = new List<ODataProperty>()
            };
            var dictOfProperties = content.Properties();
            foreach (var property in dictOfProperties)
            {
                contentODataResource.Properties.ToList().Add(new ODataProperty
                {
                    Name = property.Name,
                    Value = new ODataUntypedValue
                    {
                        RawValue = JsonConvert.SerializeObject(property.Value)
                    }
                });
            }
            return contentODataResource;
        }

        public override void WriteObjectInline(object graph, IEdmTypeReference expectedType, ODataWriter writer, ODataSerializerContext writeContext)
        {
            // This cast is safe because the type is checked before using this serializer.
            //Incompatible type kinds were found. The type 'Collection(Newtonsoft.Json.Linq.JToken)' was found to be of kind 'Collection' instead of the expected kind 'None'.
            //var content = graph as JObject;
            var content = (JToken)graph;
            //var model =content.ToObject<FaqModel>();
            var property = content.ToObject<JProperty>();
            // JObject content = (JObject)graph;
            //JObject content =(JObject)jtoken.Parent;
            //var contentDto = graph as ContentDto;

            if (content != null)
            {
                //var content = contentDto.Content;
                List<ODataProperty> dynamicProperties = new List<ODataProperty>();
                // var dictOfProperties = content.Properties();
                //foreach (var property in property)
                //{
                dynamicProperties.Add(new ODataProperty
                {
                    Name = property.Name,
                    Value = new ODataUntypedValue
                    {
                        RawValue = JsonConvert.SerializeObject(property.Value)
                    }
                });
                //}
                writer.WriteStart(new ODataResource
                {
                    TypeName = expectedType.FullName(),
                    Properties = dynamicProperties,
                });

                writer.WriteEnd();
            }
            else
            {
                base.WriteObjectInline(graph, expectedType, writer, writeContext);
            }

            // var contentDTOModel = (ContentDto)graph;

        }
    }
}
