using Landfill.Models;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Formatter.Serialization;
using Microsoft.OData;
using Microsoft.OData.Edm;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Lanfill.BAL.Implementation.Serialization
{
    public class ContentSerializer : ODataResourceSerializer
    {
        public ContentSerializer(ODataSerializerProvider sp) : base(sp) { }

        //public override ODataResource CreateResource(SelectExpandNode selectExpandNode, ResourceContext resourceContext)
        //{
        //    var resource = base.CreateResource(selectExpandNode, resourceContext);
        //    var contentModel = resourceContext.ResourceInstance as ContentDto;

        //    if (resource != null && contentModel.Content != null)
        //        resource = WriteJobjectData(contentModel.Content,resourceContext);
        //   // AppendDynamicProperties(resource, selectExpandNode, resourceContext);
        //    return resource;

        //}

        public override void WriteObject(object graph, Type type, ODataMessageWriter messageWriter, ODataSerializerContext writeContext)
        {
            base.WriteObject(graph, type, messageWriter, writeContext);
        }
        public override ODataProperty CreateStructuralProperty(IEdmStructuralProperty structuralProperty, ResourceContext resourceContext)
        {
            return base.CreateStructuralProperty(structuralProperty, resourceContext);
        }
        public override void AppendDynamicProperties(ODataResource resource, SelectExpandNode selectExpandNode, ResourceContext resourceContext)
        {
            base.AppendDynamicProperties(resource, selectExpandNode, resourceContext);
        }
        public override void WriteDeltaObjectInline(object graph, IEdmTypeReference expectedType, ODataWriter writer, ODataSerializerContext writeContext)
        {
            base.WriteDeltaObjectInline(graph, expectedType, writer, writeContext);
        }

        //private ODataResource WriteJobjectData(JObject content, ResourceContext resourceContext)
        //{
        //    string typeName = resourceContext.StructuredType.FullTypeName();
        //    var contentODataResource = new ODataResource
        //    {
        //        Id = resourceContext.GenerateSelfLink(false),
        //        TypeName = typeName,
        //        Properties = new List<ODataProperty>()
        //    };
        //    var dictOfProperties = content.Properties();
        //    foreach (var property in dictOfProperties)
        //    {
        //        contentODataResource.Properties.ToList().Add(new ODataProperty
        //        {
        //            Name = property.Name,
        //            Value = new ODataUntypedValue
        //            {
        //                RawValue = JsonConvert.SerializeObject(property.Value)
        //            }
        //        });
        //    }
        //    return contentODataResource;
        //}

        public override void WriteObjectInline(object graph, IEdmTypeReference expectedType, ODataWriter writer, ODataSerializerContext writeContext)
        {
            // This cast is safe because the type is checked before using this serializer.
            //Incompatible type kinds were found. The type 'Collection(Newtonsoft.Json.Linq.JToken)' was found to be of kind 'Collection' instead of the expected kind 'None'.
            var content = (JToken)graph;
            var property = content.ToObject<JProperty>();
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

            //exception	{"Unable to cast object of type 'Newtonsoft.Json.Linq.JProperty' to type 'Landfill.Models.ContentDto'."}	
            //System.Exception {System.InvalidCastException}

        }
    }



    public class ContentSetSerializer : ODataResourceSetSerializer
    {
        public ContentSetSerializer(ODataSerializerProvider sp) : base(sp) { }

        //public override ODataResource CreateResource(SelectExpandNode selectExpandNode, ResourceContext resourceContext)
        //{
        //    var resource = base.CreateResource(selectExpandNode, resourceContext);
        //    var contentModel = resourceContext.ResourceInstance as ContentDto;

        //    if (resource != null && contentModel.Content != null)
        //        resource = WriteJobjectData(contentModel.Content,resourceContext);
        //   // AppendDynamicProperties(resource, selectExpandNode, resourceContext);
        //    return resource;

        //}

        public override void WriteObject(object graph, Type type, ODataMessageWriter messageWriter, ODataSerializerContext writeContext)
        {
            base.WriteObject(graph, type, messageWriter, writeContext);
        }
        

        //private ODataResource WriteJobjectData(JObject content, ResourceContext resourceContext)
        //{
        //    string typeName = resourceContext.StructuredType.FullTypeName();
        //    var contentODataResource = new ODataResource
        //    {
        //        Id = resourceContext.GenerateSelfLink(false),
        //        TypeName = typeName,
        //        Properties = new List<ODataProperty>()
        //    };
        //    var dictOfProperties = content.Properties();
        //    foreach (var property in dictOfProperties)
        //    {
        //        contentODataResource.Properties.ToList().Add(new ODataProperty
        //        {
        //            Name = property.Name,
        //            Value = new ODataUntypedValue
        //            {
        //                RawValue = JsonConvert.SerializeObject(property.Value)
        //            }
        //        });
        //    }
        //    return contentODataResource;
        //}

        public override void WriteObjectInline(object graph, IEdmTypeReference expectedType, ODataWriter writer, ODataSerializerContext writeContext)
        {
            // This cast is safe because the type is checked before using this serializer.
            //Incompatible type kinds were found. The type 'Collection(Newtonsoft.Json.Linq.JToken)' was found to be of kind 'Collection' instead of the expected kind 'None'.
            
            var content = (JToken)graph;
            var property = content.ToObject<JProperty>();
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

            var contentDTOModel = (ContentDto)graph;
        }
    }
}
