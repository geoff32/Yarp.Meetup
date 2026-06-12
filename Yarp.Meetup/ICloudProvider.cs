namespace Yarp.Meetup;

internal interface ICloudProvider
{
    Task<bool> IsAzureEnabledAsync();
}