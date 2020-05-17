using Landfill.DAL.Implementation.Core;
using Landfill.Models;
using Lanfill.BAL;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.OData.Formatter.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OData.Edm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.Serialization;
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
                routeBuilder.MapODataServiceRoute("odata", "odata", GetEdmModel());//api/api
            });

        }
        private IEdmModel GetEdmModel()
        {
            EdmModel model = new EdmModel();
            EdmEnumType languages = new EdmEnumType("Landfill.Common.Enums", "Language");
            languages.AddMember(new EdmEnumMember(languages, "UA", new EdmEnumMemberValue(0)));
            languages.AddMember(new EdmEnumMember(languages, "EN", new EdmEnumMemberValue(1)));
            
            var builder = new ODataConventionModelBuilder();
            //builder.AddComplexType(typeof(ODataNamedValueDictionary<TranslationDTO>));
            //uilder.AddEnumType(languages);
            builder.EntitySet<ContentDto>("Content").EntityType.Name = "Content";

           
            //builder.EntityType<ContentDto>();
            return builder.GetEdmModel();
        }
        //enum

      
        public class MySerializerProvider : DefaultODataSerializerProvider
        {
            public MySerializerProvider(IServiceProvider rootContainer) : base(rootContainer)
            {
            }
        }



    }
}
