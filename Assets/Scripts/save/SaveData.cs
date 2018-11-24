using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SaveData {
    static private Arg mSaveData;
    static public void load(){
        mSaveData = Arg.loadJson("Assets/Resources/save/save.json");

        //手持ちロードテスト
        //List<object> tAccompanying = mSaveData["accompanying"] as List<object>;
        //Dictionary<string, object> tA1 = tAccompanying[0] as Dictionary<string, object>;
        //Debug.Log((string)tA1["name"]);

        //Arg t = MyJson.load("Assets/Resources/save/save.json");
        //Debug.Log(t.getListOfArg("accompanying")[0].getString("name"));
    }
    static public string getPlayerName() { return mSaveData.get<string>("name"); }
    static public int getMoney() { return mSaveData.get<int>("money"); }
    static public string getMapName() { return mSaveData.get<string>("map"); }
    static public Vector2 getPosition() { return new Vector2(mSaveData.get<float>("positionX"), mSaveData.get<float>("positionY")); }
    static public int getPositionX() { return mSaveData.get<int>("positionX"); }
    static public int getPositionY() { return mSaveData.get<int>("positionY"); }
    static public string getDirection() { return mSaveData.get<string>("direction"); }
}
