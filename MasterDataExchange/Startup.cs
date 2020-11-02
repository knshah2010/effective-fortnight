using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using FluentValidation;
using Framework.Extension;
using Framework.Library.Filter;
using Framework.Library.Validator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Test
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

            services.AddMvc(options =>
            {
                options.Filters.Add(new ExceptionActionFilter());
               // options.Filters.Add(new AuthenticateFilter());

                // options.Filters.Add(new ResultFilter());

                //    options.RespectBrowserAcceptHeader = true; // false by default
                //    options.OutputFormatters.Add(new XmlSerializerOutputFormatter());
            }).AddNewtonsoftJson(opt => {
                // opt.SerializerSettings.DateFormatString = "dd-MM-yyyy";
                // opt.SerializerSettings.DateFormatString = "yyyy-MM-dd";
                opt.SerializerSettings.Converters.Add(new MyDateTimeConvertor());

            });
            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
            //    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            //    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            //    c.IncludeXmlComments(xmlPath);
            //});
            services.AddCors();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            ValidatorOptions.CascadeMode = CascadeMode.StopOnFirstFailure;
            ValidatorOptions.LanguageManager = new CustomLanguageManager();

            //  JsonFormatter.SerializerSettings = new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Include };

            //CultureInfo newCulture = (CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            //newCulture.DateTimeFormat.ShortDatePattern = "dd-MMM-yyyy";
            //newCulture.DateTimeFormat.DateSeparator = "-";
            //Thread.CurrentThread.CurrentCulture = newCulture;
            //app.useswagger();
            //// enable middleware to serve swagger-ui (html, js, css, etc.),
            //// specifying the swagger json endpoint.
            //app.useswaggerui(c =>
            //{
            //    c.swaggerendpoint("/swagger/v1/swagger.json", "my api v1");
            //});

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().WithExposedHeaders("authorization"));
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
