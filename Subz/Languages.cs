namespace Subz;

public class Languages
{
	public record struct LanguageInfo (string LanguageCode, string LanguageName, string Country, string Flag);

	public static readonly IReadOnlyList<LanguageInfo> LanguageList =
	[
		new ("en", "English", "United States", "🇺🇸"),
		// new ("en-GB", "English", "United Kingdom", "🇬🇧"),
		new ("es", "Spanish", "Spain", "🇪🇸"),
		new ("fr", "French", "France", "🇫🇷"),
		new ("de", "German", "Germany", "🇩🇪"),
		new ("it", "Italian", "Italy", "🇮🇹"),
		new ("pt", "Portuguese", "Portugal", "🇵🇹"),
		new ("pt-BR", "Portuguese", "Brazil", "🇧🇷"),
		new ("tr", "Turkish", "Turkey", "🇹🇷"),
		new ("ru", "Russian", "Russia", "🇷🇺"),
		new ("ja", "Japanese", "Japan", "🇯🇵"),
		new ("zh-CN", "Chinese", "China", "🇨🇳"),
		new ("zh-TW", "Chinese new (Traditional)", "Taiwan", "🇹🇼"),
		new ("ko", "Korean", "South Korea", "🇰🇷"),
		new ("ar", "Arabic", "Saudi Arabia", "🇸🇦"),
		new ("nl", "Dutch", "Netherlands", "🇳🇱"),
		new ("sv", "Swedish", "Sweden", "🇸🇪"),
		new ("no", "Norwegian", "Norway", "🇳🇴"),
		new ("da", "Danish", "Denmark", "🇩🇰"),
		new ("pl", "Polish", "Poland", "🇵🇱"),
		new ("fi", "Finnish", "Finland", "🇫🇮"),
		new ("cs", "Czech", "Czech Republic", "🇨🇿"),
		new ("el", "Greek", "Greece", "🇬🇷"),
		new ("hi", "Hindi", "India", "🇮🇳"),
		new ("th", "Thai", "Thailand", "🇹🇭"),
		new ("vi", "Vietnamese", "Vietnam", "🇻🇳"),
		new ("id", "Indonesian", "Indonesia", "🇮🇩"),
		new ("uk", "Ukrainian", "Ukraine", "🇺🇦"),
		new ("ro", "Romanian", "Romania", "🇷🇴"),
		new ("hu", "Hungarian", "Hungary", "🇭🇺")
	];
}
