using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerDragMonitor {
    static private DragMonitor mMonitor;
    static public Vector2 direction{
        get { return new Vector2(mMonitor.xVector, mMonitor.yVector); }
    }
    [RuntimeInitializeOnLoadMethod]
    static void createMonitor(){
        mMonitor = MyBehaviour.create<DragMonitor>();
        mMonitor.name = "dragMonitor";
    }
}

public class DragMonitor : MonoBehaviour{
    public float xVector = 0;
    public float yVector = 0;
    private void Update(){
        xVector = 0;
        yVector = 0;

        //上
        if (Input.GetKey(KeyCode.UpArrow))
            yVector+=1f;
        //下
        if (Input.GetKey(KeyCode.DownArrow))
            yVector -= 1f;
        //左
        if (Input.GetKey(KeyCode.LeftArrow))
            xVector -= 1f;
        //右
        if (Input.GetKey(KeyCode.RightArrow))
            xVector += 1f;
    }
}