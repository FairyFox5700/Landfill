using Landfill.DAL.Implementation.Core;
using Landfill.Models;
using Lanfill.BAL;
using Microsoft.AspNet.OData;
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
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using static Landfill.Common.Enums.EnumsContainer;
using static Landfill.Web.Controllers.ContentController;

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
            services.AddMvc(options =>
            {
                options.EnableEndpointRouting = false;

            }).SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_3_0).AddNewtonsoftJson(options =>
            {
                // var settings = new JsonSerializerSettings { Converters = new JsonConverter[] { new DictionaryWithSpecialEnumKeyConverter<Language>() } };
                // Use the default property (Pascal) casing
                //options.SerializerSettings.ContractResolver = new DefaultContractResolver();

                // Configure a custom converter
                options.SerializerSettings.Converters.Add(new DictionaryWithSpecialEnumKeyConverter<Language>());
            });
            services.AddTransient<IContentService, ContentService>();


            services.AddOData();
            //services.AddSingleton<IODataModelManger, ODataModelManager>(DefineEdmModel);

        }
        private Microsoft.OData.Edm.IEdmModel DefineEdmModel(IServiceProvider services)
        {
            var modelManager = new EdmModel();
            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<ContentDto>(nameof(ContentDto));
            builder.EntityType<ContentDto>().HasKey(ai => ai.Id); // the call to HasKey is mandatory

            return modelManager;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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
                //routeBuilder.EnableDependencyInjection();
                routeBuilder.Expand().Select().OrderBy().Filter().Count().MaxTop(10);
                // routeBuilder.MapODataServiceRoute("odata", "odata", GetEdmModel());//api/api
                // routeBuilder.MapODataServiceRoute("odata", "odata", GetEdmModel());
                //routeBuilder.MapODataServiceRoute("odata", "odata", a =>
                //{
                //    a.AddService(Microsoft.OData.ServiceLifetime.Singleton, typeof(IEdmModel), sp => GetEdmModel());
                //    a.AddService(Microsoft.OData.ServiceLifetime.Singleton, typeof(ODataSerializerProvider), sp => new DefaultODataDeserializerProvider(sp));

                //});
                routeBuilder.MapODataServiceRoute("odata", "odata", containerBuilder =>
                {
                    containerBuilder.AddService(Microsoft.OData.ServiceLifetime.Singleton, typeof(IEdmModel), sp => GetEdmModel())
                      .AddService(Microsoft.OData.ServiceLifetime.Singleton, typeof(IEnumerable<IODataRoutingConvention>), sp =>
                        ODataRoutingConventions.CreateDefaultWithAttributeRouting("odata", routeBuilder));
                    containerBuilder.AddService(Microsoft.OData.ServiceLifetime.Singleton, typeof(ODataSerializerProvider), sp => new CustomODataSerializerProvider(sp));
                });

            });
        }

        public class CustomODataSerializerProvider : DefaultODataSerializerProvider
        {
            private MySerializer _annotatingEntitySerializer;

            public CustomODataSerializerProvider(IServiceProvider serviceProvider) : base(serviceProvider)
            {
                _annotatingEntitySerializer = new MySerializer(this);
            }

            public override ODataEdmTypeSerializer GetEdmTypeSerializer(IEdmTypeReference edmType)
            {
                if (edmType.IsEntity())
                {
                    return _annotatingEntitySerializer;
                }
                return base.GetEdmTypeSerializer(edmType);
            }
        }



        private static IEdmModel GetEdmModel()
        {
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<ContentDto>("Content");
            return builder.GetEdmModel();
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



        internal sealed class MySerializerProvider : DefaultODataSerializerProvider
        {
            private MySerializer _mySerializer;

            public MySerializerProvider(IServiceProvider sp) : base(sp)
            {
                _mySerializer = new MySerializer(this);
            }

            public override ODataEdmTypeSerializer GetEdmTypeSerializer(IEdmTypeReference edmType)
            {
                var fullName = edmType.FullName();

                if (fullName == "Namespace.Bar")
                    return _mySerializer;
                else
                    return base.GetEdmTypeSerializer(edmType);
            }
        }


        internal sealed class MySerializer : ODataResourceSerializer
        {
            public MySerializer(ODataSerializerProvider sp) : base(sp) { }


            public override ODataResource CreateResource(SelectExpandNode selectExpandNode, ResourceContext resourceContext)
            {
                var resource = base.CreateResource(selectExpandNode, resourceContext);
                var res = resourceContext.ResourceInstance as ContentDto;

                if (resource != null && res.Content != null)
                    resource = BarToResource(res.Content);

                return resource;
            }

            private ODataResource BarToResource(JObject jobject)
            {
                var model = TryConvertModel<FaqModel>(jobject);
                if ( model != null)
                {
                    var odr = new ODataResource
                    {
                        Properties = new List<ODataProperty>
                {
                    new ODataProperty
                    {
                        Name = "State",
                        Value = model.State.ToString()
                    },
                    new ODataProperty
                    {
                        Name = "MainTag",
                        Value =model.MainTag
                    },
                      new ODataProperty
                    {
                        Name = "ContentId",
                        Value =model.ContentId
                    },

                }
                    };

                    return odr;
                }
                var modelAnnouncement = TryConvertModel<AnnouncementModel>(jobject);//"REFACTORING
                if ( modelAnnouncement!=null)
                {
                    var odr = new ODataResource
                    {
                        Properties = new List<ODataProperty>
                {
                    new ODataProperty
                    {
                        Name = "State",
                        Value = model.State.ToString()
                    },
                    new ODataProperty
                    {
                        Name = "MainTag",
                        Value =model.MainTag
                    },
                      new ODataProperty
                    {
                        Name = "ContentId",
                        Value =model.ContentId
                    },

                }

                    };
                    return odr;
                }
                else
                    return new ODataResource();

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
}


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
