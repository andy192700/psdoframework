using DoFramework.Mappers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PSDoFramework.Tool;
using System.Diagnostics;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureHostConfiguration((hostConfiguration) =>
    {
        hostConfiguration.AddJsonFile("appsettings.json");
    })
    .ConfigureServices((hostContext, services) =>
    {
        services.AddSingleton<IDoCLI, DoCLI>();
        services.AddSingleton<IMapper<string[], Process>, ProcessMapper>();
        services.Configure<PowerShellSettings>(hostContext.Configuration.GetSection("PowerShellSettings"));
    });

var app = host.Build();

var cli = app.Services.GetService<IDoCLI>();

cli!.Exec(args);
