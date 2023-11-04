namespace L5;

public class Error {
    private string message = "";
    private int start;
    private int end;
    private int line;
    private string sample = "";

    private int countIndex;
    private bool LineBeforeStart(char c) {
        return countIndex <= start && c == '\n';
    }

    public Error(string s, int start = -1, int end = -1, string message = "") {
        this.message = message;
        this.start = start;
        this.end = end;
        line = s.Count(LineBeforeStart) + 1;
        sample = s.Split('\n')[line];
    }

    public void Raise(int start = -1, int end = -1, string message = "") {
        if (!this.message.Equals("")) {
            if (!message.Equals("")) {
                throw new Exception("A message is required to raise an error");
            } else {
                this.message = message
            }
        }
        
    }
}
