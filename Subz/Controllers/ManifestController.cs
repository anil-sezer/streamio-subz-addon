using Microsoft.AspNetCore.Mvc;
using Subz.Infrastructure.ConstantsAndEnums;

namespace Subz.Controllers;

[ApiController]
public class ManifestController : ControllerBase
{
    [HttpGet]
    [Route(RouteConsts.Manifest)]
    public IActionResult GetManifest()
    {
        Log.Information("Manifest requested");
        var manifest = new
        {
            id = "com.subz.opensubs",
            version = "1.0.0",
            name = "Subz",
            description = "Provides subtitles. Currently only from OpenSubtitles.org",
            logo = "https://raw.githubusercontent.com/anil-sezer/streamio-subz-addon/refs/heads/prod/Subz/wwwroot/full-logo.png",
            resources = new[] { "subtitles" },
            types = new[] { "movie", "series", "anime", "other" },
            idPrefixes = new[] { "tt" },
            catalogs = new object[] {},
            // behaviorHints = new { adult = true }
        };

        return new JsonResult(manifest);
    }
}
