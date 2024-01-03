using System.Text.RegularExpressions;

using L5.Utils.Errors;
using L5.Main;

namespace L5;

public partial class Lexer {
    private Compiler compiler;
    public class Token {
        public string type = "";
        public string value = "";
        public int start;
        public int end;

        public Token(string type, string value, int start, int end) {
            this.type = type;
            this.value = value;
            this.start = start;
            this.end = end;
        }

        public override string ToString() {
            return $"Token({type}, \"{value}\", {start}, {end})";
        }
    }

    public Lexer(Compiler c) {
        compiler = c;
    }

    [GeneratedRegex("[^0-9\\.]")]
    private static partial Regex NumRegex();
    [GeneratedRegex("[^0-9]")]
    private static partial Regex IntRegex();
    [GeneratedRegex("(?<!\\\\)'")]
    private static partial Regex CharRegex();
    [GeneratedRegex("(?<!\\\\)\"")]
    private static partial Regex StrRegex();
    [GeneratedRegex("\\s")]
    private static partial Regex SpaceRegex();

    private string[] keywords = {
        "if",
        "return",
        "to",
        "out",
        "goto",
        "goget",
        "input",
        "as",
        "call"
    };

    private Dictionary<string, string> SymbolTokens {
        get {
            return new() {
                {"index", "@"},
                {"range", "$"},
                {"colon", ":"},
                {"comma", ","},
                {"lparen", "("},
                {"rparen", ")"},
                {"lbrace", "{"},
                {"rbrace", "}"},
                {"lbracket", "["},
                {"rbracket", "]"},
                {"plus", "+"},
                {"minus", "-"},
                {"star", "*"},
                {"divide", "/"},
                {"mod", "%"},
                {"pow", "^"},
                {"assign", "="},
                {"equals", "=="},
                {"not", "!"},
                {"lt", "<"},
                {"gt", ">"},
                {"and", "&"},
                {"or", "|"},
                {"dot", "."},
                {"question", "?"},
                {"backslash", "\\"}
            };
        }
    }

    private string bucket(string s, int i, Regex stop) {
        return stop.Split(s.Substring(i))[0];
    }

    public Token[][] Lex(string s) {
        List<Token[]> tokenList = new();
        List<Token> curLine = new();

        string file = Path.GetFileName(compiler.file);

        for (int i = 0; i < s.Length; i++) {
            if (s[i] == '\n') {
                // Newlines --> Add current line to tokenList and clear curLine
                tokenList.Add(curLine.ToArray<Token>());
                curLine.Clear();
            } else if (char.IsDigit(s[i])) {
                // Numbers

                string val = bucket(s, i, NumRegex());
                MalfromedToken err = new(file, s, i, i + val.Length);
                if (val.Count(c => c == '.') > 1) {
                    err.Raise("numbers cannot have more than one decimal point");
                }
                if (val[^1] == '.') {
                    err.Raise("numbers cannot end with a decimal point");
                }
                if (!err.raised) {
                    curLine.Add(new Token("num", val, i, i + val.Length));
                }
                i += val.Length-1;
            } else if (s[i] == '\'') {
                // Char literals

                string val = bucket(s, i+1, CharRegex());
                if (val.Length != 1) {
                    new MalfromedToken(file, s, "char literals must be one character long", i, i + val.Length+1).Raise();
                } else {
                    curLine.Add(new Token("char", val[..1], i, i + val.Length+2));
                }
                i += val.Length + 1;
            } else if (char.IsLetter(s[i])) {
                // Keywords

                string val = bucket(s, i, SpaceRegex());
                if (keywords.Contains(val)) {
                    curLine.Add(new Token("keyword", val, i, i + val.Length));
                } else {
                    new MalfromedToken(file, s, $"{val} is not a recognised keyword", i, i + val.Length-1).Raise();
                }
                i += val.Length-1;
            } else if (s[i] == '"') {
                // String literals

                string val = bucket(s, i+1, StrRegex());
                curLine.Add(new Token("str", val, i, i + val.Length+2));
                i += val.Length + 1;
            } else {
                string type = "";
                string val = "";
                for (int j = 0; j < SymbolTokens.Count; j++) {
                    type = SymbolTokens.Keys.ElementAt(j);
                    val = SymbolTokens.Values.ElementAt(j);
                    if (s[i..].StartsWith(val)) {
                        curLine.Add(new Token(type, val, i, i + val.Length));
                        break;
                    }
                }
                if (type == "") {
                    new MalfromedToken(file, s, $"`{s[i]}` is not the beginning of any known token", i, i + 1).Raise();
                }
            }
        }
        tokenList.Add(curLine.ToArray<Token>());
        return tokenList.ToArray<Token[]>();
    }
}
