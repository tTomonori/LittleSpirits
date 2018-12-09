using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

static public class HandleAsync {
    //リストの要素に対して順番に処理する
    static public void processInOrder<T>(List<T> aList,Action<T,Action> aProcess,Action aCallback){
        int tLength = aList.Count;
        //イベントを1つずつ実行
        Action<int> fNext = (_) => { };
        fNext = (i) => {
            if (tLength <= i){//全ての処理終了
                aCallback();
                return;
            }
            //一つの要素に処理実行
            aProcess(aList[i], () => { fNext(i + 1); });
        };
        fNext(0);
    }
}
