using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class MapEventTriggerCreater {
    static public MapEventTrigger create(Dictionary<string,object> aData,LocalMap aMap){
        MapEventTrigger tTrigger = MyBehaviour.create<MapEventTrigger>();
        //trigger
        Dictionary<string, object> tTriggerData = (Dictionary<string, object>)aData["trigger"];
        //collider
        Dictionary<string, object> tColliderData = (Dictionary<string, object>)tTriggerData["collider"];
        switch((string)tColliderData["colliderType"]){
            case "box":
                BoxCollider2D tBox = tTrigger.gameObject.AddComponent<BoxCollider2D>();
                float tWidth = (int)tColliderData["width"];
                float tHeight = (int)tColliderData["height"];
                tBox.size = new Vector2(tWidth, tHeight);
                tBox.isTrigger = true;
                break;
            default:
                throw new Exception("MapEventTriggerCreater : 不正なColliderTypeだよ「" + (string)tColliderData["colliderType"] + "」");
        }
        //jack
        if(tTriggerData.ContainsKey("jack")){
            List<Dictionary<string, string>> tBehaviours = (List<Dictionary<string, string>>)tTriggerData["jack"];
            //passType
            tTrigger.setJudgePassTypeFunction((aBehaviour) => {
                foreach(Dictionary<string,string> tBehaviourData in tBehaviours){
                    if (isSamePerson(aBehaviour, tBehaviourData)) return MapPassType.stop;
                }
                return MapPassType.passing;
            });
            //trigger
            tTrigger.setJudgeTriggerBehaviourFunction((aBehaviour) =>{
                foreach (Dictionary<string, string> tBehaviourData in tBehaviours){
                    MapCharacter tChara = aMap.getCharacter((string)tBehaviourData["name"]);
                    if (tChara == null) continue;//指定されたキャラがいない
                    if (!tChara.canJackAi()) return false;//指定されたキャラがAIジャック不可
                }
                return true;
            });
            //イベントの最初にjackイベント追加
            foreach (string tEventKey in new string[]{ "enter","exit","stay"}){
                if (!aData.ContainsKey(tEventKey)) continue;
                aData[tEventKey] = new Dictionary<string, object>(){
                    {"jack",tTriggerData["jack"]},
                    {"event",aData[tEventKey]}
                };
            }
        }
        //passType
        //triggerBehaviour
        if(tTriggerData.ContainsKey("triggerBehaviour")){
            List<Dictionary<string, string>> tTriggerBehaviours = (List<Dictionary<string, string>>)tTriggerData["triggerBehaviour"];
            tTrigger.setJudgeTriggerBehaviourFunction((aBehaviour) => {
                foreach(Dictionary<string,string> tBehaviourData in tTriggerBehaviours){
                    if (isSamePerson(aBehaviour, tBehaviourData)) return true;
                }
                return false;
            });
        }
        //destroy
        if (tTriggerData.ContainsKey("destroy")) tTrigger.setDestroyFlag((bool)tTriggerData["destroy"]);
        //event
        if (aData.ContainsKey("enter"))
            tTrigger.setEnterEvent(MapEvent.create(new Arg((Dictionary<string,object>)aData["enter"]), aMap));
        if (aData.ContainsKey("exit"))
            tTrigger.setExitEvent(MapEvent.create(new Arg((Dictionary<string, object>)aData["exit"]), aMap));
        if (aData.ContainsKey("stay"))
            tTrigger.setStayEvent(MapEvent.create(new Arg((Dictionary<string, object>)aData["stay"]), aMap));
        return tTrigger;
    }
    //二つの引数が同じBehaviourを表しているならtrue
    static  bool isSamePerson(MapBehaviour aBehaviour,Dictionary<string,string> aData){
        if (aData.ContainsKey("name"))
            if ((string)aData["name"] == aBehaviour.name) return true;
        return false;
    }
}
