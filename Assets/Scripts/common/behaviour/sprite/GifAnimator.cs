using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class GifAnimator : MonoBehaviour {
    ///画像を貼るコンポーネント
    SpriteRenderer mRenderer;
    ///変更する画像
    private List<Sprite> mSprites;
    ///変更する画像の数
    private int mSpriteCount;
    ///現在の画像のindex
    private int mCurrentSpriteNum;
    ///アニメーションを再生中か
    private bool mPlayFlag = false;
    ///画像変更のインターバル
    private float mInterval = 0;
    ///現在の画像にしてからの経過時間
    private float mDelta = 0;
    private void Update(){
        if (!mPlayFlag) return;
        mDelta += Time.deltaTime;
        if (mDelta < mInterval) return;
        while (mDelta >= mInterval){
            mDelta -= mInterval;
            mCurrentSpriteNum = (mCurrentSpriteNum + 1) % mSpriteCount;
            mRenderer.sprite = mSprites[mCurrentSpriteNum];
        }
    }
    ///アニメーションさせる画像を設定
    public void setSprites(List<Sprite> aSprites){
        if (mRenderer == null) mRenderer = gameObject.GetComponent<SpriteRenderer>();
        mSprites = aSprites;
        mSpriteCount = mSprites.Count;
        mCurrentSpriteNum = 0;
        mRenderer.sprite = aSprites[0];
        mDelta = 0;
    }
    ///画像変更インターバル設定
    public void setInterval(float aInterval){
        if (aInterval <= 0) return;
        mInterval = aInterval;
        mDelta = 0;
    }
    ///再生
    public void play(){
        if (mInterval <= 0){
            Debug.Log("GifAnimator : 再生する前にインターバル設定してね ( interval = " + mInterval.ToString() + " )");
            return;
        }
        mPlayFlag = true;
    }
    ///停止
    public void pause(){
        mPlayFlag = false;
    }
}