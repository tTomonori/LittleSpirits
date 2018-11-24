using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEventList : MapEventChild {
    private MapEvent[] mEvents;
    public MapEventList(Arg aArg,MapEventParent aParent):base(aParent){
        List<Dictionary<string, object>> tEventList = aArg.get<List<Dictionary<string, object>>>("events");
        mEvents = new MapEvent[tEventList.Count];
        for (int i = 0; i < tEventList.Count;i++){
            Dictionary<string, object> tEventData = tEventList[i];
            mEvents[i] = MapEvent.create(new Arg(tEventData),aParent);
        }
    }
    public override void run(ReturnArg aCallback){
        int i = -1;
        int tLength = mEvents.Length;
        ReturnArg tLoop=(a)=>{};
        tLoop = (a) =>{
            i++;
            if(tLength<=i){
                aCallback();
                return;
            }
            mEvents[i].run(tLoop);
        };
        tLoop();
    }
}
