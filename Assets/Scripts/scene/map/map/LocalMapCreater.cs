using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LocalMapCreater {
    static public LocalMap create(string aMapName){
        //マップデータロード
        Arg tMapData = new Arg(MyJson.deserializeResourse("Database/map/" + aMapName));
        List<List<int>> tTroutData = tMapData.get <List<List<int>>>("feild");
        Arg tChipData = tMapData.get<Arg>("chip");

        int tWidth = tTroutData[0].Count;
        int tHeight = tTroutData.Count;
        MapTrout[,] tTrouts = new MapTrout[tWidth,tHeight];

        //マス
        List<MapCharacter> tCharacters = new List<MapCharacter>();
        GameObject tTiles = GameObject.Find("mapTiles");
        for (int tX = 0; tX < tWidth;tX++){
            for (int tY = 0; tY < tHeight;tY++){
                string tChipNum = tTroutData[tY][tX].ToString();
                List<Arg> tChip = tChipData.get<List<Arg>>(tChipNum);
                MapTrout tTrout = MapTroutCreater.create(tChip);
                tTrout.transform.parent = tTiles.transform;
                tTrout.mapPosition = new Vector2(tX, tY);
                tTrout.name = "("+tX.ToString()+","+tY.ToString()+")";
                tTrouts[tX,tY] = tTrout;
            }
        }
        //オブジェクト
        List<MapObject> tObjects = new List<MapObject>();
        GameObject tSpeakers = GameObject.Find("mapSpeakers");
        foreach(Arg tObjectData in tMapData.get<List<Arg>>("object")){
            MapObject tObject = MapObjectCreater.create(tObjectData.get<string>("file"), tObjectData.get<string>("name"));
            tObject.mapPosition = new Vector2(tObjectData.get<float>("x"), tObjectData.get<float>("y"));
            if (tObjectData.ContainsKey("z"))
                tObject.zIndex = tObjectData.get<float>("z");
            tObject.transform.parent = tSpeakers.transform;
            tObjects.Add(tObject);
            //子オブジェクト

        }
        LocalMap tMap = new LocalMap(tTrouts, tCharacters, tObjects, new List<MapEventTrigger>());
        //イベント
        List<MapEventTrigger> tTriggers = new List<MapEventTrigger>();
        GameObject tEvents = GameObject.Find("mapEvents");
        foreach (Arg tEventData in tMapData.get<List<Arg>>("event")){
            MapEventTrigger tTrigger = MapEventTriggerCreater.create(tEventData);
            tTrigger.mapPosition = new Vector2(tEventData.get<float>("x"), tEventData.get<float>("y"));
            tTrigger.transform.parent = tEvents.transform;
            tMap.addEventTrigger(tTrigger);
        }
        return tMap;
    }
}
