using Swashbuckle.Swagger;
using System.Web.Http.Description;

namespace WebAPI.SwaggerFilters
{
    public class SwaggerFile : IOperationFilter
    {
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            operation.parameters.Add(new Parameter
            {
                type = "file",
                name = "upFile",
                required = false,
                @in = "formData",
                description = "The file to upload"
            });
            operation.consumes.Add("multipart/form-data");
        }
    }
}