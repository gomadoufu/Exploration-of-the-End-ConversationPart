using System;
//ConversationManager.cs
//Created by gomadoufu
//Created on 2022/5/4
//Last modified on 2022/5/4
//会話パートを管理するクラス
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

public class ConversationManager : MonoBehaviour
{
    [SerializeField]
    private Text lineText;

    [SerializeField]
    private Text nameText;

    //テキスト処理オブジェクト
    TextEdit textEditor;

    //グラフィックス管理オブジェクト
    ConversationGraphics graphic;

    //会話シーンを開始するフラグ
    private static bool Conversation_Start = false;

    //会話シーンが処理中であることを示すフラグ
    public bool Conversation_Now = false;

    //空行が続くかどうかのフラグ
    public bool preLineIsBlank = false;

    //いま質問&回答フェーズにあるかどうか
    public bool Answering_Now = false;

    //疑問文に対する答えの@をカウントする
    private int atMarkCount = 0;

    private int numOfLineToYes = 0;

    private int numOfLineToNo = 0;

    private int numOfLineToNext = 0;

    //ウィンドウがクリックされたかどうかのフラグ
    public bool windowClicked = false;

    //「はい」のボタンがクリックされたかどうかのフラグ
    public bool YesClicked = false;

    //「いいえ」のボタンがクリックされたかどうかのフラグ
    public bool NoClicked = false;

    //テキストのうち、今処理している行
    public int line = 0;

    //テキスト行のうち、今処理している文字
    public int character = 0;

    private int framecount = 0;

    //一文字ずつ表示する時のフレーム間隔
    //ex.2 -> 2フレームごとに1文字表示
    private int textspeed = 5;

    public static void ConversationSceneStart()
    {
        Conversation_Start = true;
    }

    void Start()
    {
        //ConversationGraphicsを短い名前にしとく
        graphic = GetComponent<ConversationGraphics>();

        //テキストファイルのデータを取得するインスタンスを作成
        TextAsset textasset = new TextAsset();

        //Resourcesフォルダから対象テキストを取得
        textasset = Resources.Load("Sample", typeof (TextAsset)) as TextAsset;

        //テキストデータをあらかじめ全て処理
        textEditor = new TextEdit(textasset.text);
        textEditor.edit();
    }

    // Update is called once per frame
    void Update()
    {
        //会話シーンを開始する合図が来るまで、何もしない
        if (!Conversation_Start && !Conversation_Now)
        {
            return;
        }

        if (!Conversation_Now)
        {
            //graphicに会話シーン開始の合図を送る
            graphic.StartConversation();

            //次のフレームからは送らない
            Conversation_Now = true;
        }

        Line lineobj = textEditor.GetLinesList()[line];

        //空行の場合、会話ウィンドウを閉じる
        if (lineobj.isBlank)
        {
            graphic.CloseWindow();
            Conversation_Now = false;
        }

        //空行が2行続いたら、会話シーンを止める
        if (lineobj.isBlank && preLineIsBlank)
        {
            //ここでConversation_StartもNowもfalseにする
            Conversation_Start = false;
            Conversation_Now = false;

            preLineIsBlank = false;

            graphic.EndConversation();
        }

        //地の文の場合、名前のウィンドウを出さない
        if (String.IsNullOrEmpty(lineobj.name))
        {
            graphic.DescriptivePart();
        }
        else
        {
            graphic.CommonPart();
        }

        //質問文の場合、Answering_Nowをtrueにして、回答文への移動に備える
        if (lineobj.isQuestion)
        {
            atMarkCount = 0;
            graphic.ShowChoices();
            Answering_Now = true;

            //表示はしないが、あらかじめ3つ目の@まで読み込んでおき、@それぞれまでの行数を数えておく
            while (atMarkCount == 1)
            {
                line++;
                Line tmp = textEditor.GetLinesList()[line];
                if (tmp.isOneOfTheAns)
                {
                    atMarkCount++;
                }
            }
        }

        //テキストを表示する
        //テキストの表示は最後
        //textspeedの分だけ間をあける
        if (framecount % textspeed == 0)
        {
            //characterが文字列の長さ以下の間一文字ずつ表示する
            if (lineobj.say.Length >= character)
            {
                lineText.text = lineobj.say.Substring(0, character);
                character++;
            }
        }

        //名前は普通に出す
        nameText.text = lineobj.name;
        framecount++;

        //疑問文の後にYesがクリックされた場合
        if (Answering_Now && YesClicked)
        {
            YesClicked = false;
            lineobj = textEditor.GetLinesList()[line];
        }

        //ウィンドウがクリックされたら、次の行を表示する
        //選択肢が出ていたら隠す
        if (windowClicked && !lineobj.isQuestion)
        {
            //クリックされた時にcharacterが文字列の表示途中だったら、全部表示する
            if (lineobj.say.Length > character)
            {
                lineText.text = lineobj.say;
                character = lineobj.say.Length;
                windowClicked = false;
                return;
            }
            lineText.text = "";
            character = 0;
            line++;
            windowClicked = false;
            graphic.HideChoices();

            //最後に、処理中の行が空行ならフラグを立てる
            if (lineobj.isBlank)
            {
                preLineIsBlank = true;
            }
            else
            {
                preLineIsBlank = false;
            }
        }
    }
}
