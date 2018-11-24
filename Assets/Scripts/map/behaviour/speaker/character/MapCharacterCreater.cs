using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MapCharacterCreater {
    static public MapCharacter create(string aAiName,string aSpriteFileName,Direction aDirection,string aName="",Vector2? aColliderSize=null){
        if (aColliderSize == null) aColliderSize = new Vector2(0.6f, 0.3f);
        //画像
        Sprite tOrigen = Resources.Load<Sprite>("Sprites/character/" + aSpriteFileName);
        Sprite[,] tSprites = new Sprite[3, 4];
        for (int x = 0; x < 3;x++){
            for (int y = 0; y < 4;y++){
                tSprites[x, y] = SpriteCutter.Create(tOrigen.texture, new Rect(x * 100, y * 100, 100, 100), new Vector2(0.5f, 0f), 80);
            }
        }
        Dictionary<string, List<Sprite>> tDic = new Dictionary<string, List<Sprite>>();
        tDic["standUp"] = new List<Sprite>() { tSprites[1, 0] };
        tDic["standDown"] = new List<Sprite>() { tSprites[1, 3] };
        tDic["standLeft"] = new List<Sprite>() { tSprites[1, 2] };
        tDic["standRight"] = new List<Sprite>() { tSprites[1, 1] };
        tDic["moveUp"] = new List<Sprite>() { tSprites[0, 0], tSprites[1, 0], tSprites[2, 0], tSprites[1, 0] };
        tDic["moveDown"] = new List<Sprite>() { tSprites[0, 3], tSprites[1, 3], tSprites[2, 3], tSprites[1, 3] };
        tDic["moveLeft"] = new List<Sprite>() { tSprites[0, 2], tSprites[1, 2], tSprites[2, 2], tSprites[1, 2] };
        tDic["moveRight"] = new List<Sprite>() { tSprites[0, 1], tSprites[1, 1], tSprites[2, 1], tSprites[1, 1] };
        MapCharacter tCharacter = MyBehaviour.create<MapCharacter>();
        tCharacter.init(aAiName, tDic, (Vector2)aColliderSize,aDirection,aName);
        return tCharacter;
    }
}
