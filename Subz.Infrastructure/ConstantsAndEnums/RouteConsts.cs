namespace Subz.Infrastructure.ConstantsAndEnums;

public static class RouteConsts
{
    public const string Api = "api";
    public const string SubtitleSearchRoute = $"{Api}/subtitles";
    public const string SubtitleServeRoute = Api+"/download-subtitle";
    public const string Manifest = $"{Api}/manifest.json";
    
    public const string FileId = "fileId";
}
