﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapAttributeBehaviour : MonoBehaviour {
    public MapBehaviourAttribute mAttribute;
    ///マップ属性セット
    public void setAttribute(string aAttribute){
        mAttribute = new MapBehaviourAttribute(aAttribute);
    }
    public void setAttribute(MapBehaviourAttribute.Attribute aAttribute){
        mAttribute = new MapBehaviourAttribute(aAttribute);
    }
    ///引数のマップ属性が自分を通過できるか
    virtual public MapPassType confirmPassType(MapAttributeBehaviour aBehaviour){
        if (mAttribute.getAttributeType() == MapBehaviourAttribute.AttributeType.mapEvent){
            //自分がeventのcolliderの時
            return gameObject.GetComponent<MapEventTrigger>().confirmPassType(aBehaviour);
        }
        else{
            //自分がeventのcollider以外の時
            if (aBehaviour.mAttribute.canPass(mAttribute)) return MapPassType.passing;
            else return MapPassType.collision;
        }
    }
}
