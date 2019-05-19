using System.Web.Http;
using WebActivatorEx;
using WebAPI;
using Swashbuckle.Application;
using WebAPI.SwaggerFilters;
using System;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace WebAPI
{
    public class SwaggerConfig
    {
        public static void Register()
        {
            var thisAssembly = typeof(SwaggerConfig).Assembly;

            GlobalConfiguration.Configuration
                .EnableSwagger(c =>
                {
                    c.OperationFilter<SwaggerAuthorizationFilter>();
                    c.OperationFilter<SwaggerFile>();
                    c.DocumentFilter<SwaggerToken>();

                    c.SingleApiVersion("v1", "Album Web Api");

                    // You can use "BasicAuth", "ApiKey" or "OAuth2" options to describe security schemes for the API.
                    // See https://github.com/swagger-api/swagger-spec/blob/master/versions/2.0.md for more details.
                    // NOTE: These only define the schemes and need to be coupled with a corresponding "security" property
                    // at the document or operation level to indicate which schemes are required for an operation. To do this,
                    // you'll need to implement a custom IDocumentFilter and/or IOperationFilter to set these properties
                    // according to your specific authorization implementation
                    //
                    //c.BasicAuth("basic")
                    //    .Description("Basic HTTP Authentication");
                    //
                    // NOTE: You must also configure 'EnableApiKeySupport' below in the SwaggerUI section
                    //c.ApiKey("apiKey")
                    //    .Description("API Key Authentication")
                    //    .Name("apiKey")
                    //    .In("header");
                    //
                    //c.OAuth2("oauth2")
                    //    .Description("OAuth2 Implicit Grant")
                    //    .Flow("implicit")
                    //    .AuthorizationUrl("http://petstore.swagger.wordnik.com/api/oauth/dialog")
                    //    //.TokenUrl("https://tempuri.org/token")
                    //    .Scopes(scopes =>
                    //    {
                    //        scopes.Add("read", "Read access to protected resources");
                    //        scopes.Add("write", "Write access to protected resources");
                    //    });

                    c.IncludeXmlComments(GetXmlCommentsPath());
                    c.DescribeAllEnumsAsStrings();
                })
                .EnableSwaggerUi(c =>
                {
                    c.DocumentTitle("Album WebApi");
                });
        }

        private static string GetXmlCommentsPath()
        {
            return AppDomain.CurrentDomain.BaseDirectory + @"App_Data\WebAPI.xml";
        }
    }
}
