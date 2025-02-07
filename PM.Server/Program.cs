using PM.Application;
using PM.Infrastructure;
using PM.WebApi;
using PM.WebApi.Common.Mapping;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services
        .AddMappings()
        .AddPresentation()
        .AddApplication()
        .AddInfrastructure(builder.Configuration);
}

var app = builder.Build();
{
    app.UseCors(policy => policy
        .WithOrigins("https://localhost:4200")
        .AllowAnyMethod()
        .AllowAnyHeader());

    app.UseExceptionHandler("/error");

    // Enable middleware to serve generated Swagger as a JSON endpoint and Swagger UI
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "PM API V1");
    });

    app.UseHttpsRedirection();
    app.UseDatabaseInitialization();
    app.MapControllers();
    app.Run();
}
