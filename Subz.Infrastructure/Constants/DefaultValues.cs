// ReSharper disable InconsistentNaming
namespace Subz.Infrastructure.Constants;

public static class DefaultValues
{
    public const string DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
    public const int EmptyForInt = -1;
    public const string EmptyForString = "";
    
    public const string HealthCheck_Liveness = "/liveness";
    public const string HealthCheck_Readiness = "/readiness";
    public const string HealthCheck_ThirdParty = "/thirdPartyHealthCheck";
    
}
