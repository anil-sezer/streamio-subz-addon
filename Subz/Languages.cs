namespace Subz;

public class Languages
{
	public record struct LanguageInfo (string alpha2Code, string alpha3Code, string LanguageName, string Country, string Flag);

	public static readonly IReadOnlyList<LanguageInfo> LanguageList =
	[
		new ("en", "eng", "English", "United States", "🇺🇸"),
		new ("es", "spa", "Spanish", "Spain", "🇪🇸"),
		new ("fr", "fra", "French", "France", "🇫🇷"),
		new ("de", "deu", "German", "Germany", "🇩🇪"),
		new ("it", "ita", "Italian", "Italy", "🇮🇹"),
		new ("pt", "por", "Portuguese", "Portugal", "🇵🇹"),
		new ("pt-BR", "por", "Portuguese", "Brazil", "🇧🇷"),
		new ("tr", "tur", "Turkish", "Turkey", "🇹🇷"),
		new ("ru", "rus", "Russian", "Russia", "🇷🇺"),
		new ("ja", "jpn", "Japanese", "Japan", "🇯🇵"),
		new ("zh-CN", "zho", "Chinese", "China", "🇨🇳"),
		new ("zh-TW", "zho", "Chinese new (Traditional)", "Taiwan", "🇹🇼"),
		new ("ko", "kor", "Korean", "South Korea", "🇰🇷"),
		new ("ar", "ara", "Arabic", "Saudi Arabia", "🇸🇦"),
		new ("nl", "nld", "Dutch", "Netherlands", "🇳🇱"),
		new ("sv", "swe", "Swedish", "Sweden", "🇸🇪"),
		new ("no", "nor", "Norwegian", "Norway", "🇳🇴"),
		new ("da", "dan", "Danish", "Denmark", "🇩🇰"),
		new ("pl", "pol", "Polish", "Poland", "🇵🇱"),
		new ("fi", "fin", "Finnish", "Finland", "🇫🇮"),
		new ("cs", "ces", "Czech", "Czech Republic", "🇨🇿"),
		new ("el", "ell", "Greek", "Greece", "🇬🇷"),
		new ("hi", "hin", "Hindi", "India", "🇮🇳"),
		new ("th", "tha", "Thai", "Thailand", "🇹🇭"),
		new ("vi", "vie", "Vietnamese", "Vietnam", "🇻🇳"),
		new ("id", "ind", "Indonesian", "Indonesia", "🇮🇩"),
		new ("uk", "ukr", "Ukrainian", "Ukraine", "🇺🇦"),
		new ("ro", "ron", "Romanian", "Romania", "🇷🇴"),
		new ("hu", "hun", "Hungarian", "Hungary", "🇭🇺")
	];
	
	public static string GetLanguageAlpha3CodeFromName(string languageName)
	{
		var language = LanguageList.FirstOrDefault(x => x.LanguageName == languageName);
		return language.alpha3Code;
	}
}