using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEventOperator {
    static private string mName = "mapEventOperator";
    private LocalMap mMap;
    public MapEventOperator(LocalMap aMap){
        mMap = aMap;
        enable();
    }
    //イベント監視開始
    public void enable(){
        Observer tObserver = new Observer(mName, getMessage, "map");
        Subject.addObserver(tObserver);
    }
    //イベント監視終了
    public void disable(){
        Subject.removeObserver(mName);
    }
    private void getMessage(Message aMessage){
        if (aMessage.name != "mapEvent") return;
        MapEvent tEvent = aMessage.getParameter<MapEvent>("event");
        tEvent.run(mMap, () => { });
    }
}
