using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class MapObjectCreater {
    static private Dictionary<string, Arg> mObjectDatas=new Dictionary<string, Arg>();
    static public MapObject create(string aFileName, string aName){
        if (!mObjectDatas.ContainsKey(aFileName)){
            mObjectDatas[aFileName] = new Arg(MyJson.deserializeResourse("Sprites/map/accessory/" + aFileName));
        }
        Arg tObjectData = mObjectDatas[aFileName].get<Arg>(aName);
        MapObject tMapObject = MyBehaviour.create<MapObject>();
        tMapObject.name = aName;
        //sprite
        if(tObjectData.ContainsKey("sprite")){
            Dictionary<string, object> tSpriteData = tObjectData.get<Dictionary<string, object>>("sprite");
            float tInterval;
            List<Sprite> tSprites = MapObjectImageLoader.load(tSpriteData, out tInterval);
            MapTile tTile = MyBehaviour.create<MapTile>();
            //collisionType設定
            if (tInterval > 0)
                tTile.init(tSprites, tInterval, new MapBehaviourAttribute(tObjectData.get<string>("attribute")));
            else tTile.init(tSprites[0], new MapBehaviourAttribute(tObjectData.get<string>("attribute")));
            if (tSpriteData.ContainsKey("zIndex")) tTile.positionZ = (float)tSpriteData["zIndex"];
            tMapObject.addTile(tTile);
        }
        //tile
        if(tObjectData.ContainsKey("tile")){
            Dictionary<string, object> tTileData = tObjectData.get<Dictionary<string, object>>("tile");
            foreach (MapTile tTile in MapObjectImageLoader.loadWithTile(tTileData)){
                tMapObject.addTile(tTile);
            }
        }
        //attribute
        tMapObject.mBehaviourAttribute = new MapBehaviourAttribute(tObjectData.get<string>("attribute"));
        //collider設定
        if(tObjectData.ContainsKey("collider")){
            Arg tColliderData = tObjectData.get<Arg>("collider");
            switch (tColliderData.get<string>("colliderType")){
                case "box":
                    BoxCollider2D tBox = tMapObject.gameObject.AddComponent<BoxCollider2D>();
                    tBox.size = new Vector2(tColliderData.get<float>("colliderSizeX"), tColliderData.get<float>("colliderSizeY"));
                    float tColliderOffsetX = (tColliderData.ContainsKey("colliderOffsetX")) ? tColliderData.get<float>("colliderOffsetX") : 0;
                    float tColliderOffsetY = (tColliderData.ContainsKey("colliderOffsetY")) ? tColliderData.get<float>("colliderOffsetY") : 0;
                    tColliderOffsetY += +tColliderData.get<float>("colliderSizeY") / 2;
                    tBox.offset = new Vector2(tColliderOffsetX, tColliderOffsetY);
                    break;
                case "polygon":
                    PolygonCollider2D tPolygon = tMapObject.gameObject.AddComponent<PolygonCollider2D>();
                    List<List<float>> tPointData = tColliderData.get<List<List<float>>>("points");
                    Vector2[] tPoints = new Vector2[tPointData.Count];
                    for (int i = 0; i < tPointData.Count;i++){
                        tPoints[i] = new Vector2(tPointData[i][0], tPointData[i][1]);
                    }
                    tPolygon.points = tPoints;
                    break;
                default:
                    throw new Exception("MapObjectCreater : 不正なColliderTypeだよ「" + tObjectData.get<string>("colliderType") + "」");
            }
        }
        //category
        if(tObjectData.ContainsKey("category")){
            
        }
        return tMapObject;
    }
}
