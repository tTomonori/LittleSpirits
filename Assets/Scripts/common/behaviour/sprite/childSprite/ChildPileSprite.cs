using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//複数の画像を重ねる
public class ChildPileSprite : ChildSprite {
    public override void initImage(Arg aArg, string aDirectory){
        createChild();
        foreach(Arg tData in aArg.get<List<Arg>>("pile")){
            GameObject tObject = MyBehaviour.create<MyBehaviour>().gameObject;
            tObject.transform.SetParent(mChild.gameObject.transform, false);
            ChildSprite.addSpriteObject(tObject, tData, aDirectory);
        }
    }
}
