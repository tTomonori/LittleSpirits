using System.Collections;
using System.Collections.Generic;
using UnityEngine;

partial class MapCharacter{
    ///正面を調べる
    private void search(){
        BoxCollider2D tMyCollider = gameObject.GetComponent<BoxCollider2D>();
        float tCenterX = tMyCollider.bounds.center.x;
        float tCenterY = tMyCollider.bounds.center.y;
        switch(mDirection.value){
            case Direction.direction.Up:tCenterY += 0.5f + tMyCollider.size.y / 2;break;
            case Direction.direction.Down:tCenterY -= 0.5f + tMyCollider.size.y / 2;break;
            case Direction.direction.Left:tCenterX -= 0.5f + tMyCollider.size.x / 2;break;
            case Direction.direction.Right:tCenterX += 0.5f + tMyCollider.size.x / 2;break;
        }
        //調べる範囲
        Vector2 tSearchSize = new Vector2(tMyCollider.size.x,
                                        (mDirection.value == Direction.direction.Left || mDirection.value == Direction.direction.Right) 
                                          ? tMyCollider.size.y : tMyCollider.size.x);
        //正面にあるcolliderを取得
        Collider2D[] tColliders = Physics2D.OverlapBoxAll(new Vector2(tCenterX,tCenterY), tSearchSize, 0);
        foreach(Collider2D tCollider in tColliders){
            MapSearchedBehaviour tSearched = tCollider.GetComponent<MapSearchedBehaviour>();
            if (tSearched == null) continue;
            tSearched.searched(this);
            return;
        }
    }
    ///メニューを開く
    private void openMenu(){
        //シーン停止
        MySceneManager.pauseScene("map");
        //メニューを開く
        MySceneManager.openScene("mainMenu",new Arg(),null,(aArg) => {
            MySceneManager.playScene("map");
        });
    }
}