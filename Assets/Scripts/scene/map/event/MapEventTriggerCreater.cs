using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class MapEventTriggerCreater {
    static public MapEventTrigger create(Arg aData){
        MapEventTrigger tTrigger = MyBehaviour.create<MapEventTrigger>();
        //collider
        ColliderInstaller.addCollider(tTrigger.gameObject, aData.get<Arg>("collider"));
        Collider2D[] tColliders = tTrigger.gameObject.GetComponents<Collider2D>();
        foreach(Collider2D tCollider in tColliders){
            tCollider.isTrigger = true;
        }
        //mapBehaviourAttribute
        tTrigger.gameObject.AddComponent<MapAttributeBehaviour>().setAttribute(MapBehaviourAttribute.Attribute.empty);
        //イベント設定
        tTrigger.set((aData.ContainsKey("enter")) ? new MapEvent(aData.get<Arg>("enter")) : null,
                     (aData.ContainsKey("exit")) ? new MapEvent(aData.get<Arg>("exit")) : null,
                     (aData.ContainsKey("stay")) ? new MapEvent(aData.get<Arg>("stay")) : null,
                     aData.get<Arg>("trigger"), aData.get<bool>("destroy"));
        return tTrigger;
    }
}
