//TextEdit.cs
//Created by gomadoufu
//Created on 2022/5/4
//Last modified on 2022/5/4
//テキストファイルを適切なLineオブジェクトに変換するクラス
//@param string text(テキストファイルのデータ)
//
/* TODO: 仕様が固まったら、テキストの処理を全てやってから最後にLineオブジェクトを作る
ようにする。そうすると、Lineオブジェクトの各プロパティをReadOnlyにすることができるハズ。*/
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TextEdit
{
    //テキストファイルから読み込んだ文章全体
    //後から代入不可
    public string rawtext { get; private set; }

    //読み込んだ文章を行ごとに分けて作ったLineオブジェクトのList
    private List<Line> Lines_list = new List<Line>();

    //コンストラクタ。rawtextに、投げられたテキストファイルの文章を全部つっこむ
    public TextEdit(string text)
    {
        this.rawtext = text;
    }

    //各行を受け取り、Lineオブジェクトを生成して、Lines_listに追加する
    public void SetLine(string line)
    {
        this.Lines_list.Add(new Line(line));
    }

    //Lines_listを吐き出すゲッター
    public List<Line> GetLinesList()
    {
        return this.Lines_list;
    }

    /*
    textRead
    外部ファイルのテキストデータを一括で取得するクラスメソッド。コンストラクタと同じもの。
    テキストファイルから文字列を引き出したいときに使う。
    テキストマネージャ以外でも使う可能性ありなので、インスタンスを作らなくても使えるようにしておく。
    @param string
    @return string
    */
    public static string TextRead(string path)
    {
        StreamReader reader = new StreamReader(path);
        return reader.ReadToEnd();
    }

    /*
    textSplit
    生の文章を一行ずつに分割する。これもどこかで使えそうだからクラスメソッドで。
    汎用性のために、引数も返り値もLineオブジェクトでなくstring型なので注意。
    @param string
    @return string[]
    */
    public static string[] TextSplit(string str)
    {
        string[] lines = str.Split('\n');
        return lines;
    }

    /*
    editEach
    Lineオブジェクトをシナリオ記法通りに分割する
    @param Line
    */
    private void EditEach(Line line)
    {
        //このタイミングで、会話中フラグをtrue, 文字を入れるウィンドウの表示、立ち絵を消す
        //空行を見る→シナリオ記法通りかみる→疑問文か見る→@を見る
        //@で疑問文モードに入ったりするのは、TextEditor側で管理しないとダメかも。各行はそれを知らないから。
        if (String.IsNullOrEmpty(line.text))
        {
            line.isBlank = true;
            return;
        }

        //疑問文の場合
        if (line.text.StartsWith("?"))
        {
            line.isQuestion = true;

            //文頭の?を取る
            line.text = line.text.TrimStart('?');
        }

        //答えの開始場合
        if (line.text.StartsWith("@"))
        {
            line.isOneOfTheAns = true;

            //文頭の@を取る
            line.text = line.text.TrimStart('@');
        }

        //ここでしか使わない変数だからちょっと長い名前でもいいよね、いいよ
        string[] who_said_what = line.text.Split('_');
        line.name = who_said_what[0];
        line.say = who_said_what[1];
    }

    // ここがメインの編集関数、監督
    public void edit()
    {
        //生の文章を改行区切りで分けてrawlinesに入れる
        string[] rawlines = TextSplit(this.rawtext);

        //rawlinesをforで回して1行ずつ取り出し、SetLineメソッドに渡す
        for (int i = 0; i < rawlines.Length; i++)
        {
            //もし処理中の行にアスタリスクが含まれていたら、それは川村のコメント行なので、Lineオブジェクトすら作らずに無視する
            if (rawlines[i].Contains("*"))
            {
                //コメントの前後に空行があるので、後ろの空行も合わせて無視する
                i++;
                continue;
            }

            //ここで渡したrawlinesをLine.textに持つLineオブジェクトが作られ、Lines_listに追加される
            SetLine(rawlines[i]);
        }

        //SetLineで作られた各Lineオブジェクトを編集する
        foreach (var Line in Lines_list)
        {
            EditEach (Line);
        }
    }
}
