using System.Collections;
using System.Collections.Generic;
using UnityEngine;

partial class MapCharacter{
    public class MovingState : MapCharaState{
        public MovingState(MapCharacter aParent) : base(aParent){}
        private bool mMoveFlag = false;//移動入力されたか記憶
        //移動
        public override void move(Vector2 aVector){
            mMoveFlag = true;
            Direction tDirection = new Direction(aVector);
            parent.mDirection = tDirection;
            parent.changeImage("move" + tDirection.str);
            //parent.move(aVector, 0.1f);
            parent.mMoveComponent.move(aVector, 0.1f);
        }
        public override void update(){
            if(mMoveFlag){
                mMoveFlag = false;
                return;
            }
            //移動しなかった
            parent.changeState(new StandState(parent));
        }
    }
}
