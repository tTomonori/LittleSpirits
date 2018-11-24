using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SpriteCutter {
    static public Sprite Create(Texture2D texture,Rect rect,Vector2 pivot,float pixelsPerUnit=100.0f,
                                uint extrude=0,SpriteMeshType meshType=SpriteMeshType.FullRect,
                                Vector4 border=new Vector4(),bool generateFallbackPhysicsShape=false){
        Rect tRect = new Rect(rect.xMin +1f, rect.yMin +1f, rect.width -2f, rect.height -2f);
        return Sprite.Create(texture, tRect, pivot, pixelsPerUnit-2f, extrude, meshType, border, generateFallbackPhysicsShape);
    }
}
