using System.Diagnostics.CodeAnalysis;
using Yarp.ReverseProxy;
using Yarp.ReverseProxy.Model;

namespace Yarp.Meetup;

internal class AzureSwitchMiddleware(
    RequestDelegate next,
    IProxyStateLookup proxyStateLookup,
    ICloudProvider cloudProvider
    )
{
    private const string AzureRouteKey = "AzureRoute";

    public async Task InvokeAsync(HttpContext context)
    {
        if (await cloudProvider.IsAzureEnabledAsync())
        {
            var routeModel = context.GetRouteModel();
            if (TryGetAzureRoute(routeModel, out var azureRoute)
                && azureRoute.Cluster is not null)
            {
                context.ReassignProxyRequest(azureRoute, azureRoute.Cluster);
            }
        }

        await next(context);
    }

    private bool TryGetAzureRoute(RouteModel routeModel, [NotNullWhen(true)]out RouteModel? azureRoute)
    {
        azureRoute = null;

        return routeModel.Config.Metadata is not null
            && routeModel.Config.Metadata.TryGetValue(AzureRouteKey, out var azureRouteId)
            && proxyStateLookup.TryGetRoute(azureRouteId, out azureRoute);
    }
}
