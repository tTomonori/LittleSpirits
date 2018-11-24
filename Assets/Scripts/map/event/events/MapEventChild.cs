using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MapEventChild : MapEvent{
    private MapEventParent mParent;
    public MapEventChild(MapEventParent aParent){
        mParent = aParent;
    }
}
