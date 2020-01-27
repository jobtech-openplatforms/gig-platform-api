using System.Reflection;
using Jobtech.OpenPlatforms.GigDataCommon.Library.Messages;
using Jobtech.OpenPlatforms.GigPlatformApi.EventDispatcher.Managers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Rebus.Config;
using Rebus.Retry.Simple;
using Rebus.Routing.TypeBased;
using Rebus.Serialization.Json;
using Rebus.ServiceProvider;

namespace Jobtech.OpenPlatforms.GigPlatformApi.EventDispatcher.IoC
{
    public static class EventDispatcherServiceExtension
    {

        public static IServiceCollection AddEventDispatcher(this IServiceCollection collection,
            IConfiguration configuration)
        {

            collection.AddTransient<IPlatformDispatchManager, PlatformDispatchManager>();

            var serviceBusConnectionString = configuration.GetConnectionString("ServiceBus");

            collection.AddRebus(c =>
                    c
                        .Transport(t =>
                            t.UseAzureServiceBus(
                                serviceBusConnectionString,
                                "gigplatformapi.input"))
                        .Options(o =>
                        {
                            o.SimpleRetryStrategy(errorQueueAddress: "gigplatformapi.error",
                                secondLevelRetriesEnabled: true);
                        })
                        .Routing(r => r.TypeBased()
                            .Map<PlatformUserUpdateDataMessage>("platformdatafetcher.input"))
                        .Serialization(
                            s =>
                            {
                                var jsonSettings = new JsonSerializerSettings
                                {
                                    TypeNameHandling = TypeNameHandling.None,
                                    //ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
                                    ContractResolver = new PrivateResolver()
                                };

                                s.UseNewtonsoftJson(jsonSettings);
                            })

                );

            return collection;
        }
    }

    public class PrivateResolver : DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member
            , MemberSerialization memberSerialization)
        {
            var prop = base.CreateProperty(member, memberSerialization);

            if (!prop.Writable)
            {
                var property = member as PropertyInfo;
                if (property != null)
                {
                    var hasPrivateSetter = property.GetSetMethod(true) != null;
                    prop.Writable = hasPrivateSetter;
                }
            }

            return prop;
        }
    }
}
