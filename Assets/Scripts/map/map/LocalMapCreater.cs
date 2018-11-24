using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LocalMapCreater {
    static public LocalMap create(string aMapName){
        //マップデータロード
        Arg tMapData = new Arg(MyJson.deserializeResourse("Database/map/" + aMapName));
        List<List<int>> tTroutData = tMapData.get <List<List<int>>>("feild");
        Dictionary<string, List<Dictionary<string, object>>> tChipData
        = tMapData.get<Dictionary<string, List<Dictionary<string, object>>>>("chip");

        int tWidth = tTroutData[0].Count;
        int tHeight = tTroutData.Count;
        MapTrout[,] tTrouts = new MapTrout[tWidth,tHeight];
        List<MapCharacter> tCharacters = new List<MapCharacter>();
        List<MapObject> tObjects = new List<MapObject>();
        //List<MapEventTrigger> tTriggers = new List<MapEventTrigger>();

        GameObject tTiles = GameObject.Find("mapTiles");
        GameObject tSpeakers = GameObject.Find("mapSpeakers");
        //マス
        for (int tX = 0; tX < tWidth;tX++){
            for (int tY = 0; tY < tHeight;tY++){
                string tChipNum = tTroutData[tY][tX].ToString();
                List<Dictionary<string, object>> tChip = tChipData[tChipNum];
                MapTrout tTrout = MapTroutCreater.create(tChip);
                tTrout.transform.parent = tTiles.transform;
                tTrout.mapPosition = new Vector2(tX, tY);
                tTrout.name = "("+tX.ToString()+","+tY.ToString()+")";
                tTrouts[tX,tY] = tTrout;
            }
        }
        //オブジェクト
        foreach(Dictionary<string,object> tObjectData in tMapData.get<List<Dictionary<string,object>>>("object")){
            MapObject tObject = MapObjectCreater.create((string)tObjectData["file"], (string)tObjectData["name"]);
            tObject.mapPosition = new Vector2((int)tObjectData["x"], (int)tObjectData["y"]);
            tObject.transform.parent = tSpeakers.transform;
            tObjects.Add(tObject);
        }
        LocalMap tMap = new LocalMap(tTrouts, tCharacters, tObjects, new List<MapEventTrigger>());
        //イベント
        foreach(Dictionary<string,object> tEventData in tMapData.get<List<Dictionary<string,object>>>("event")){
            MapEventTrigger tTrigger = MapEventTriggerCreater.create(tEventData,tMap);
            tTrigger.mapPosition = new Vector2((int)tEventData["x"], (int)tEventData["y"]);
            tMap.addEventTrigger(tTrigger);
        }
        return tMap;
    }
}
