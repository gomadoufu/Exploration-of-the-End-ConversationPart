using System;
//ConversationGraphics.cs
//Created by gomadoufu
//Created on 2022/5/4
//Last modified on 2022/5/4
//テキストに合わせて、適切な文字表示やウィンドウ表示をするクラス
//5/5 立ち絵の表示だけGeneralに分離すること！！！！！！！
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConversationGraphics : MonoBehaviour
{
    [SerializeField]
    private Image linebox;

    [SerializeField]
    private Image namebox;

    [SerializeField]
    private Image selectbox;

    [SerializeField]
    private Image selectIconY;

    [SerializeField]
    private Image selectIconN;

    [SerializeField]
    private Image StandImageR;

    [SerializeField]
    private Transform StandImagepos;

    //立ち絵の種類の宣言
    public enum standimage
    {
        hakase,
        sensei
    }

    //立ち絵のフェードアウトとインの速度（弄ると位置座標がずれるので触らないこと）
    private float fadespeed = 0.1f;

    //列挙型のstandimage型、立ち絵の情報を管理する
    private standimage nextstand;

    private standimage nowstand;

    void Start()
    {
        linebox.enabled = false;
        namebox.enabled = false;
        selectbox.enabled = false;
    }

    public void StartConversation()
    {
        linebox.enabled = true;
        namebox.enabled = true;
    }

    //地の文
    public void DescriptivePart()
    {
        namebox.enabled = false;
    }

    //普通の文
    public void CommonPart()
    {
        namebox.enabled = true;
    }

    //選択肢の表示
    public void ShowChoices()
    {
        selectbox.enabled = true;
    }

    //選択肢を隠す
    public void HideChoices()
    {
        selectbox.enabled = false;
    }

    //会話のウィンドウを閉じる
    public void CloseWindow()
    {
        linebox.enabled = false;
        namebox.enabled = false;
        selectbox.enabled = false;
        //        selectIconY.enabled = false;
        //       selectIconN.enabled = false;
    }

    //会話パートを終了する
    public void EndConversation()
    {
        linebox.enabled = false;
        namebox.enabled = false;
        selectbox.enabled = false;
        //      selectIconY.enabled = false;
        //     selectIconN.enabled = false;
        //    StandImageR.enabled = false;
    }
}
