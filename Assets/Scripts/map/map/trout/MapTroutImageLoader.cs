using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MapTroutImageLoader {
    static private Dictionary<string, Arg> mSpriteData = new Dictionary<string, Arg>();
    static public List<Sprite> load(string aFile, int aX, int aY,
                                    out MapBehaviourAttribute aAttribute, out float aInterval){
        string tKey = aX.ToString() + "," + aY.ToString();
        //地形データファイル読み込み
        if (!mSpriteData.ContainsKey(aFile)){
            mSpriteData[aFile] = new Arg(MyJson.deserializeResourse("Sprites/map/ground/" + aFile + "C"));
        }
        Arg tData = mSpriteData[aFile].get<Arg>(tKey);
        //地形属性
        aAttribute = new MapBehaviourAttribute(tData.get<string>("attribute"));

        //画像
        List<Sprite> tList = new List<Sprite>();
        if(tData.ContainsKey("interval")){
            //アニメーションあり
            aInterval = tData.get<float>("interval");
            foreach(List<int> tPositionList in tData.get<List<List<int>>>("animation")){
                tList.Add(ChipLoader.load("Sprites/map/ground/" +aFile, tPositionList[0], tPositionList[1]));
            }
            return tList;
        }else{
            //アニメーションなし
            aInterval = -1f;
            tList.Add(ChipLoader.load("Sprites/map/ground/"+aFile, aX, aY));
            return tList;
        }
    }
}
