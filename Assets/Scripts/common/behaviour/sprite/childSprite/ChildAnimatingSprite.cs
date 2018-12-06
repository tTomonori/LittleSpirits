using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//１枚の画像を一定時間ごとに切り替える
public class ChildAnimatingSprite : ChildSprite {
    public GifAnimator animator { get { return mAnimator; } }
    private GifAnimator mAnimator;
    public override void initImage(Arg aArg, string aDirectory){
        createChild();
        mAnimator = MyBehaviour.create<GifAnimator>();
        mAnimator.gameObject.transform.SetParent(mChild.transform, false);
        Arg tAnimationData = aArg.get<Arg>("animation");
        //更新インターバル
        mAnimator.setInterval(tAnimationData.get<float>("interval"));
        //画像のpivot
        Vector2 tPivot = new Vector2(aArg.ContainsKey("pivotX") ? aArg.get<float>("pivotX") : 0.5f,
                             aArg.ContainsKey("pivotY") ? (aArg.get<float>("pivotY")) : 0.5f);
        //画像
        List<Sprite> tSprites = new List<Sprite>();
        if(tAnimationData.ContainsKey("tile")){//コマを座標指定
            int tWidth = (aArg.ContainsKey("width")) ? aArg.get<int>("width") : 1;
            int tHeight = (aArg.ContainsKey("height")) ? aArg.get<int>("height") : 1;
            foreach(List<int> tXY in tAnimationData.get<List<List<int>>>("tile")){
                tSprites.Add(ChipLoader.load(aDirectory + "/" + aArg.get<string>("file"), tXY[0], tXY[1], tWidth, tHeight));
            }
        }
        else if (tAnimationData.get<string>("direction") == "horizontal"){//コマが横方向
            for (int i = 0; i < tAnimationData.get<int>("frame"); i++){
                tSprites.Add(ChipLoader.load(aDirectory+"/"+aArg.get<string>("file"),aArg.get<int>("x")+i*aArg.get<int>("width"), aArg.get<int>("y"),
                                             aArg.get<int>("width"), aArg.get<int>("height"),tPivot));
            }
        }else{//コマが縦方向
            for (int i = 0; i < tAnimationData.get<int>("frame"); i++){
                tSprites.Add(ChipLoader.load(aDirectory+"/"+aArg.get<string>("file"), aArg.get<int>("x"), aArg.get<int>("y")+i*aArg.get<int>("height"),
                                             aArg.get<int>("width"), aArg.get<int>("height"),tPivot));
            }
        }
        mAnimator.setSprites(tSprites);
        mAnimator.play();
    }
}
