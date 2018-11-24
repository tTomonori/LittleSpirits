using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class MapEvent {
    public abstract void run(ReturnArg aCallback);
    //イベント生成
    static public MapEvent create(Arg aArg,LocalMap aMap){
        return new MapEventParent(aArg, aMap);
    }
    //イベントクラス内部でイベントを生成する場合
    static protected MapEventChild create(Arg aArg,MapEventParent aParent){
        if (!aArg.ContainsKey("type"))
            return new MapEventSingle(aArg,aParent);
        switch(aArg.get<string>("type")){
            case "list":
                return new MapEventList(aArg,aParent);
            default:
                throw new Exception("MapEvent : 不正なイベントタイプ「" + aArg.get<string>("type") + "」");
        }
    }
}