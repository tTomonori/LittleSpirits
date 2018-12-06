using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//画像を並べて一枚のイラストにする
public class ChildAlignedSprite : ChildSprite {
    private List<MapTile> mTileList;
    public override void initImage(Arg aArg, string aDirectory){
        createChild();
        mTileList = new List<MapTile>();
        string tFileName = aDirectory+"/"+aArg.get<string>("file");
        List<List<List<int>>> tTileData = aArg.get<List<List<List<int>>>>("tiles");
        float tWidth = tTileData[0].Count;
        float tHeight = tTileData.Count;
        for (int tY = 0; tY < tTileData.Count; tY++){
            List<List<int>> tLine = tTileData[tY];
            for (int tX = 0; tX < tLine.Count; tX++){
                List<int> tChip = tLine[tX];
                if (tChip[0] == -1) continue;
                Sprite tSprite = ChipLoader.load(tFileName, tChip[0], tChip[1]);
                MapTile tTile = MyBehaviour.create<MapTile>();
                tTile.init(tSprite, new MapBehaviourAttribute(MapBehaviourAttribute.Attribute.accessory));
                tTile.transform.position = new Vector3(tX - tWidth / 2 + 0.5f, tHeight - tY - 0.5f, 0);
                mTileList.Add(tTile);
                tTile.gameObject.transform.SetParent(mChild.transform, false);
            }
        }
    }
}
