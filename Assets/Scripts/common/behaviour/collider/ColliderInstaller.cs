using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//オブジェクトにcolliderを設定する
public class ColliderInstaller {
    public static Collider2D addCollider(GameObject aObject, Arg aArg){
        switch (aArg.get<string>("colliderType")){
            case "box"://四角形
                BoxCollider2D tBox = aObject.AddComponent<BoxCollider2D>();
                tBox.size = new Vector2(aArg.get<float>("colliderSizeX"), aArg.get<float>("colliderSizeY"));
                float tBoxOffsetX = (aArg.ContainsKey("colliderOffsetX")) ? aArg.get<float>("colliderOffsetX") : 0;
                float tBoxOffsetY = (aArg.ContainsKey("colliderOffsetY")) ? aArg.get<float>("colliderOffsetY") : 0;
                tBox.offset = new Vector2(tBoxOffsetX, tBoxOffsetY);
                return tBox;
            case "block"://四角形(offsetYを自動で +colliderHeight/2)
                BoxCollider2D tBlock = aObject.AddComponent<BoxCollider2D>();
                tBlock.size = new Vector2(aArg.get<float>("colliderSizeX"), aArg.get<float>("colliderSizeY"));
                float tBlockOffsetX = (aArg.ContainsKey("colliderOffsetX")) ? aArg.get<float>("colliderOffsetX") : 0;
                float tBlockOffsetY = (aArg.ContainsKey("colliderOffsetY")) ? aArg.get<float>("colliderOffsetY") : 0;
                tBlockOffsetY += +aArg.get<float>("colliderSizeY") / 2;
                tBlock.offset = new Vector2(tBlockOffsetX, tBlockOffsetY);
                return tBlock;
            case "polygon"://colliderの頂点指定
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
