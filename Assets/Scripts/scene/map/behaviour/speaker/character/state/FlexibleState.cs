using System.Collections;
using System.Collections.Generic;
using UnityEngine;

partial class MapCharacter{
    //ユーザの操作を受付可能な状態
    public class FlexibleState : MapCharaState{
        public FlexibleState(MapCharacter aParent) : base(aParent){}
        ///調べる
        public override void search(){
            parent.search();
        }
        //メニューを開く
        public override void openMenu(){
            parent.openMenu();
        }
    }
}
