using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

partial class MyBehaviour{
    ///拡大縮小率
    public IEnumerator scaleBy(float aScale,float aDuration,Action aCallback=null){
        float tDifference = 0;
        while(true){
            //今回の変化量
            float tDelta = Time.deltaTime * aScale / aDuration;
            tDifference = tDifference + tDelta;
            //指定値を超えていたら調整
            if (Math.Abs(tDifference) > Math.Abs(aScale)) tDelta = tDelta - (tDifference - aScale);
            //スケール更新
            this.scale += new Vector3(tDelta, tDelta, tDelta);
            //終了判定
            if (Math.Abs(tDifference) >= Math.Abs(aScale)) break;
            yield return null;
        }
        if (aCallback != null)
            aCallback();
    }
}