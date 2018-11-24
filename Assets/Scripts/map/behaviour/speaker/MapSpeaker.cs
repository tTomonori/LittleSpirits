using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MapSpeaker : MapBehaviour {
    protected Collider2D mCollider;
    ///移動
    protected void move(Vector2 aDirection, float aSpeed){
        Vector2 tVector = calcuMoveVector(aDirection, aSpeed);
        switch(mCollider.GetType().ToString()){
            case "UnityEngine.BoxCollider2D":
                break;
            default:
                throw new Exception("MapSpeaker : Colliderが「"+mCollider.GetType().ToString()+"」のspeakerの移動は定義してないよ");
        }
        float tMinSide;//当たり判定をとる間隔
        bool tPositiveX = tVector.x > 0;
        bool tPositiveY = tVector.y > 0;
        BoxCollider2D tBox = (BoxCollider2D)mCollider;
        tMinSide = (tBox.size.x < tBox.size.y) ? tBox.size.x : tBox.size.y;
        Vector2 tInterval = tVector.normalized * tMinSide;
        Vector2 tNextInterval = tInterval;
        Vector2 tRemainingVector = tVector;
        while(true){
            if (tRemainingVector == Vector2.zero) return;
            if ((tPositiveX) ? tRemainingVector.x <= 0 : tRemainingVector.x > 0) return;
            if ((tPositiveY) ? tRemainingVector.y <= 0 : tRemainingVector.y > 0) return;
            if((tPositiveX?tRemainingVector.x<tNextInterval.x:tRemainingVector.x>tNextInterval.x)
               || (tPositiveY?tRemainingVector.y<tNextInterval.y:tRemainingVector.y>tNextInterval.y))
                tNextInterval = tRemainingVector;
            MapPassType tPassType = moveAlong(tNextInterval);
            if (tPassType == MapPassType.collision) return;
            if (tPassType == MapPassType.stop) return;
            tRemainingVector -= tInterval;
        }
    }
    //引数の距離だけ移動(何かに衝突したら,そのcolliderに沿って移動)
    private MapPassType moveAlong(Vector2 aVector){
        Vector2 tLastCollisionVector;
        MapPassType tPassType = moveSlightly(aVector, out tLastCollisionVector);
        if (tPassType == MapPassType.stop) return MapPassType.stop;
        if (tLastCollisionVector == Vector2.zero) return MapPassType.passing;
        //当たり判定をとる
        Collider2D[] tColliders = Physics2D.OverlapBoxAll(
            new Vector2(((BoxCollider2D)mCollider).bounds.center.x + tLastCollisionVector.x,
                        ((BoxCollider2D)mCollider).bounds.center.y + tLastCollisionVector.y),
                        ((BoxCollider2D)mCollider).size, 0);
        foreach (Collider2D tCollider in tColliders){
            if (tCollider == mCollider) continue;
            ColliderDistance2D tCD = Physics2D.Distance(tCollider, mCollider);
            Vector2 tDistanceVector = tCD.normal;//衝突したcolliderへの方向
            if (tDistanceVector == Vector2.zero) continue;
            Vector2 tRightAngleVector = new Vector2(-tDistanceVector.y, tDistanceVector.x);//衝突したcolliderへの方向に直角
            float k = (aVector.x * tDistanceVector.y - aVector.y * tDistanceVector.x) / (tRightAngleVector.x * tDistanceVector.y - tRightAngleVector.y * tDistanceVector.x);
            Vector2 tARightAngleVector = k * tRightAngleVector;//移動先(=引数)のベクトルの<衝突したcolliderへの方向に直角>成分
            Vector2 tReCollisionVector;
            MapPassType tAlongResult = moveSlightly(tARightAngleVector, out tReCollisionVector);
            switch(tAlongResult){
                case MapPassType.passing:
                    return MapPassType.passing;
                case MapPassType.collision:
                    break;
                case MapPassType.stop:
                    return MapPassType.stop;
            }
        }
        return MapPassType.collision;
    }
    ///引数の距離だけ移動する(移動の結果(移動できたもしくは衝突したけどある程度移動した,衝突して全く移動しなかった,途中で停止した)を返す)
    /// (衝突したなら,衝突するまでの最短距離を返す)
    ///colliderをすり抜けない距離を指定すること前提
    private MapPassType moveSlightly(Vector2 aVector,out Vector2 oCollisionVector){
        float tAllowableinterval = 0.01f;//許容する衝突した物体との距離
        //最大ループ回数
        int tMaxLoopNum = 0;
        for (float tD = aVector.magnitude; tD > tAllowableinterval;){
            tD = tD / 2;
            tMaxLoopNum++;
        }
        MapPassType tCurrent=MapPassType.collision;
        Vector2 tTargetVector = aVector;//次に移動可能か確認する移動先
        Vector2 tArrivedVector = Vector2.zero;//暫定の移動先
        Vector2 tInterval = aVector / 2;//前に確認した位置から,次に確認する位置までの距離
        Vector2 tLastCollisionVector = Vector2.zero;//最後に衝突した移動距離
        //初回の衝突判定
        switch(confirmPassBy(tTargetVector)){
            case MapPassType.passing:
                //衝突することなく移動できた
                applyVector(tTargetVector);
                oCollisionVector = Vector2.zero;
                return MapPassType.passing;
            case MapPassType.collision:
                tCurrent = MapPassType.collision;
                tLastCollisionVector = tTargetVector;
                tTargetVector = tTargetVector - tInterval;
                break;
            case MapPassType.stop:
                tCurrent = MapPassType.stop;
                tArrivedVector = tTargetVector;
                tTargetVector = tTargetVector - tInterval;
                break;
        }
        //２回目以降の衝突判定
        for (int i = 1; i < tMaxLoopNum;i++){
            tInterval = new Vector2(tInterval.x / 2, tInterval.y / 2);
            switch (confirmPassBy(tTargetVector)){
                case MapPassType.passing://移動可能
                    switch(tCurrent){
                        case MapPassType.passing:
                            tArrivedVector = tTargetVector;
                            tTargetVector = tTargetVector + tInterval;
                            break;
                        case MapPassType.collision:
                            tCurrent = MapPassType.passing;
                            tArrivedVector = tTargetVector;
                            tTargetVector = tTargetVector + tInterval;
                            break;
                        case MapPassType.stop:
                            tTargetVector = tTargetVector + tInterval;
                            break;
                    }
                    break;
                case MapPassType.collision://衝突
                    tLastCollisionVector = tTargetVector;
                    switch (tCurrent){
                        case MapPassType.passing:
                            tTargetVector = tTargetVector - tInterval;
                            break;
                        case MapPassType.collision:
                            tTargetVector = tTargetVector - tInterval;
                            break;
                        case MapPassType.stop:
                            throw new Exception("MapSpeaker : 当たり判定貫通してない? stop→collision");
                    }
                    break;
                case MapPassType.stop://停止
                    switch (tCurrent){
                        case MapPassType.passing:
                            throw new Exception("MapSpeaker : 当たり判定貫通してない? passing→stop");
                        case MapPassType.collision:
                            tCurrent = MapPassType.stop;
                            tArrivedVector = tTargetVector;
                            tTargetVector = tTargetVector - tInterval;
                            break;
                        case MapPassType.stop:
                            tArrivedVector = tTargetVector;
                            tTargetVector = tTargetVector - tInterval;
                            break;
                    }
                    break;
            }
        }
        //衝突して全く移動できなかった
        if (tCurrent == MapPassType.collision){
            oCollisionVector = tLastCollisionVector;
            return MapPassType.collision;
        }
        //停止した
        if(tCurrent == MapPassType.stop){
            applyVector(tArrivedVector);
            oCollisionVector = Vector2.zero;
            return MapPassType.stop;
        }
        //何かに衝突したがある程度移動した
        applyVector(tArrivedVector);
        oCollisionVector = tLastCollisionVector;
        return MapPassType.passing;
    }
    ///引数の距離だけ移動できるか(ColliderがBoxCollider2dであること前提)
    private MapPassType confirmPassBy(Vector2 aVector){
        BoxCollider2D tBox = (BoxCollider2D)mCollider;
        //当たり判定をとる
        Collider2D[] tColliders = Physics2D.OverlapBoxAll(
            new Vector2(tBox.bounds.center.x + aVector.x, tBox.bounds.center.y + aVector.y), tBox.size, 0);
        bool tStopFlag = false;
        //当たったcolliderの確認
        foreach(Collider2D tCollider in tColliders){
            if (tCollider == tBox) continue;//自分のcollider
            MapPassType tPassType = tCollider.GetComponent<MapBehaviour>().confirmPassType(this);
            //通過不可
            if (tPassType == MapPassType.collision) return MapPassType.collision;
            //停止
            if (tPassType == MapPassType.stop) tStopFlag = true;
        }
        return (tStopFlag) ? MapPassType.stop : MapPassType.passing;
    }
    //当たり判定を無視して強制移動
    protected void forceMove(Vector2 aDirection,float aSpeed){
        applyVector(calcuMoveVector(aDirection, aSpeed));
    }
    //指定距離先へ移動
    protected void applyVector(Vector2 aVector){
        moveInMap(new Vector2(aVector.x, -aVector.y));
    }
    ///移動距離計算
    private Vector2 calcuMoveVector(Vector2 aDirection,float aSpeed){
        Vector2 tUnit = aDirection.normalized;
        return new Vector2(tUnit.x * aSpeed, tUnit.y * aSpeed);
    }
}
