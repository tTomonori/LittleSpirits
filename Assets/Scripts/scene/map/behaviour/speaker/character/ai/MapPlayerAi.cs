using System.Collections;
using System.Collections.Generic;
using UnityEngine;

partial class MapCharacter{
    public class MapPlayerAi : MapCharaAi{
        public MapPlayerAi(MapCharacter aParent) : base(aParent){}
        ///ユーザの入力監視class
        private MapUserInput mInput = GameObject.Find("mapUserInput").GetComponent<MapUserInput>();
        public override void update(){
            //移動
            Vector2 tVector = PlayerDragMonitor.direction;
            if(tVector!=Vector2.zero){
                parent.mState.move(tVector);
            }
            //調べる
            if(Input.GetKeyDown(KeyCode.Z)){
                parent.mState.search();
            }else if(mInput.mMenu){//メニューを開く
                parent.mState.openMenu();
            }
        }
    }
}
