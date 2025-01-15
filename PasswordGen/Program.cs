using CommandLine;
using System.Text;

namespace PasswordGen;

public class Program {
    internal static string? GeneratedPassword { get; set; }
    internal static bool WasArgsNull { get; set; }
    private static ConsoleColor _defaultColor;

    public static void Main(string[]? args) {
        _defaultColor = Console.ForegroundColor;
        WasArgsNull = args is null || args.Length == 0;
        Parser.Default.ParseArguments<Options>(args ?? ["-l", "18", "-s", "true", "-w", "false"])
            .WithParsed(RunOptions)
            .WithNotParsed(HandleParseErrors);
    }


    /// <summary>
    /// Main code logic
    /// </summary>
    private static void RunOptions(Options opts) {
        if (opts.Length is not 0 && opts.Length < 15) {
            Console.WriteLine("Password needs to be 15 or more characters");
            return;
        }
        else {
            GeneratedPassword = opts.UseWordsInPassword ?
                ConvertWordWithL33t(GetRandomWords().Trim()) : // Trim ending spaces
                GetRandomStringFromChars(opts.Length);
        }

        if (opts.ShowLongOutput && !string.IsNullOrWhiteSpace(GeneratedPassword))
            FormatOutput(GeneratedPassword);
    }

    private static void FormatOutput(string password) {
        var list = new List<string>();
        foreach (var c in password) {
            if (GetNumberCharacterName(c) is not "NaN") {
                list.Add(GetNumberCharacterName(c));
                continue;
            }

            if (GetSpecialCharacterName(c) is not "UNKNOWN") {
                list.Add(GetSpecialCharacterName(c));
                continue;
            }

            list.Add((char.IsUpper(c) ? "uppercase" : "lowercase") + " " + c.ToString().ToUpper());
        }

        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(password);
        Console.ForegroundColor = _defaultColor;
        Console.WriteLine();
        Console.WriteLine($"({string.Join(", ", list)})");
        Console.WriteLine();
        if (WasArgsNull) {
            Console.Write("Press any key to exit...");
            Console.ReadKey();
        }
    }

    /// <summary>
    /// Parameter Error Parser
    /// </summary>
    private static void HandleParseErrors(IEnumerable<Error> errs) {
        var list = new List<string>();
        foreach (var error in errs) {
            if (error.Tag.HasFlag(ErrorType.HelpRequestedError))
                return;
            list.Add(error.ToString()!);
        }

        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Parameter Error");
        list.ForEach(Console.WriteLine);
        Console.ForegroundColor = _defaultColor;
        Console.WriteLine();
    }

    /// <summary>
    /// Returns a random string of a specified length
    /// </summary>
    /// <param name="length">Target length of randomized string</param>
    /// <returns>Random string of characters</returns>
    private static string GetRandomStringFromChars(int length = 16) =>
        new(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz!@#$%&*_-=+?", length)
            .Select(s => s[new Random().Next(s.Length)]).ToArray());

    /// <summary>
    /// Gets the word for the inputted special character
    /// </summary>
    /// <param name="c">single character</param>
    /// <returns>Word form of special character</returns>
    private static string GetSpecialCharacterName(char c) =>
        new Dictionary<char, string> {
            { '!', "EXCLAMATION" },
            { '@', "AT-SIGN" },
            { '#', "HASHTAG-POUND" },
            { '$', "DOLLAR-SIGN" },
            { '%', "PERCENT-SIGN" },
            { '&', "AMPERSAND" },
            { '*', "ASTERISK" },
            { '_', "UNDERSCORE" },
            { '-', "HYPHEN" },
            { '+', "PLUS" },
            { '=', "EQUAL" },
            // { '<', "LESS-THAN" }, // These break helpdesk
            // { '>', "GREATER-THAN" },
            { '?', "QUESTION-MARK" }
        }.TryGetValue(c, out var name) ? name : "UNKNOWN";

    /// <summary>
    /// Gets the word for of the inputted number
    /// </summary>
    /// <param name="c">single character</param>
    /// <returns>Word form of number</returns>
    private static string GetNumberCharacterName(char c) =>
        new Dictionary<char, string> {
            { '0', "ZERO" },
            { '1', "ONE" },
            { '2', "TWO" },
            { '3', "THREE" },
            { '4', "FOUR" },
            { '5', "FIVE" },
            { '6', "SIX" },
            { '7', "SEVEN" },
            { '8', "EIGHT" },
            { '9', "NINE" }
        }.TryGetValue(c, out var name) ? name : "NaN";

    private static readonly List<string> _passwordWordList = [
        "red", "blue", "green", "yellow", "purple", "orange", "pink", "brown", "black",
        "white", "gray", "cyan", "magenta", "maroon", "teal", "navy", "lavender",
        "peach", "salmon", "beige", "ivory", "coral", "turquoise", "amber", "gold",
        "silver", "bronze", "lime", "indigo", "crimson", "aqua", "emerald", "sapphire",
        "ruby", "charcoal", "khaki", "violet", "plum", "mustard", "ochre", "jade",
        "mint", "tangerine", "rose", "scarlet", "cobalt", "orchid", "sepia", "cerulean",
        "periwinkle", "ultramarine", "chartreuse", "fuchsia", "alabaster", "ebony",
        "sand", "mahogany", "cherry", "pearl", "cinnamon", "denim", "sky", "steel",
        "ash", "wheat", "brick", "moss", "olive", "sienna", "azure", "blush", "brick",
        "chocolate", "ocean", "sunset", "storm", "rust", "indigo", "amethyst",
        "banana", "bubblegum", "butter", "caramel", "cloud", "cream", "desert",
        "dusk", "flamingo", "forest", "garnet", "honey", "ivory", "lemon", "mocha",
        "papaya", "sage", "shadow", "snow", "topaz", "wine",
        "acorn", "blizzard", "canyon", "dolphin", "evergreen", "firefly", "garnet", "harvest",
        "ivy", "jasper", "kelp", "lotus", "mist", "nectarine", "opal", "pebble", "quokka",
        "reef", "sapphire", "tidal", "ultraviolet", "vortex", "willow", "xenia", "yarrow",
        "zen", "aspen", "birch", "cedar", "dahlia", "elm", "fern", "grape", "hibiscus",
        "iris", "juniper", "kiwi", "lavender", "maple", "nutmeg", "oak", "plum", "quince",
        "rosewood", "spruce", "tulip", "umbra", "violet", "walnut", "xanadu", "yellowstone",
        "zephyrine", "aster", "blossom", "cloud", "dew", "essence", "fountain", "grove",
        "horizon", "indigo", "jade", "knight", "lyric", "moss", "nectar", "obsidian",
        "prairie", "quintet", "ridge", "summit", "topaz", "utopia", "verdant", "wander",
        "xylophone", "yosemite", "zenobia", "avalanche", "beetle", "cascade", "drizzle",
        "ember", "fable", "glint", "hazel", "illumination", "jigsaw", "kaleidoscope",
        "lagoon", "meander", "nimbus", "onyx", "pinnacle", "quartzite", "raptor",
        "spark", "thistle", "ultra", "vista", "wren", "xerox", "yawn", "zenith",
        "apple", "banana", "cherry", "dragon", "eclipse", "falcon", "gadget", "horizon",
        "icicle", "jungle", "kangaroo", "lighthouse", "mountain", "nebula", "ocean",
        "panther", "quartz", "rainbow", "starlight", "tornado", "umbrella", "volcano",
        "whisper", "xenon", "yonder", "zephyr", "amber", "boulder", "crystal", "dandelion",
        "ember", "feather", "galaxy", "harbor", "island", "jewel", "krypton", "lantern",
        "meadow", "nectar", "onyx", "phoenix", "quiver", "raven", "sunset", "thunder",
        "unicorn", "velocity", "wanderer", "xylophone", "yacht", "zenith", "aurora",
        "beacon", "cascade", "dusk", "ember", "frost", "glacier", "harmony", "illusion",
        "journey", "karma", "legacy", "mirage", "nocturne", "odyssey", "paradox",
        "quasar", "resonance", "serenade", "tranquil", "uplift", "vista", "whimsy",
        "xenith", "yield", "alpha", "beta", "gamma", "delta", "omega", "epsilon",
        "zeta", "theta", "iota", "kappa", "lambda", "mu", "nu", "xi", "omicron", "pi",
        "rho", "sigma", "tau", "upsilon", "phi", "chi", "psi", "omega", "asteroid",
        "comet", "equinox", "nebula", "pulsar", "quasar", "solstice", "blackhole",
        "stardust", "cosmos", "nova", "lunar", "solar", "orbit", "eclipse"
        ];

    private static Dictionary<char, char> charToL33t => new Dictionary<char, char>() {
        { 'a', '4' },
        { 'b', '8' },
        { 'e', '3' },
        { 'i', '1' },
        { 'l', '1' },
        { 'o', '0' },
        { 't', '7' }
    };

    /// <summary>
    /// Converts each character in a string with a chance of becoming a number or special character associated with that character
    /// </summary>
    private static string ConvertWordWithL33t(string words) {
        var charBuilder = new StringBuilder();
        for (var i = 0; words.Length > 0; i++) {
            var random = new Random().Next(0, 4); // 25%
            var _5050 = new Random().Next(0, 1);  // 50%
            var _33 = new Random().Next(0, 2);    // 33.3334%

            if (random is 3) {
                charToL33t.TryGetValue((char)i, out var newChar);
                charBuilder.Append(newChar);
            }
            else if (_5050 is 0 && i is 's' or 'S' && random is 3) {
                charBuilder.Append('$');
            }
            else if (_5050 is 1 && i is 's' or 'S' && random is 3) {
                charBuilder.Append('5');
            }
            else if (_33 is 0 && i is ' ' && random is 3) {
                charBuilder.Append('-');
            }
            else if (_33 is 1 && i is ' ' && random is 3) {
                charBuilder.Append('_');
            }
            else
                charBuilder.Append(i);
        }

        return charBuilder.ToString();
    }

    /// <summary>
    /// Gets three random words, if the length is still less than 16 characters, it will continue to add words until it is long enough
    /// </summary>
    private static string GetRandomWords() {
        var thePassword = string.Empty;

        var run = 0;
        foreach (var word in _passwordWordList) {
            if (run >= 3 || thePassword.Length < 15) break;
            thePassword += new Random().Next(0, _passwordWordList.Count);
            thePassword += " ";
            thePassword += new Random().Next(0, _passwordWordList.Count);
            thePassword += " ";
            thePassword += new Random().Next(0, _passwordWordList.Count);
            thePassword += " ";
            run++;
        }

        return thePassword;
    }
}

internal class Options {
    [Option('l', "length",
        Default = 16,
        Required = false,
        HelpText = "Defines the length of the generated password")]
    public int Length { get; set; }

    [Option('s', "showLongOutput",
        Default = true,
        Required = false,
        HelpText = "Shows the explanation of each character")]
    public bool ShowLongOutput { get; set; }

    [Option('w', "useWords",
        Default = false,
        Required = false,
        HelpText = "Defines if you would like to generate a password using words rather than random characters")]
    public bool UseWordsInPassword { get; set; }
}