using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBehaviour : MyBehaviour {
    ///Behaviour属性
    public MapBehaviourAttribute mBehaviourAttribute;
    private float mZIndex = 0.5f;
    public float zIndex{
        get { return mZIndex; }
        set{
            mZIndex = value;
            positionZ = -mapPosition.y + zIndex;
        }
    }
    public Vector2 mapPosition{
        get { return new Vector2(positionX,-positionY); }
        set{
            positionX = value.x;
            positionY = -value.y;
            positionZ = -value.y + zIndex;
        }
    }
    public void moveInMap(Vector2 aDirection){
        Vector2 tMapPosition = mapPosition;
        mapPosition = new Vector2(tMapPosition.x + aDirection.x, tMapPosition.y + aDirection.y);
    }
    // virtual public MapPassType confirmPassType(MapBehaviour aBehaviour){
    //    if (aBehaviour.mBehaviourAttribute.canPass(mBehaviourAttribute)) return MapPassType.passing;
    //    else return MapPassType.collision;
    //}
}
public enum MapPassType{
    passing,collision,stop
}