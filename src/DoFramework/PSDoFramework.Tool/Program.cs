using DoFramework.Mappers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PSDoFramework.Tool;
using System.Diagnostics;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddSingleton<IDoCLI, DoCLI>();
        services.AddSingleton<IMapper<string[], Process>, ToolingArgMapper>();
    });

var app = host.Build();

var cli = app.Services.GetService<IDoCLI>();

cli!.Exec(args);
