using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTrout : MapBehaviour {
    private List<MapTile> mTiles=new List<MapTile>();
    private BoxCollider2D mCollider;
    private void Awake(){
        mBehaviourAttribute = new MapBehaviourAttribute("none");
        zIndex = 10;
        mCollider = gameObject.AddComponent<BoxCollider2D>();
        mCollider.size = new Vector2(1, 1);
    }
    ///マスの画像を追加
    //public void addTile(MapTile aTile){
    //    mTiles.Add(aTile);
    //    aTile.transform.parent = this.gameObject.transform;
    //    aTile.transform.localPosition = new Vector3(0, 0, 0);
    //    //タイルを重ねて地形属性更新
    //    mBehaviourAttribute.pile(aTile.attribute);
    //    if (mBehaviourAttribute.attribute==MapBehaviourAttribute.Attribute.flat
    //        || mBehaviourAttribute.attribute==MapBehaviourAttribute.Attribute.bridge)
    //        mCollider.enabled = false;
    //    else
    //        mCollider.enabled = true;
    //}
}
