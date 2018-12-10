using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildOneSprite : ChildSprite {
    private SpriteRenderer mRenderer;
    public override void initImage(Arg aArg, string aDirectory){
        createChild();
        float tWidth = (aArg.ContainsKey("width")) ? aArg.get<float>("width") : 1;
        float tHeight = (aArg.ContainsKey("height")) ? aArg.get<float>("height") : 1;
        //画像のpivot
        Vector2 tPivot = new Vector2(aArg.ContainsKey("pivotX") ? aArg.get<float>("pivotX") : 0.5f,
                                     aArg.ContainsKey("pivotY") ? (aArg.get<float>("pivotY")) : 0.5f);
        //画像読み込み
        Sprite tSprite = ChipLoader.load(aDirectory+"/"+aArg.get<string>("file"), aArg.get<int>("x"), aArg.get<int>("y"),
                                         tWidth, tHeight, tPivot);
        //画像表示用の子ノード生成
        GameObject tSpriteNode = MyBehaviour.create<MyBehaviour>().gameObject;
        tSpriteNode.transform.SetParent(mChild.transform, false);
        mRenderer = tSpriteNode.AddComponent<SpriteRenderer>();
        mRenderer.sprite = tSprite;
    }
}
