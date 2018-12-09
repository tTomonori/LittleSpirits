using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapObject : MapSpeaker {
    private List<MapTile> mTiles=new List<MapTile>();
    public void addTile(MapTile aTile){
        mTiles.Add(aTile);
        aTile.transform.SetParent(gameObject.transform, false);
        //aTile.transform.localPosition = new Vector3(aOffsetX, aOffsetY - 0.5f, aOffsetY - 0.5f);
    }
}
