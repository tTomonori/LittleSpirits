using System.Collections;
using System.Collections.Generic;
using UnityEngine;

partial class MapCharacter{
    public class MapPlayerAi : MapCharaAi{
        public MapPlayerAi(MapCharacter aParent) : base(aParent){}
        public override void update(){
            Vector2 tVector = PlayerDragMonitor.direction;
            if(tVector!=Vector2.zero){
                parent.mState.move(tVector);
            }
        }
    }
}
