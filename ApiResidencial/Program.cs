using Infrastructure.CrossCutting.IoC;
using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);


var services = builder.Services;
services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

builder.Services.AddControllersWithViews();

builder.Services.AddRazorPages();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();


services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "ResidenciaNet_API",
        Version = "v1",
        Description = "",
    });
        //c.AddSecurityDefinition(
        //            "oauth2",
        //            new OpenApiSecurityScheme
        //            {
        //                Type = SecuritySchemeType.OAuth2,
        //                Flows = new OpenApiOAuthFlows
        //                {
        //                    AuthorizationCode = new OpenApiOAuthFlow
        //                    {
        //                        AuthorizationUrl = new Uri("https://localhost:7270/connect/authorize"),
        //                        TokenUrl = new Uri("https://localhost:7270/connect/token"),
        //                        Scopes = new Dictionary<string, string> {
        //                            { "combitimeapi", "Demo API" }
        //                        }
        //                    }
        //                }
        //            });

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
    //EXENCIAL PARA ESCREVER NOS CONTROLLERS
    c.EnableAnnotations();
});

ConfigurationIoC.Configure(builder.Services);

var app = builder.Build();
app.UseCors();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("v2/swagger.json", "Minha API V1");

        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", " v1");
            c.InjectStylesheet("/swagger-ui/custom.css");
        });
    });
}

app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "apicontraktor v1");
    c.InjectStylesheet("/swagger-ui/custom.css");
    c.RoutePrefix = string.Empty;
}
);

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();