using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MapEvent {
    private Arg mEvent;
    ///jackしたAI
    private Dictionary<string, MapCharacter.MapCharaJackedAi> mJackedAi;
    public MapEvent(Arg aArg){
        mEvent = aArg;
    }
    public void run(LocalMap aMap,VoidFunction aCallback){
        mJackedAi = new Dictionary<string, MapCharacter.MapCharaJackedAi>();
        //AI Jack
        //jackできるか確認
        foreach(string tName in mEvent.get<List<String>>("jack")){
            if(!aMap.getCharacter(tName).canJackAi()){//jack不可(イベント発火中止)
                aCallback();
                return;
            }
        }
        //jackする
        foreach (string tName in mEvent.get<List<String>>("jack")){
            mJackedAi[tName] = aMap.getCharacter(tName).jackAi();
        }
        //イベント発火
        runEventList(()=>{
            //jackしたAIを解放
            foreach(KeyValuePair<string,MapCharacter.MapCharaJackedAi> tAi in mJackedAi){
                tAi.Value.endJack();
            }
            mJackedAi = new Dictionary<string, MapCharacter.MapCharaJackedAi>();
            //会話ウィンドウを閉じる
            ConversationWindow.close(()=>{
                aCallback();
            });
        });
    }
    ///イベント発火
    private void runEventList(VoidFunction aCallback){
        List<Arg> tEvents = mEvent.get<List<Arg>>("events");
        int tLength = tEvents.Count;
        //イベントを1つずつ実行
        Action<int> fNext = (_) => { };
        fNext = (i) =>{
            if(tLength<=i){//全てのイベント終了
                aCallback();
                return;
            }
            //イベントを1つ実行
            runEvent(tEvents[i], () => { fNext(i + 1); });
        };
        fNext(0);
    }
    ///イベントを一つ実行
    private void runEvent(Arg aEvent,VoidFunction aCallback){
        switch(aEvent.get<string>("event")){
            case "log"://ログ出力
                Debug.Log(aEvent.get<string>("value"));
                aCallback();
                break;
            case "text"://会話イベント
                ConversationWindow.show(aEvent, () =>{
                    aCallback();
                });
                break;
            default:
                throw new Exception("MapEvent : 「"+aEvent.get<string>("event")+"」ってどんなイベント？");
        }
    }
}