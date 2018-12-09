using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ConversationMain : MonoBehaviour {
    //名前表示ウィンドウ
    private GameObject mNameWindow;
    //名前表示欄
    private TextMesh mNameText;
    //会話文表示欄
    private TextMesh mText1;
    private TextMesh mText2;
    private TextMesh mText3;
    //会話文送り
    private Action mNext;
    void Awake(){
        //Arg tArg = MySceneManager.getArg("conversation");
        //会話文表示欄
        foreach (TextMesh tText in this.GetComponentsInChildren<TextMesh>()){
            switch (tText.name){
                case "text1": mText1 = tText; break;
                case "text2": mText2 = tText; break;
                case "text3": mText3 = tText; break;
            }
        }
        //名前ウィンドウ
        mNameWindow = this.transform.Find("nameWindow").gameObject;
        //名前表示欄
        mNameText = mNameWindow.GetComponentInChildren<TextMesh>();
    }
    //会話文表示
    public void showMessage(Arg aArg,Action aCallback){
        string tType = (aArg.ContainsKey("type")) ? aArg.get<string>("type") : "single";
        switch(tType){
            case "single":
                showSingle(aArg, aCallback);
                break;
            case "list":
                HandleAsync.processInOrder<Arg>(aArg.get<List<Arg>>("list"), (aElement, aRes) =>{
                    showMessage(aElement, aRes);
                }, () => { aCallback(); });
                break;
        }
    }
    private void showSingle(Arg aArg,Action aCallback){
        //名前
        if (aArg.ContainsKey("name")){
            mNameWindow.SetActive(true);
            mNameText.text = aArg.get<string>("name");
        }else{
            mNameWindow.SetActive(false);
        }
        //会話文
        mText1.text = aArg.get<string>("text");
        mNext = () =>{
            mNext = null;
            aCallback();
        };
    }
    private void OnMouseUp(){
        if(mNext!=null)
            mNext();
    }
}
