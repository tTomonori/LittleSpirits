using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class ChildSprite : MonoBehaviour {
    static public ChildSprite addSpriteObject(GameObject aObject,Arg aArg,string aDirectory){
        ChildSprite tSprite;
        //プロパティをみて画像表示タイプを判別
        if (aArg.ContainsKey("animation"))
            tSprite = aObject.AddComponent<ChildAnimatingSprite>();
        else if (aArg.ContainsKey("tiles"))
            tSprite = aObject.AddComponent<ChildAlignedSprite>();
        else if (aArg.ContainsKey("pile"))
            tSprite = aObject.AddComponent<ChildPileSprite>();
        else
            tSprite = aObject.AddComponent<ChildOneSprite>();
        
        tSprite.initImage(aArg,aDirectory);
        return tSprite;
    }
    protected GameObject mChild;
    ///画像を表示するための子要素生成
    protected void createChild(){
        mChild = MyBehaviour.create<MyBehaviour>().gameObject;
        mChild.name = "sprites";
        mChild.transform.localPosition = new Vector3(0, 0, 0);
        mChild.transform.SetParent(gameObject.transform, false);
    }
    ///画像を設定
    abstract public void initImage(Arg aArg,string aDirectory="");
}
