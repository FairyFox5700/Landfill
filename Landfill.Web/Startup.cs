using Landfill.BAL.Abstract;
using Landfill.DAL.Implementation.Core;
using Landfill.Models;
using Lanfill.BAL;
using Lanfill.BAL.Implementation.Mapping;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.OData.Formatter.Serialization;
using Microsoft.AspNet.OData.Routing.Conventions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OData;
using Microsoft.OData.Edm;
using System;
using System.Collections.Generic;
using System.Linq;
using LandFill.DAL.Abstract;
using Landfill.DAL.Implementation.Repositories;
using Lanfill.BAL.Implementation.Serialization;
using static Landfill.Common.Enums.EnumsContainer;
using static Landfill.Web.Controllers.ContentController;
using Newtonsoft.Json.Linq;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Formatter;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

namespace Landfill.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<LandfillContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddTransient<IMappingModel, MappingModel>();
            services.AddMvc(options =>
            {
                options.EnableEndpointRouting = false;

            }).SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_3_0).AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.Converters.Add(new DictionaryWithSpecialEnumKeyConverter<Language>());
            });
            services.AddTransient<IContentService, ContentService>();
            services.AddTransient<IContentRepository, ContentRepository>();
            services.AddOData();

        }
        private IEdmModel DefineEdmModel(IServiceProvider services)
        {
            var modelManager = new EdmModel();
            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<ContentDto>(nameof(ContentDto));
            builder.EntityType<ContentDto>().HasKey(ai => ai.Id); // the call to HasKey is mandatory

            return modelManager;
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthorization();

            app.UseMvc(routeBuilder =>
            {
                routeBuilder.Expand().Select().OrderBy().Filter().Count().MaxTop(10);
                routeBuilder.MapODataServiceRoute("odata", "odata", containerBuilder =>
                {
                    containerBuilder.AddService(Microsoft.OData.ServiceLifetime.Singleton, typeof(IEdmModel), sp => GetEdmModel())
                      .AddService(Microsoft.OData.ServiceLifetime.Singleton, typeof(IEnumerable<IODataRoutingConvention>), sp =>
                        ODataRoutingConventions.CreateDefaultWithAttributeRouting("odata", routeBuilder));
                    containerBuilder.AddService<ODataSerializer, MongoDataSerializer>(Microsoft.OData.ServiceLifetime.Singleton);
                    containerBuilder.AddService<ODataSerializerProvider, CustomODataSerializerProvider2>(Microsoft.OData.ServiceLifetime.Singleton);
                //containerBuilder.AddService(Microsoft.OData.ServiceLifetime.Singleton, typeof(ODataSerializerProvider), sp => new CustomODataSerializerProvider(sp));
                });

            });
        }




        private static IEdmModel GetEdmModel()
        {
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<ContentDto>("Content").EntityType.HasKey(r => r.Id);
            builder.EntitySet<TranslationDTO>("Translation").EntityType.HasKey(r => r.Id); 
            return builder.GetEdmModel();
        }

        public class CustomODataSerializerProvider2 : DefaultODataSerializerProvider
        {
            private readonly ContentSerializer dataSerializer;

            public CustomODataSerializerProvider2(
                IServiceProvider odataServiceProvider)
                : base(odataServiceProvider)
            {
                this.dataSerializer= new ContentSerializer(this);
            }

            public override ODataEdmTypeSerializer GetEdmTypeSerializer(IEdmTypeReference edmType)
            {
                if (edmType.FullName()==typeof(JToken).FullName)                                           //FullName() == typeof(ContentDto).FullName
                {
                    return this.dataSerializer;
                }

                return base.GetEdmTypeSerializer(edmType);
            }
        }

       

        public class DataSerSerializator : ODataResourceSetSerializer
        {
            public DataSerSerializator(ODataSerializerProvider serializerProvider) : base(serializerProvider)
            {
            }
         
            public override ODataOperation CreateODataOperation(IEdmOperation operation, ResourceSetContext resourceSetContext, ODataSerializerContext writeContext)
            {
                return base.CreateODataOperation(operation, resourceSetContext, writeContext);
            }
            public override void WriteObject(object graph, Type type, ODataMessageWriter messageWriter, ODataSerializerContext writeContext)
            {
                base.WriteObject(graph, type, messageWriter, writeContext);
            }
            public override void WriteObjectInline(object graph, IEdmTypeReference expectedType, ODataWriter writer, ODataSerializerContext writeContext)
            {
                base.WriteObjectInline(graph, expectedType, writer, writeContext);
            }
            public override ODataValue CreateODataValue(object graph, IEdmTypeReference expectedType, ODataSerializerContext writeContext)
            {
                return base.CreateODataValue(graph, expectedType, writeContext);
            }
           
        }
        public class MongoDataSerializer : ODataResourceSerializer
        {
            public MongoDataSerializer(ODataSerializerProvider serializerProvider)
                : base(serializerProvider)
            {
            }
            public override ODataValue CreateODataValue(object graph, IEdmTypeReference expectedType, ODataSerializerContext writeContext)
            {
                return base.CreateODataValue(graph, expectedType, writeContext);
            }
            public override ODataFunction CreateODataFunction(IEdmFunction function, ResourceContext resourceContext)
            {
                return base.CreateODataFunction(function, resourceContext);
            }
            public override void AppendDynamicProperties(ODataResource resource, SelectExpandNode selectExpandNode, ResourceContext resourceContext)
            {
                base.AppendDynamicProperties(resource, selectExpandNode, resourceContext);
            }
            public override void WriteObject(object graph, Type type, ODataMessageWriter messageWriter, ODataSerializerContext writeContext)
            {
                base.WriteObject(graph, type, messageWriter, writeContext);
            }
            public override ODataAction CreateODataAction(IEdmAction action, ResourceContext resourceContext)
            {
                return base.CreateODataAction(action, resourceContext);
            }
            public override ODataProperty CreateStructuralProperty(IEdmStructuralProperty structuralProperty, ResourceContext resourceContext)
            {
                return base.CreateStructuralProperty(structuralProperty, resourceContext);
            }
            public override void WriteDeltaObjectInline(object graph, IEdmTypeReference expectedType, ODataWriter writer, ODataSerializerContext writeContext)
            {
                base.WriteDeltaObjectInline(graph, expectedType, writer, writeContext);
            }
            public override ODataNestedResourceInfo CreateNavigationLink(IEdmNavigationProperty navigationProperty, ResourceContext resourceContext)
            {
                return base.CreateNavigationLink(navigationProperty, resourceContext);
            }
            public override ODataResource CreateResource(SelectExpandNode selectExpandNode, ResourceContext resourceContext)
            {
                return base.CreateResource(selectExpandNode, resourceContext);
            }
            public override string CreateETag(ResourceContext resourceContext)
            {
                return base.CreateETag(resourceContext);
            }
            public override SelectExpandNode CreateSelectExpandNode(ResourceContext resourceContext)
            {
                return base.CreateSelectExpandNode(resourceContext);
            }
            public override void WriteObjectInline(
                object graph,
                IEdmTypeReference expectedType,
                ODataWriter writer,
                ODataSerializerContext writeContext)
            {
                // This cast is safe because the type is checked before using this serializer.
                var mongoData = (ContentDto)graph;
                var properties = new List<ODataProperty>();

                writer.WriteStart(new ODataResource
                {
                    TypeName = expectedType.FullName(),
                    Properties = properties,
                });

                writer.WriteEnd();
            }
        }











    }

}



//    private IEdmModel GetEdmModel()
//{
//    EdmModel model = new EdmModel();
//    EdmEnumType languages = new EdmEnumType("Landfill.Common.Enums", "Language");
//    languages.AddMember(new EdmEnumMember(languages, "UA", new EdmEnumMemberValue(0)));
//    languages.AddMember(new EdmEnumMember(languages, "EN", new EdmEnumMemberValue(1)));

//    var builder = new ODataConventionModelBuilder();
//    //builder.AddComplexType(typeof(ODataNamedValueDictionary<TranslationDTO>));
//    //uilder.AddEnumType(languages);
//    builder.EntitySet<ContentDto>("Content").EntityType.Name = "Content";
//    //builder.EntitySet<TranslationDTO>("Translation").EntityType.Name = "Transalation";


//    //builder.EntityType<ContentDto>();
//    return builder.GetEdmModel();
//}
//enum





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
