using Landfill.BAL.Abstract;
using Landfill.DAL.Implementation.Core;
using Landfill.Entities;
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
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using static Landfill.Common.Enums.EnumsContainer;
using static Landfill.Web.Controllers.ContentController;

namespace Landfill.Web
{
    public partial class Startup
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
                // var settings = new JsonSerializerSettings { Converters = new JsonConverter[] { new DictionaryWithSpecialEnumKeyConverter<Language>() } };
                // Use the default property (Pascal) casing
                //options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                // Configure a custom converter
                options.SerializerSettings.Converters.Add(new DictionaryWithSpecialEnumKeyConverter<Language>());
            });
            services.AddTransient<IContentService, ContentService>();
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
                    containerBuilder.AddService(Microsoft.OData.ServiceLifetime.Singleton, typeof(ODataSerializerProvider), sp => new CustomODataSerializerProvider(sp));
                });

            });
        }

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
                if (edmType.FullName() == typeof(JToken).FullName)//"Collection(Newtonsoft.Json.Linq.JToken)"// typeof(JToken).FullName
                {
                    return contentSerializer;//Newtonsoft.Json.Linq.JToken
                }
                var isAsm = (edmType.FullName() == typeof(Dictionary<Language, TranslationDTO>).FullName);
                return base.GetEdmTypeSerializer(edmType);
            }
        }



        private static IEdmModel GetEdmModel()
        {
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<ContentDto>("Content");
            builder.EntitySet<TranslationDTO>("Translation");
            return builder.GetEdmModel();
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
