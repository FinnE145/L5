namespace L5;

public class Error {
    public string file = "";
    public string message = "";
    public int start = -1;
    public int end = -1;
    public int line = -1;
    public int lineOffset = -1;
    public string sample = "";

    private void GetLineNumber(string s, int start, out int line, out int lineOffset) {
        line = 0;
        lineOffset = 0;

        for (int cc = 0; cc < start; cc++) {
            Console.WriteLine($"At char {(s[cc] == '\n' ? "<nl>" : s[cc])}");
            if (s[cc] == '\n') {
                line++;
                lineOffset = cc;
                Console.WriteLine($"Moved to line {line}");
            }
        }
    }

    public Error(string file, string s, string message, int start, int end) {
        this.file = file;
        this.message = message;
        this.start = start;
        this.end = end;
        GetLineNumber(s, start, out line, out lineOffset);
        sample = s.Split('\n')[line];
    }

    public Error(string file, string s, int start, int end) {
        this.file = file;
        this.start = start;
        this.end = end;
        GetLineNumber(s, start, out line, out lineOffset);
        sample = s.Split('\n')[line];
    }

    public Error(string file, string message) {
        this.file = file;
        this.message = message;
    }

    public Error(string file) {
        this.file = file;
    }

    private void RaiseArgs(string file, int line, int lStart, int lEnd, string message, string sample) {
        line += 1;
        lStart += line == 1 ? 1 : 0;
        lEnd += line == 1 ? 1 : 0;
        Console.WriteLine($"{this.GetType().Name} in {file}: {message}");
        Console.WriteLine($"Line {line}: {lStart}-{lEnd}> {sample}");
        Console.WriteLine($"{new string(' ', lStart + line.ToString().Length + lStart.ToString().Length + lEnd.ToString().Length + 9)}{new string('^', lEnd-lStart+1)}");
    }

    private void RaiseWithLocalArgs(string file, string s, int line, int start, int end, string message) {
        if (line == -1 || lineOffset == -1) {
            GetLineNumber(s, start, out line, out lineOffset);
        }
        string sample = s.Split('\n')[line];
        RaiseArgs(file, line, start-lineOffset, end-lineOffset, message, sample);
    }

    public void Raise(string file, string s, int start, int end, string message = "") {
        line = -1;
        RaiseWithLocalArgs(file, s, line, start, end, message);
    }

    public void Raise(string s, int start, int end, string message = "") {
        line = -1;
        RaiseWithLocalArgs(file, s, line, start, end, message);
    }

    public void Raise(string message) {
        RaiseArgs(file, line, start-lineOffset, end-lineOffset, message, sample);
    }

    public void Raise() {
        RaiseArgs(file, line, start-lineOffset, end-lineOffset, message, sample);
    }
}
