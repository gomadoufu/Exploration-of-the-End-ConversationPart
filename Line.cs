using System;

public class Line
{
    //各行のデータを保持する(話者_内容)
    public string text { get; set; }

    //話者を保持する
    public string name { get; set; }

    //内容を保持する
    public string say { get; set; }

    //疑問文かどうか
    public bool isQuestion { get; set; }

    //答えの開始かどうか
    public bool isOneOfTheAns { get; set; }

    //空行かどうか
    public bool isBlank { get; set; }

    public Line(string text)
    {
        this.text = text;
        this.name = "";
        this.say = "";
        this.isQuestion = false;
        this.isOneOfTheAns = false;
        this.isBlank = false;
    }
}
