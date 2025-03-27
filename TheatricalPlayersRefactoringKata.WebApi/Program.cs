using TheatricalPlayersRefactoringKata.Application.DependencyInjection;
using TheatricalPlayersRefactoringKata.WebApi.Extensions;
using TheatricalPlayersRefactoringKata.WebApi.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDependencyApplicationSetup(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddEssentials();

builder.Services.AddVersionedApiExplorer(options =>
{
    // Agrupar por numero de versao
    // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
    // note: the specified format code will format the version as "'v'major[.minor][-status]"
    options.GroupNameFormat = "'v'VVV";

    // Necessario para o correto funcionamento das rotas
    // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
    // can also be used to control the format of the API version in route templates
    options.SubstituteApiVersionInUrl = true;
});

var app = builder.Build();
app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseRouting();
app.ConfigureSwagger();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHealthChecks("/health");
    endpoints.MapHealthChecks("/ready");
});

app.Run();
