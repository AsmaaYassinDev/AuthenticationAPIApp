using AuthenticationAPIApp;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(config)
    .Enrich.FromLogContext()
    .CreateLogger();


builder.Host.UseSerilog();

var startup = new Startup(builder.Configuration);
startup.RegisterServices(builder.Services);

var app = builder.Build();
startup.SetupMiddlewares(app, builder.Environment);
