using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

static public class ConversationWindow  {
    //会話文表示クラス
    static private ConversationMain mConversationMain;
    //会話文を表示(会話ウィンドウが開かれていないなら開く)
    static public void show(Arg aArg,Action aCallback){
        if(mConversationMain!=null){
            //既にウィンドウが開かれている
            Display(aArg,aCallback);
            return;
        }
        //ウィンドウを開いてから表示する
        MySceneManager.openScene("conversation", new Arg(), (aScene) =>{
            foreach(GameObject tObject in aScene.GetRootGameObjects()){
                mConversationMain = tObject.GetComponent<ConversationMain>();
                if (mConversationMain != null) break;
            }
            Display(aArg, aCallback);
        });
    }
    //会話ウィンドウを閉じる
    static public void close(Action aCallback){
        if(mConversationMain==null){//既に閉じられている
            aCallback();
            return;
        }
        mConversationMain = null;
        MySceneManager.closeScene("conversation", new Arg(),(_)=>{
            aCallback();
        });
    }
    //会話文を表示
    static private void Display(Arg aArg,Action aCalback){
        mConversationMain.showMessage(aArg, () =>{
            aCalback();
        });
    }
}
