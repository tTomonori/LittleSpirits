using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate MapPassType MapEventJudgePassType(MapBehaviour aBehaviour);
public delegate bool MapEventJudgeTriggerBehaviour(MapBehaviour aBehaviour);
public class MapEventTrigger : MapBehaviour {
    //実行するイベント
    private MapEvent mEnterEvent;
    private MapEvent mExitEvent;
    private MapEvent mStayEvent;
    //発動させるBehaviourかどうか判定する
    private MapEventJudgeTriggerBehaviour mJudgeBehaviour;
    //入って来たBehaviourの通過パターンを返す
    private MapEventJudgePassType mJudgePassType;
    //一度発火したら削除するか
    private bool mDestroyFlag = false;
    public void setEnterEvent(MapEvent aEvent){
        mEnterEvent = aEvent;
    }
    public void setExitEvent(MapEvent aEvent){
        mExitEvent = aEvent;
    }
    public void setStayEvent(MapEvent aEvent){
        mStayEvent = aEvent;
    }
    public void setJudgePassTypeFunction(MapEventJudgePassType aJudge){
        mJudgePassType = aJudge;
    }
    public void setJudgeTriggerBehaviourFunction(MapEventJudgeTriggerBehaviour aJudge){
        mJudgeBehaviour = aJudge;
    }
    public void setDestroyFlag(bool aDestroy){
        mDestroyFlag = aDestroy;
    }
    //colliderの通過を許可するか
    public override MapPassType confirmPassType(MapBehaviour aBehaviour){
        if (mJudgePassType == null) return MapPassType.passing;
        return mJudgePassType(aBehaviour);
    }
    //colliderが入って来た時
    private void OnTriggerEnter2D(Collider2D other){
        if (mEnterEvent == null) return;//イベントが設定されているか
        if (!canFire(other)) return;//イベントを発火させるかどうか
        if (mDestroyFlag) gameObject.GetComponent<Collider2D>().enabled = false;//削除フラグ確認
        Subject.sendMessage(new Message("mapEvent", new Arg(new Dictionary<string, object>() { { "event", mEnterEvent } })));
    }
    //colliderが出て行った時
    private void OnTriggerExit2D(Collider2D other){
        if (mExitEvent == null) return;//イベントが設定されているか
        if (!canFire(other)) return;//イベントを発火させるかどうか
        if (mDestroyFlag) gameObject.GetComponent<Collider2D>().enabled = false;//削除フラグ確認
        Subject.sendMessage(new Message("mapEvent", new Arg(new Dictionary<string, object>() { { "event", mExitEvent } })));
    }
    //colliderが接触している間
    private void OnTriggerStay2D(Collider2D other){
        if (mStayEvent == null) return;//イベントが設定されているか
        if (!canFire(other)) return;//イベントを発火させるかどうか
        if (mDestroyFlag) gameObject.GetComponent<Collider2D>().enabled = false;//削除フラグ確認
        Subject.sendMessage(new Message("mapEvent", new Arg(new Dictionary<string, object>() { { "event", mStayEvent } })));
    }
    //イベントを発火できるか
    private bool canFire(Collider2D aCollider){
        MapSpeaker tOtherSpeaker = aCollider.GetComponent<MapSpeaker>();
        if (tOtherSpeaker == null) return false;
        if (mJudgeBehaviour == null) return true;
        return mJudgeBehaviour(tOtherSpeaker);
    }
}
