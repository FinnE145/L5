using L5;

string s = @"abcdefg
hijklmnop
qrs
tuv
wxy&z".Replace("\r", "");
Console.WriteLine(s);

new Error("test.l5", "there was an error").Raise("newfile.l5", s, 8, 10);