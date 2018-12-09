using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public delegate MapPassType MapEventJudgePassType(MapBehaviour aBehaviour);
public delegate bool MapEventJudgeTriggerBehaviour(MapBehaviour aBehaviour);
public class MapEventTrigger : MapBehaviour {
    //実行するイベント
    private MapEvent mEnterEvent;
    private MapEvent mExitEvent;
    private MapEvent mStayEvent;
    //発動させるBehaviourかどうか判定する
    private Arg mTrigger;
    //一度発火したら削除するか
    private bool mDestroyFlag;
    public void set(MapEvent aEnter,MapEvent aExit,MapEvent aStay,Arg aTrigger,bool aDestroy){
        mEnterEvent = aEnter;
        mExitEvent = aExit;
        mStayEvent = aStay;
        mTrigger = aTrigger;
        mDestroyFlag = aDestroy;
    }
    //colliderの通過を許可するか
    public MapPassType confirmPassType(MapAttributeBehaviour aBehaviour){
        if(mStayEvent==null){
            if(Physics2D.IsTouching(aBehaviour.GetComponents<Collider2D>()[0], this.GetComponents<Collider2D>()[0])){
                return MapPassType.passing;//stayイベントがない　かつ　既に接触している → 通過可
            }
        }

        if (equal(aBehaviour.gameObject, mTrigger)) 
            return (MapPassType)Enum.Parse(typeof(MapPassType), mTrigger.get<string>("passType"), true);
        else 
            return MapPassType.passing;
    }
    //colliderが入って来た時
    private void OnTriggerEnter2D(Collider2D other){
        if (mEnterEvent == null) return;//イベントが設定されているか
        if (!equal(other.gameObject, mTrigger)) return;//イベントを発火させるかどうか
        if (mDestroyFlag) gameObject.GetComponent<Collider2D>().enabled = false;//削除フラグ確認
        Subject.sendMessage(new Message("mapEvent", new Arg(new Dictionary<string, object>() { { "event", mEnterEvent } })));
    }
    //colliderが出て行った時
    private void OnTriggerExit2D(Collider2D other){
        if (mExitEvent == null) return;//イベントが設定されているか
        if (!equal(other.gameObject, mTrigger)) return;//イベントを発火させるかどうか
        if (mDestroyFlag) gameObject.GetComponent<Collider2D>().enabled = false;//削除フラグ確認
        Subject.sendMessage(new Message("mapEvent", new Arg(new Dictionary<string, object>() { { "event", mExitEvent } })));
    }
    //colliderが接触している間
    private void OnTriggerStay2D(Collider2D other){
        if (mStayEvent == null) return;//イベントが設定されているか
        if (!equal(other.gameObject, mTrigger)) return;//イベントを発火させるかどうか
        if (mDestroyFlag) gameObject.GetComponent<Collider2D>().enabled = false;//削除フラグ確認
        Subject.sendMessage(new Message("mapEvent", new Arg(new Dictionary<string, object>() { { "event", mStayEvent } })));
    }
    ///colliderとargが同じbehaviourを表している
    private bool equal(GameObject aBehaviour,Arg aTerms){
        foreach(Arg tTerm in aTerms.get<List<Arg>>("terms")){
            //名前
            if(tTerm.ContainsKey("name")){
                if (aBehaviour.name == tTerm.get<string>("name")) return true;
            }
        }
        return false;
    }
}
