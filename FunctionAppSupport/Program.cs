using System;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MediatR;
using FunctionAppSupport.Data;

namespace FunctionAppSupport;

public class Program
{
    public static void Main()
    {
        var host = new HostBuilder()
            .ConfigureFunctionsWorkerDefaults()
            .ConfigureOpenApi()
            .ConfigureServices(services =>
            {
                services.AddDbContext<SupportContext>(
                    options => options.UseSqlServer(
                        Environment.GetEnvironmentVariable("DBSupport")));
                services.AddMediatR(typeof(Program));
            })
            .Build();

        host.Run();
    }
}