using System.Globalization;
using System.Text.RegularExpressions;

namespace L5;

public class Lexer {
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
    }

    /* static bool IsDoubleQuote(char c) {
        return c == '"';
    }

    static bool IsSingleQuote(char c) {
        return c == '\'';
    } */

    static Token LexNums(string s, int i) {
        return new Token("int", "123", 0, 3);
    }

    static Token LexStr(string s, int i) {
        return new Token("str", "abc", 0, 3);
    }

    static Token LexChar(string s, int i) {
        return new Token("char", "a", 0, 3);
    }

    /* private Dictionary<Regex, Func<string[], int, int, Token>> tokenFlags = new Dictionary<Regex, Func<string[], int, int, Token>>() {
        {new Regex("[0-9]"), Lexer.LexNums},
        {new Regex("\""), Lexer.LexStr},
        {new Regex("'"), Lexer.LexChar}
    }; */

    private string bucket(string s, int i, Regex stop) {
        return stop.Split(s.Substring(i))[0];
    }

    public Token[] Lex(string s) {
        List<Token> tokenList = new List<Token>();

        for (int i = 0; i < s.Length; i++) {
            if (char.IsDigit(s[i])) {
                string val = bucket(s, i, new Regex("[^0-9\\.]"));
                tokenList.Add(new Token("num", val, i, i + val.Length));
            } else if (s[i] == '\'') {
                string val = bucket(s, i, new Regex("(?<!\\\\)\\'"));
                if (val.Length > 1) {
                    //Error
                }
                tokenList.Add(new Token("char", val.Remove('\''), i, i + val.Length));
            }
        }

        return tokenList.ToArray<Token>();
    }
}
