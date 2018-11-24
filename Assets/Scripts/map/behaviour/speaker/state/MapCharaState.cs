using System.Collections;
using System.Collections.Generic;
using UnityEngine;

partial class MapCharacter{
    public abstract class MapCharaState{
        protected MapCharacter parent;
        public MapCharaState(MapCharacter aParent){
            parent = aParent;
        }
        virtual public void update(){}
        //この状態に遷移した
        virtual public void enter(){}
        //この状態を終えた
        virtual public void exit(){}
        //移動
        virtual public void move(Vector2 aVector){}
        //調べる
        virtual public void investigate(){}
        //メニューを開く
        virtual public void openMenu(){}
    }
}