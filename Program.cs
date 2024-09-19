using agit.Api.Master;
using System.Text;
using agit.Api.Connection;
using agit.Api.Helpers;
using agit.Api.Interface;
using agit.Api.Middlewares;
using agit.Api.Repossitory;
using agit.Api.Usecase;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var appName = Basic_configuration.Get_variable_global("APP_NAME");
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// START setup dokumntasi swagger
setSwaggerDoc(builder.Services, appName);
// END setup dokumntasi swagger

// START add costom logging  
setup_custom_logging(builder);
// END add costom logging   


// START register sigleton class / entity   
builder.Services.AddSingleton<Basic_logger>();
builder.Services.AddSingleton<Basic_configuration>();
builder.Services.AddSingleton<Basic_response>();
// END register sigleton class / entity    

// Start config DB
builder.Services.AddSingleton<Basic_database>();
// END config DB

// START auto register controller
builder.Services.AddControllers().ConfigureApiBehaviorOptions(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});
// END auto register controller

builder.Services.AddRazorPages();

// START auto register usecase dan repository
builder.Services.AddScoped<IN_use_production, Use_production>();
builder.Services.AddScoped<IN_rep_production, Rep_production>();
// END auto register usecase dan repository

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint($"/swagger/{appName}_DEFAULT_SWAGGER/swagger.json", $"{appName} API V1 lah");
    });
}


app.UseHttpsRedirection();

// START add config auto logger request - response
app.UseMiddleware<Mid_base_logger>();
// END add config auto logger request - response


// START config CORS
app.UseCors(options =>
{
    options.AllowAnyOrigin();
    options.AllowAnyMethod();
    options.AllowAnyHeader();
});
// END config CORS

app.MapGet("/ping", () => new
{
    message = "ping!!!",
    month = "Server Actived!!",
    status = "online"
});

app.MapControllers();
app.Run();



void setSwaggerDoc(IServiceCollection services, string appName)
{
    services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc($"{appName}_DEFAULT_SWAGGER",
            new OpenApiInfo { Title = $"{appName}_SWAGGER", Version = "v1" });
        c.EnableAnnotations();
        c.MapType<string>(() => new OpenApiSchema { Type = "string" });
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter the token in the field provided.",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = string.Empty
        });

        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[] { }
            }
        });
    });
}

void setup_custom_logging(WebApplicationBuilder builder)
{
    // START add costom logging    
    builder.Services.AddHttpLogging(logging =>
    {
        // Customize HTTP logging here.
        logging.LoggingFields = HttpLoggingFields.All;
        logging.RequestHeaders.Add("sec-ch-ua");
        logging.ResponseHeaders.Add("my-response-header");
        logging.MediaTypeOptions.AddText("application/json");
        logging.RequestBodyLogLimit = 4096;
        logging.ResponseBodyLogLimit = 4096;
    });

    var enable_log_console =
        Helper.Hel_convert_string_to_bool(Basic_configuration.Get_variable_global("ENABLE_LOG_CONSOLE"));
    var logger_configuration = new LoggerConfiguration()
        .MinimumLevel.Information()
        .WriteTo.File($"Logs/{appName}.log", rollingInterval: RollingInterval.Day, encoding: Encoding.UTF8);

    if (enable_log_console) logger_configuration.WriteTo.Console();

    var logger = logger_configuration.CreateLogger();
    builder.Logging.ClearProviders();
    builder.Services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(logger));
    // END add costom logging
}
