using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTile : MyBehaviour {
    private MapBehaviourAttribute mAttribute;
    public MapBehaviourAttribute attribute{
        get { return mAttribute; }
    }
    //アニメーションなしの初期化
    public void init(Sprite aSprite,MapBehaviourAttribute aAttribute){
        if (mAttribute != null) return;
        mAttribute = aAttribute;
        SpriteRenderer tRenderer = gameObject.AddComponent<SpriteRenderer>();
        tRenderer.sprite = aSprite;
    }
    //アニメーションありの初期化
    public void init(List<Sprite> aSprite,float aInterval,MapBehaviourAttribute aAttribute){
        if (mAttribute != null) return;
        mAttribute = aAttribute;
        GifAnimator tGif = gameObject.AddComponent<GifAnimator>();
        tGif.setSprites(aSprite);
        tGif.setInterval(aInterval);
        tGif.play();
    }
}
