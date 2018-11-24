using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MapEventParent : MapEvent{
    private LocalMap mMap;
    private MapEventChild mChild;
    private Dictionary<string, MapCharacter.MapCharaJackedAi> mJackedAis;
    private List<string> mTargetAi;
    public MapEventParent(Arg aArg,LocalMap aMap){
        mMap = aMap;
        if (!aArg.ContainsKey("jack")){
            mChild = MapEvent.create(aArg, this);
            return;
        }else{
            mChild = MapEvent.create(aArg.get<Arg>("event"),this);
        }
        //AIジャックの標的を記憶
        List<Dictionary<string, string>> tTargetList = aArg.get<List<Dictionary<string, string>>>("jack");
        mTargetAi = new List<string>();
        foreach(Dictionary<string,string> tData in tTargetList){
            mTargetAi.Add(tData["name"]);
        }
    }
    public override void run(ReturnArg aCallback){
        VoidFunction tRunChild=()=>{
            mChild.run((a) => {
                releaseAis();
            });
        };
        if(mTargetAi==null){
            //AIジャックなしの場合
            tRunChild();
            return;
        }
        //AIジャック
        jackAi(mTargetAi, () =>{
            tRunChild();
        });
    }
    //指定された全てのキャラのAIをジャック
    public void jackAi(List<string> aName,VoidFunction aCallback){
        int tCount = aName.Count;
        VoidFunction tJacked = () => {
            tCount--;
            if (tCount > 0) return;
            aCallback();
        };
        foreach(string tName in aName){
            MyBehaviour.runCoroutine(jackAi(tName, tJacked));
        }
    }
    //AIジャックする
    public IEnumerator jackAi(string aName, VoidFunction aCallback){
        if (mJackedAis == null) mJackedAis = new Dictionary<string, MapCharacter.MapCharaJackedAi>();
        while(true){
            MapCharacter tChara = mMap.getCharacter(aName);
            if (tChara == null) throw new Exception("MapEventParent : 「"+aName+"」なんて名前のキャラはいないの");
            if (tChara.canJackAi()){
                mJackedAis[aName] = tChara.jackAi();
                break;
            }
            yield return null;
        }
        aCallback();
    }
    //ジャックしたAIを全て戻す
    private void releaseAis(){
        if (mJackedAis == null) return;
        foreach(KeyValuePair<string,MapCharacter.MapCharaJackedAi> tAiKey in mJackedAis){
            mJackedAis[tAiKey.Key].endJack();
        }
    }
}
