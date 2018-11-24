using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MapTroutCreater {
    static public MapTrout create(List<Dictionary<string,object>> aChip){
        MapTrout tTrout = MyBehaviour.create<MapTrout>();
        foreach(Dictionary<string,object> tData in aChip){
            MapTile tTile = createTile((string)tData["file"], (int)tData["x"], (int)tData["y"]);
            tTrout.addTile(tTile);
        }
        return tTrout;
    }
    static public MapTile createTile(string aFileName,int aX,int aY){
        MapBehaviourAttribute tAttribute;
        float tInterval;
        List<Sprite> tSprites = MapTroutImageLoader.load(aFileName, aX, aY, out tAttribute, out tInterval);
        MapTile tTile = MyBehaviour.create<MapTile>();
        if(tInterval>0){
            //アニメーションあり
            tTile.init(tSprites, tInterval, tAttribute);
        }else{
            //アニメーションなし
            tTile.init(tSprites[0], tAttribute);
        }
        return tTile;
    }
}