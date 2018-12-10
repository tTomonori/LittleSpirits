using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class MapObjectCreater {
    ///オブジェクトの画像があるフォルダのパス
    static public readonly string kDirPath = "Sprites/map/accessory";
    static private Dictionary<string, Arg> mObjectDatas = new Dictionary<string, Arg>();
    static public MapObject create(string aFileName, string aName,string id=""){
        if (!mObjectDatas.ContainsKey(aFileName)){
            mObjectDatas[aFileName] = new Arg(MyJson.deserializeResourse(MapObjectCreater.kDirPath + "/" + aFileName));
        }
        Arg tObjectData = mObjectDatas[aFileName].get<Arg>(aName);
        MapObject tMapObject = MyBehaviour.create<MapObject>();
        tMapObject.name = (id == "") ? aName : aFileName;
        //sprite
        if(tObjectData.ContainsKey("sprite")){
            //画像があるフォルダのパス
            string tPath = HandleString.cutOff(aFileName, "/");
            tPath = (tPath == "") ? MapObjectCreater.kDirPath : MapObjectCreater.kDirPath + "/" + tPath;
            Arg tData = tObjectData.get<Arg>("sprite");
            //pivotYが設定されていないなら0に設定
            if (!tData.ContainsKey("pivotY")) tData.set("pivotY", 0f);
            //スプライトセット
            ChildSprite.addSpriteObject(tMapObject.gameObject, tData, tPath);
        }
        //zIndex
        if (tObjectData.ContainsKey("zIndex"))
            tMapObject.zIndex = tObjectData.get<float>("zIndex");
        //マップ属性
        if(tObjectData.ContainsKey("attribute")){
            tMapObject.gameObject.AddComponent<MapAttributeBehaviour>().setAttribute(tObjectData.get<string>("attribute"));
        }
        //collider設定
        if(tObjectData.ContainsKey("collider")){
            Arg tColliderData = tObjectData.get<Arg>("collider");
            ColliderInstaller.addCollider(tMapObject.gameObject, tObjectData.get<Arg>("collider"));
        }
        //category
        if(tObjectData.ContainsKey("category")){
            
        }
        return tMapObject;
    }
}
