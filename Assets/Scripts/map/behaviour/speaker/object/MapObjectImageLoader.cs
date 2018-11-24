using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class MapObjectImageLoader {
    static public string kFileDirectory{
        get { return "Sprites/map/accessory/"; }
    }
    static public List<Sprite> load(Dictionary<string,object> aData,out float aInterval){
        List<Sprite> tSprites = new List<Sprite>();
        if(aData.ContainsKey("animation")){
            Dictionary<string, object> tAnimationData = (Dictionary<string, object>)aData["animation"];
            //アニメーションあり
            aInterval = (float)tAnimationData["interval"];
            int tWidth = (int)aData["width"];
            int tHeight = (int)aData["height"];
            int tFrameWidth;
            int tFrameHeight;
            if((string)tAnimationData["direction"]=="horizontal"){
                tFrameWidth = tWidth;
                tFrameHeight = 0;
            }else if((string)tAnimationData["direction"] == "vertical"){
                tFrameWidth = 0;
                tFrameHeight = tHeight;
            }else{throw new Exception("MapObjectLoader : アニメーションの不正な方向");}
            for (int i = 0; i < (int)tAnimationData["frame"];i++){
                Sprite tSprite = ChipLoader.load("Sprites/map/accessory/"+(string)aData["file"],
                                                 (int)aData["x"]+i*tFrameWidth, (int)aData["y"]+i*tFrameHeight,
                                                 tWidth, tHeight,
                                                 new Vector2(0.5f, 0));
                tSprites.Add(tSprite);
            }
        }else{
            //アニメーションなし
            aInterval = -1f;
            Sprite tSprite = ChipLoader.load("Sprites/map/accessory/" +(string)aData["file"], 
                            (int)aData["x"], (int)aData["y"], 
                            (int)aData["width"], (int)aData["height"], 
                            new Vector2(0.5f, 0));
            tSprites.Add(tSprite);
        }
        return tSprites;
    }
    static public List<MapTile> loadWithTile(Dictionary<string,object> aData){
        List<MapTile> tTileList = new List<MapTile>();
        string tFileName = kFileDirectory + (string)aData["file"];
        List<List<List<int>>> tTileData = (List<List<List<int>>>)aData["tiles"];
        float tWidth = tTileData[0].Count;
        float tHeight = tTileData.Count;
        for (int tY = 0; tY < tTileData.Count;tY++){
            List<List<int>> tLine = tTileData[tY];
            for (int tX = 0; tX < tLine.Count;tX++){
                List<int> tChip = tLine[tX];
                if (tChip[0] == -1) continue;
                Sprite tSprite = ChipLoader.load(tFileName, tChip[0], tChip[1]);
                MapTile tTile = MyBehaviour.create<MapTile>();
                tTile.init(tSprite, new MapBehaviourAttribute(MapBehaviourAttribute.Attribute.accessory));
                tTile.transform.position = new Vector3(tX - tWidth / 2 + 0.5f, tHeight - tY - 0.5f, 0);
                tTileList.Add(tTile);
            }
        }
        return tTileList;
    }
}
