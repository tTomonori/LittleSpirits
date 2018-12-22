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
    ///正面を調べる
    private void search(){
        BoxCollider2D tMyCollider = gameObject.GetComponent<BoxCollider2D>();
        float tCenterX = tMyCollider.bounds.center.x;
        float tCenterY = tMyCollider.bounds.center.y;
        switch(mDirection.value){
            case Direction.direction.Up:tCenterY += 0.5f + tMyCollider.size.y / 2;break;
            case Direction.direction.Down:tCenterY -= 0.5f + tMyCollider.size.y / 2;break;
            case Direction.direction.Left:tCenterX -= 0.5f + tMyCollider.size.x / 2;break;
            case Direction.direction.Right:tCenterX += 0.5f + tMyCollider.size.x / 2;break;
        }
        //正面にあるcolliderを取得
        Collider2D[] tColliders = Physics2D.OverlapBoxAll(new Vector2(tCenterX,tCenterY),
                                                          new Vector2(tMyCollider.size.x, tMyCollider.size.x), 0);
        foreach(Collider2D tCollider in tColliders){
            MapSearchedBehaviour tSearched = tCollider.GetComponent<MapSearchedBehaviour>();
            if (tSearched == null) continue;
            tSearched.searched(this);
            return;
        }
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
