﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using PrivilegeMobileService.Model;
using System.Collections.Specialized;
using PrivilegeCoreLibrary;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.Extensions.FileProviders;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace PrivilegeMobileService
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var loggerFactory = new LoggerFactory();
            loggerFactory.AddConsole();
            loggerFactory.AddDebug();
            // Add framework services.
            services.AddMvc(config =>
            {
                var policy = new AuthorizationPolicyBuilder()
                                 .RequireAuthenticatedUser()
                                 .Build();
                config.Filters.Add(new AuthorizeFilter(policy));
                config.Filters.Add(new ApiExceptionFilter(loggerFactory));
            });
            services.AddOptions();
            var jwtAppSettingOptions = Configuration.GetSection(nameof(JwtIssuerOptions));
            var dataStoreSettingOptions = Configuration.GetSection(nameof(DataStoreOptions));
            var imageStoreOptions = Configuration.GetSection(nameof(ImageStoreOptions));
            var secretKey = jwtAppSettingOptions[nameof(JwtIssuerOptions.SecretKey)];
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));
            services.Configure<JwtIssuerOptions>(options =>
            {
                options.Issuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                options.Audience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)];
                options.SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            });
            services.Configure<DataStoreOptions>(options =>
            {
                options.Host = dataStoreSettingOptions[nameof(DataStoreOptions.Host)];
                options.Port = dataStoreSettingOptions[nameof(DataStoreOptions.Port)];
                options.DatabaseName = dataStoreSettingOptions[nameof(DataStoreOptions.DatabaseName)];
                options.UserName = dataStoreSettingOptions[nameof(DataStoreOptions.UserName)];
                options.Password = dataStoreSettingOptions[nameof(DataStoreOptions.Password)];
            });
            services.Configure<ImageStoreOptions>(options =>
            {
                options.PathRoot = imageStoreOptions[nameof(ImageStoreOptions.PathRoot)];
                options.MaxFileSize = imageStoreOptions.GetValue<int>(nameof(ImageStoreOptions.MaxFileSize));
             });
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });
            services.AddAuthorization(options =>
            {
                options.AddPolicy("CustomerPolicy",
                                policy => policy.RequireClaim("RoleJa", "Customer"));
                options.AddPolicy("CompanyPolicy",
                                policy => policy.RequireClaim("RoleJa", "Company"));
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "PrivilegeAPI", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            var jwtAppSettingOptions = Configuration.GetSection(nameof(JwtIssuerOptions));
            var imageStoreOptions = Configuration.GetSection(nameof(ImageStoreOptions));
            var secretKey = jwtAppSettingOptions[nameof(JwtIssuerOptions.SecretKey)];
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)],

                ValidateAudience = true,
                ValidAudience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)],

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,
                
                RequireExpirationTime = false,
                ValidateLifetime = false,

                ClockSkew = TimeSpan.Zero
            };

            app.UseJwtBearerAuthentication(new JwtBearerOptions
            {
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                TokenValidationParameters = tokenValidationParameters
            });
            app.UseCors("CorsPolicy");
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(imageStoreOptions[nameof(ImageStoreOptions.PathRoot)]),
                RequestPath = new PathString("/images"),
                OnPrepareResponse = ctx =>
                {ctx.Context.Response.Headers.Append("Cache-Control", "public,max-age=600");}
            });
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUi(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "PrivilegeAPI V1");
            });
        }
    }
}
