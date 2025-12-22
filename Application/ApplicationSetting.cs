using Application.Ports;
using Application.Usecases.ExampleUsecase;
using FakeApiInfrastructure;
using InternalHttpClientInfrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

/* ********************************************************************************************************          
# * Copyright © 2026 Arify Labs - All rights reserved.   
# * 
# * Info                  : System API Template.
# *
# * By                    : Victor Jhampier Caxi Maquera
# * Email/Mobile/Phone    : victorjhampier@gmail.com | 968991*14
# *
# * Creation date         : 03/08/2026
# * 
# * Docs for json Ignore
# * https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/ignore-properties
# **********************************************************************************************************/

namespace Application;

public static class ApplicationSetting
{
    public static void AddApplicationServices(this IServiceCollection services, IConfiguration configuration, bool isDevelopment)
    {
        // Added Infrstrutures
        services.AddInternalHttpClientConnector();
        services.AddFakeApiInfrastructure();

        //Dependency inyection        
        services.AddTransient<IExamplePort, ExampleCase>();
    }
}
