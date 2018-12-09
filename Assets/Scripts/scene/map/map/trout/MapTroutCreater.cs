using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MapTroutCreater {
    ///マスの画像があるフォルダのパス
    static public readonly string kDirPath = "Sprites/map/ground";
    ///マスのデータ
    static private Dictionary<string, Arg> mSpriteData = new Dictionary<string, Arg>();
    ///マスのデータを読み込む
    static public Arg loadMasData(string aFileName, int aX, int aY){
        string tKey = aX.ToString() + "," + aY.ToString();
        //地形データファイル読み込み
        if (!mSpriteData.ContainsKey(aFileName)){
            mSpriteData[aFileName] = new Arg(MyJson.deserializeResourse(kDirPath + "/" + aFileName + "C"));
        }
        return mSpriteData[aFileName].get<Arg>(tKey);
    }
    ///マス生成
    static public MapTrout create(List<Arg> aChip){
        MapTrout tTrout = MyBehaviour.create<MapTrout>();
        List<Arg> tMasDataList = new List<Arg>();//マスのタイル情報リスト
        MapAttributeBehaviour tAttributeBehaviour = tTrout.gameObject.AddComponent<MapAttributeBehaviour>();//マップ属性
        tAttributeBehaviour.setAttribute(MapBehaviourAttribute.Attribute.none);//マップ属性の初期値をnoneに設定
        //マスのタイル情報を読んでリストに
        foreach(Arg tData in aChip){
            Arg tMasData = loadMasData(tData.get<string>("file"), tData.get<int>("x"), tData.get<int>("y"));
            tMasData.set("file", tData.get<string>("file"));
            tMasDataList.Add(tMasData);
            //マップ属性を重ねる
            tAttributeBehaviour.mAttribute.pile(tMasData.get<string>("attribute"));
        }
        //画像設定
        ChildSprite.addSpriteObject(tTrout.gameObject, new Arg(new Dictionary<string,object>(){{"pile",tMasDataList}}), kDirPath);
        //collider設定
        if(tAttributeBehaviour.mAttribute.attribute!=MapBehaviourAttribute.Attribute.flat&&
           tAttributeBehaviour.mAttribute.attribute != MapBehaviourAttribute.Attribute.none){
            BoxCollider2D tBox=tTrout.gameObject.AddComponent<BoxCollider2D>();
            tBox.size = new Vector2(1, 1);
        }
        return tTrout;
    }
}
