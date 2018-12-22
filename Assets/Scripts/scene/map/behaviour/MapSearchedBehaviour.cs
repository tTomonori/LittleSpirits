using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSearchedBehaviour : MonoBehaviour {
    ///上から調べられた時のイベント
    private MapEvent mFromUpEvent;
    ///下から調べられた時のイベント
    private MapEvent mFromDownEvent;
    ///左から調べられた時のイベント
    private MapEvent mFromLeftEvent;
    ///右から調べられた時のイベント
    private MapEvent mFromRightEvent;
    ///方向指定のイベントがない時に発火させるイベント
    private MapEvent mEvent;
    ///調べられた時のイベントセット
    public void setSearchedEvent(Arg aArg){
        if (aArg.ContainsKey("event")) mEvent = new MapEvent(aArg.get<Arg>("event"));
        if (aArg.ContainsKey("up")) mFromUpEvent = new MapEvent(aArg.get<Arg>("up"));
        if (aArg.ContainsKey("down")) mFromDownEvent = new MapEvent(aArg.get<Arg>("down"));
        if (aArg.ContainsKey("left")) mFromLeftEvent = new MapEvent(aArg.get<Arg>("left"));
        if (aArg.ContainsKey("right")) mFromRightEvent = new MapEvent(aArg.get<Arg>("right"));
    }
    ///調べられた
    public void searched(MapCharacter aCharacter){
        //距離を測る
        ColliderDistance2D tDistance = Physics2D.Distance(gameObject.GetComponents<Collider2D>()[0],
                                                     aCharacter.gameObject.GetComponents<Collider2D>()[0]);
        //調べた方向
        Direction tDirection = new Direction(tDistance.normal);
        //発火するイベント
        MapEvent tEvent = getEvent(tDirection);

        if (tEvent == null) return;
        Subject.sendMessage(new Message("mapEvent", new Arg(new Dictionary<string, object>() { { "event", tEvent } })));
    }
    ///指定した方向から調べた時のイベント取得
    private MapEvent getEvent(Direction tDirection){
        switch(tDirection.value){
            case Direction.direction.Up:return mFromDownEvent != null ? mFromDownEvent : mEvent;
            case Direction.direction.Down:return mFromUpEvent != null ? mFromUpEvent : mEvent;
            case Direction.direction.Left:return mFromRightEvent != null ? mFromRightEvent : mEvent;
            case Direction.direction.Right:return mFromLeftEvent != null ? mFromLeftEvent : mEvent;
            default:return null;
        }
    }
}
