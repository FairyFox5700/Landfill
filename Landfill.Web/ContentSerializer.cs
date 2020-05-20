using Microsoft.AspNet.OData.Formatter.Serialization;
using Microsoft.OData;
using Microsoft.OData.Edm;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace Landfill.Web
{
    public partial class Startup
    {
        internal sealed class ContentSerializer : ODataResourceSerializer
        {
            public ContentSerializer(ODataSerializerProvider sp) : base(sp) { }

            //public override ODataResource CreateResource(SelectExpandNode selectExpandNode, ResourceContext resourceContext)
            //{
            //    var resource = base.CreateResource(selectExpandNode, resourceContext);
            //    var res = resourceContext.ResourceInstance as ContentDto;

            //    if (resource != null && res.Content != null)
            //        resource=WriteJobjectData(res.Content);
            //    return resource;
            //}

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
}

//internal sealed class ContentDictSerializer : ODataResourceSerializer
//{
//    public ContentDictSerializer(ODataSerializerProvider serializerProvider) : base(serializerProvider)
//    {
//    }

//    public override void WriteObjectInline(object graph, IEdmTypeReference expectedType, ODataWriter writer, ODataSerializerContext writeContext)
//    {
//        var dictOfProperties = content.Properties();
//    }
//}

//public override ODataResource CreateResource(SelectExpandNode selectExpandNode, ResourceContext resourceContext)
//{
//    var resource = base.CreateResource(selectExpandNode, resourceContext);
//    var res = resourceContext.ResourceInstance as ContentDto;

//    if (resource != null && res.Content != null)
//        WriteJobjectData(res.Content);
//    return resource;
//}

//public void WriteJobjectData(JObject content)
//{
//    var dictOfProperties = content.Properties();

//    List<ODataProperty> dynamicProperties = new List<ODataProperty>();

//    foreach (var property in dictOfProperties)
//    {
//        if (property.Value != null)
//        {
//            dynamicProperties.Add(new ODataProperty
//            {
//                Name = property.Name,
//                Value = new ODataUntypedValue
//                {
//                    RawValue = property.Value.ToString()
//                }
//            });
//        }
//        continue;
//    }
//}

//        var model = resourceContext.SerializerContext.Model;
//        IEdmTypeReference edmTypeReference = model.FindType(property.Value.GetType().FullName) as IEdmTypeReference;


//         //IEdmTypeReference edmTypeReference = resourceContext.SerializerContext.GetEdmType(property.Value,
//          property.Value.GetType();
//        if (edmTypeReference == null)
//        {
//            throw new ArgumentNullException();
//        }

//        if (edmTypeReference.IsStructured() ||
//           (edmTypeReference.IsCollection() && edmTypeReference.AsCollection().ElementType().IsStructured()))
//        {
//            if (resourceContext.DynamicComplexProperties == null)
//            {
//                resourceContext.DynamicComplexProperties = new ConcurrentDictionary<string, object>();
//            }

//            resourceContext.DynamicComplexProperties.Add(property.Name, property.Value);//addded properies
//        }
//        else
//        {
//            ODataEdmTypeSerializer propertySerializer = SerializerProvider.GetEdmTypeSerializer(edmTypeReference);
//            if (propertySerializer == null)
//            {
//                throw new ArgumentNullException();
//            }

//            dynamicProperties.Add(propertySerializer.CreateProperty(
//                property.Value, edmTypeReference, property.Name, resourceContext.SerializerContext));
//        }
//    }

//    if (dynamicProperties.Any())
//    {
//        resource.Properties = resource.Properties.Concat(dynamicProperties);
//    }
//}



//internal  ODataProperty CreateProperty(object graph, IEdmTypeReference expectedType, string elementName,
//           ODataSerializerContext writeContext)
//{

//    return new ODataProperty
//    {
//        Name = elementName,
//        Value = CreateODataValue(graph, expectedType, writeContext)
//    };
//}


/*                var model = resourceContext.EdmModel;
                var newModel = new EdmModel();
                EdmEntityType content = new EdmEntityType("Landfill.Model", "ContentDto");
                content.AddKeys(content.AddStructuralProperty("Id", EdmPrimitiveTypeKind.Int32));

                EdmEntityType faq = new EdmEntityType("Landfill.Model", "FaqModel");
                faq.AddKeys(faq.AddStructuralProperty("ContentId", EdmPrimitiveTypeKind.Int32));
                faq.AddStructuralProperty("MainTag", EdmPrimitiveTypeKind.String);
                
                //language
                EdmEnumType state = new EdmEnumType("Landfill.Common.Enums", "State");
                state.AddMember(new EdmEnumMember(state, "Red", new EdmEnumMemberValue(0)));
                state.AddMember(new EdmEnumMember(state, "Blue", new EdmEnumMemberValue(1)));
                state.AddMember(new EdmEnumMember(state, "Green", new EdmEnumMemberValue(2)));
                newModel.AddElement(faq);
                newModel.AddElement(state);
                // navigation properties
                EdmNavigationProperty ordersNavProp = content.AddUnidirectionalNavigation(
                    new EdmNavigationPropertyInfo
                    {
                        Name = "Faqs",
                        TargetMultiplicity = EdmMultiplicity.Many,
                        Target = faq
                    });
                content.AddNavigationTarget(ordersNavProp,faq);
                newModel.AddElement(address);


                var complexType = new EdmComplexType("WebApiDocNS", "SubAddress");

    */
