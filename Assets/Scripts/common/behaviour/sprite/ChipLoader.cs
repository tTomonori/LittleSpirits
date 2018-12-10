using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ChipLoader {
    static private Dictionary<string, Dictionary<string, Sprite>> mSprites=new Dictionary<string, Dictionary<string, Sprite>>();
    static public Sprite load(string aFileName,float aX,float aY,float aWidht=1,float aHeight=1,Vector2? aPivot=null){
        Vector2 tPivot = (aPivot == null) ? new Vector2(0.5f, 0.5f) : (Vector2)aPivot;
        string tKey = aX.ToString() + "," + aY.ToString() + "," + aWidht.ToString() + "," + aHeight.ToString() + "," + tPivot.ToString();
        if (!mSprites.ContainsKey(aFileName)) mSprites[aFileName] = new Dictionary<string, Sprite>();
        if (mSprites[aFileName].ContainsKey(tKey)) return mSprites[aFileName][tKey];
        Sprite tOrigen = Resources.Load<Sprite>(aFileName);
        Vector3 tSize = tOrigen.bounds.size;
        Sprite tSprite = SpriteCutter.Create(tOrigen.texture, new Rect(aX * 100, (tSize.y - 1 - aY - aHeight + 1) * 100, aWidht*100, aHeight*100), 
                                       tPivot, 100, 0,
                               SpriteMeshType.FullRect);
        mSprites[aFileName][tKey] = tSprite;
        return tSprite;
    }
}
