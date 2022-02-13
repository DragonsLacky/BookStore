namespace Web.Middleware;

public class AuthorizationHeaderParameterOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        operation.Security ??= new List<OpenApiSecurityRequirement>();


        var scheme = new OpenApiSecurityScheme
            {Reference = new OpenApiReference {Type = ReferenceType.SecurityScheme, Id = "bearer"}};
        operation.Security.Add(new OpenApiSecurityRequirement
        {
            [scheme] = new List<string>()
        });
    }
}