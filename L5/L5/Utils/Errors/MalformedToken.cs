namespace L5.Utils.Errors;

public class MalfromedToken : Error
{
    public MalfromedToken(string file, string s, string message, int start, int end) : base(file, s, message, start, end) {}
    public MalfromedToken(string file, string s, int start, int end) : base(file, s, start, end) {}
    public MalfromedToken(string file, string message) : base(file, message) {}
    public MalfromedToken(string file) : base(file) {}
    public MalfromedToken() : base() {}
}
