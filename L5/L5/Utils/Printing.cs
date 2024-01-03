namespace L5.Utils;

public class Printing
{
    public static void PrintTokenArray(Lexer.Token[] tokens, string sep = "")
    {
        int i = 0;
        foreach (Lexer.Token token in tokens)
        {
            Console.Write(string.Format(sep, i));
            Console.WriteLine(token);
            i++;
        }
    }

    public static void PrintTokenArrays(Lexer.Token[][] tokenArrays, string outerSep = "----\n", string innerSep = "")
    {
        int i = 0;
        foreach (Lexer.Token[] tokenArray in tokenArrays)
        {
            Console.Write(string.Format(outerSep, i));
            PrintTokenArray(tokenArray, innerSep);
            i++;
        }
    }
}
