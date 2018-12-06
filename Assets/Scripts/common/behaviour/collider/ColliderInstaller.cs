using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//オブジェクトにcolliderを設定する
public class ColliderInstaller {
    public static Collider2D addCollider(GameObject aObject, Arg aArg){
        switch (aArg.get<string>("colliderType")){
            case "box":
                BoxCollider2D tBox = aObject.AddComponent<BoxCollider2D>();
                tBox.size = new Vector2(aArg.get<float>("colliderSizeX"), aArg.get<float>("colliderSizeY"));
                float tColliderOffsetX = (aArg.ContainsKey("colliderOffsetX")) ? aArg.get<float>("colliderOffsetX") : 0;
                float tColliderOffsetY = (aArg.ContainsKey("colliderOffsetY")) ? aArg.get<float>("colliderOffsetY") : 0;
                tColliderOffsetY += +aArg.get<float>("colliderSizeY") / 2;
                tBox.offset = new Vector2(tColliderOffsetX, tColliderOffsetY);
                return tBox;
            case "polygon":
                PolygonCollider2D tPolygon = aObject.AddComponent<PolygonCollider2D>();
                List<List<float>> tPointData = aArg.get<List<List<float>>>("points");
                Vector2[] tPoints = new Vector2[tPointData.Count];
                for (int i = 0; i < tPointData.Count; i++){
                    tPoints[i] = new Vector2(tPointData[i][0], tPointData[i][1]);
                }
                tPolygon.points = tPoints;
                return tPolygon;
            default:
                throw new Exception("ColliderInstaller : 不正なColliderTypeだよ「" + aArg.get<string>("colliderType") + "」");
        }
    }
}
