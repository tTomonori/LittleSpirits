using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalMap {
    private MapTrout[,] mTrouts;
    private List<MapCharacter> mCharacters;
    private List<MapObject> mObjects;
    private List<MapEventTrigger> mTriggers;
    private MapEventOperator mEventOperator;
    public LocalMap(MapTrout[,] aTrouts,List<MapCharacter> aCharacters,List<MapObject> aObjects,List<MapEventTrigger> aTriggers){
        mTrouts = aTrouts;
        mCharacters = aCharacters;
        mObjects = aObjects;
        mTriggers = aTriggers;
        mEventOperator = new MapEventOperator(this);
    }
    public void addCharacter(MapCharacter aChara){
        mCharacters.Add(aChara);
    }
    public void addEventTrigger(MapEventTrigger aTrigger){
        mTriggers.Add(aTrigger);
    }
    public MapCharacter getCharacter(string aName){
        foreach(MapCharacter tChara in mCharacters){
            if (tChara.name == aName) return tChara;
        }
        return null;
    }
}
