using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

partial class MapCharacter{
    public class MapCharaJackedAi : MapCharaAi{
        public MapCharaJackedAi(MapCharacter aParent) : base(aParent) { }
        public override void update(){
            
        }
        //AIのジャックを終える
        public void endJack(){
            parent.endJackAi();
            parent = null;
        }
    }
    //////////////////
    //MapCharacter側
    private MapCharaAi mNatureAi;
    ///AIを変更して操作権限をジャックする
    public MapCharaJackedAi jackAi(){
        if (mNatureAi != null)
            throw new Exception("MapCharaJackedAi : 既に「" + mName + "」はAIジャックされてるから今はジャックできないよ");
        mNatureAi = mAi;
        mAi = new MapCharaJackedAi(this);
        return (MapCharaJackedAi)mAi;
    }
    ///AIをジャックできるか
    public bool canJackAi(){
        return (mNatureAi == null) ? true : false;
    }
    private void endJackAi(){
        if (mNatureAi == null)
            throw new Exception("MapCharaJackedAi : AIジャックしてないのにendJackAi呼ばないで");
        mAi = mNatureAi;
        mNatureAi = null;
    }
}
