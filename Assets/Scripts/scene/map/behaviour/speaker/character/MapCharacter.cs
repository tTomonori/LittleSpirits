using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class MapCharacter : MapSpeaker {
    private MapCharaAi mAi;
    private MapCharaState mState;
    //画像
    private Dictionary<string, List<Sprite>> mSprites;
    private GifAnimator mGifAnimator;
    //現在再生中のgif画像名
    private string mCurrentGifName;
    ///キャラ名
    private string mName;
    //キャラの向き
    public Direction mDirection;
    ///移動コンポーネント
    private MapBehaviourMoveComponent mMoveComponent;
    ///初期化
    public void init(string aAiName,Dictionary<string,List<Sprite>> aSprites,Vector2 aColliderSize,Direction? aDirection=null,string aName=""){
        if(mSprites!=null)return;
        mSprites = aSprites;
        mAi = createAi(aAiName);
        mState = new StandState(this);
        //名前
        mName = aName;
        gameObject.name = (mName == "") ? "NPC" : mName;
        //親要素
        gameObject.transform.parent = GameObject.Find("mapSpeakers").transform;
        //向き
        mDirection = (aDirection == null) ? new Direction(Direction.direction.Down) : (Direction)aDirection;
        //初期画像
        mGifAnimator = MyBehaviour.create<GifAnimator>();
        mGifAnimator.transform.parent = gameObject.transform;
        changeImage("stand"+mDirection.str);
        mGifAnimator.setInterval(0.2f);
        mGifAnimator.play();
        //当たり判定
        BoxCollider2D tCollider = gameObject.AddComponent<BoxCollider2D>();
        tCollider.offset = new Vector2(0, ((Vector2)aColliderSize).y / 2);
        tCollider.size = aColliderSize;
        //マップ属性
        gameObject.AddComponent<MapAttributeBehaviour>().setAttribute(MapBehaviourAttribute.Attribute.character);
        //rigidbody
        Rigidbody2D tRigid = gameObject.AddComponent<Rigidbody2D>();
        tRigid.bodyType = RigidbodyType2D.Kinematic;
        //移動コンポーネント
        mMoveComponent = gameObject.AddComponent<MapBehaviourMoveComponent>();
    }
    ///画像変更
    private void changeImage(string aName){
        if (mCurrentGifName == aName) return;
        mCurrentGifName = aName;
        mGifAnimator.setSprites(mSprites[aName]);
    }
    ///状態変更
    private void changeState(MapCharaState aState){
        mState.exit();
        mState = aState;
        mState.enter();
    }
    ///座標設定
    public void setPosition(Vector2 aPosition){
        mapPosition = aPosition;
    }
	void Start () {
        mAi = new MapPlayerAi(this);
	}
	void FixedUpdate () {
        mAi.update();
        mState.update();
	}
}
