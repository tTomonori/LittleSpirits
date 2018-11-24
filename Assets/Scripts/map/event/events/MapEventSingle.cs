using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MapEventSingle : MapEventChild {
    private Arg mArg;
    public MapEventSingle(Arg aArg,MapEventParent aParent):base(aParent){
        mArg = aArg;
    }
    public override void run(ReturnArg aCallback){
        switch(mArg.get<string>("event")){
            case "log":
                Debug.Log(mArg.get<string>("value"));
                aCallback();
                return;
            default:
                throw new Exception("MapEventSingle : 不正なイベント「"+mArg.get<string>("event")+"」って何すればいいのか分かんないよ");
        }
    }
}
