using StackExchange.Redis;

namespace Yarp.Meetup;

internal class CloudProvider(IConnectionMultiplexer connectionMultiplexer, ILogger<CloudProvider> logger) : ICloudProvider
{
    public async Task<bool> IsAzureEnabledAsync()
    {
        try
        {
            var database = connectionMultiplexer.GetDatabase();

            return await database.StringGetAsync("IsAzureEnabled") == "true";
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to load switch on redis");
            return false;
        }
    }
}
