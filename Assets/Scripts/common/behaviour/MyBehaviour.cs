using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class MyBehaviour : MonoBehaviour {
    static private MyBehaviour instance;
    static private MyBehaviour ins{
        get{
            if (instance == null){
                instance = MyBehaviour.create<MyBehaviour>();
                instance.name = "MyBehaviourInstance";
            }
            return instance;
        }
    }
    ///Behaviour生成
    static public T create<T>() where T : MonoBehaviour{
        return new GameObject().AddComponent<T>();
    }
    ///タイムアウト設定
    public void setTimeout(float aSeconds,VoidFunction aFunction){
        StartCoroutine(coroutine(aSeconds, aFunction));
    }
    private IEnumerator coroutine(float aSeconds, VoidFunction aFunction){
        yield return new WaitForSeconds(aSeconds);
        aFunction();
    }
    //子ルーチン実行
    static public void runCoroutine(IEnumerator aCoroutine){
        ins.StartCoroutine(aCoroutine);
    }
    ///削除する
    public void delete(){
        Destroy(gameObject);
    }
    //座標
    public float positionX{
        get { return gameObject.transform.position.x; }
        set {
            Vector3 tPosition = gameObject.transform.position;
            gameObject.transform.position = new Vector3(value, tPosition.y, tPosition.z);
        }
    }
    public float positionY{
        get { return gameObject.transform.position.y; }
        set{
            Vector3 tPosition = gameObject.transform.position;
            gameObject.transform.position = new Vector3(tPosition.x, value, tPosition.z);
        }
    }
    public float positionZ{
        get { return gameObject.transform.position.z; }
        set{
            Vector3 tPosition = gameObject.transform.position;
            gameObject.transform.position = new Vector3(tPosition.x, tPosition.y, value);
        }
    }
    //スケール
    public Vector3 scale{
        get { return gameObject.transform.localScale; }
        set { gameObject.transform.localScale = value; }
    }
}
