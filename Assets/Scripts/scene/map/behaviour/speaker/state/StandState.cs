using System.Collections;
using System.Collections.Generic;
using UnityEngine;

partial class MapCharacter{
    public class StandState : MapCharaState{
        public StandState(MapCharacter aParent) : base(aParent){}
        //移動
        public override void move(Vector2 aVector){
            parent.changeState(new MovingState(parent));
            parent.mState.move(aVector);
        }
        public override void enter(){
            parent.changeImage("stand" + parent.mDirection.str);
        }
    }
}
