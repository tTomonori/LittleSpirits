using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

partial class MapCharacter{
    public abstract class MapCharaAi{
        protected MapCharacter parent;
        public MapCharaAi(MapCharacter aParent){
            parent = aParent;
        }
        abstract public void update();
    }
    //文字列からAI特定
    private MapCharaAi createAi(string aAiName){
        switch(aAiName){
            case "player":
                return new MapPlayerAi(this);
            default:
                throw new Exception("MapCharaAi : 「"+aAiName+"」なんて名前のAIはないよ");
        }
    }
}