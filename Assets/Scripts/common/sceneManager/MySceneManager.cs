using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class MySceneManager {
    static MySceneManager(){
        //新しくシーンを読み込んだ時
        SceneManager.sceneLoaded += (aScene, aMode) => {
            //SceneDataにSceneを記憶
            MySceneManager.findSceneData(aScene.name).scene = aScene;
            //カメラノードのAudioListenerを消す
            foreach (GameObject tObject in aScene.GetRootGameObjects()){
                AudioListener tAudioListener = tObject.GetComponent<AudioListener>();
                if (tAudioListener != null){
                    GameObject.Destroy(tAudioListener);
                    return;
                }
            }
        };
    }
    public class SceneData{
        public SceneData(string aName,Arg aArg,SendArg aCallback){
            name = aName;
            arg = aArg;
            callback = aCallback;
            pausedBehaviour = new List<MonoBehaviour>();
        }
        public string name;
        public Arg arg;
        public SendArg callback;
        public Scene scene;
        public List<MonoBehaviour> pausedBehaviour;
    }
    ///開いている全てのシーン
    static private List<SceneData> mScenes = new List<SceneData>();
    ///最前面にあるシーン
    static private SceneData mFrontmostScene{
        get { return mScenes[mScenes.Count - 1]; }
    }
    ///指定した名前のシーンのデータを探す
    static private SceneData findSceneData(string aName){
        foreach(SceneData tData in mScenes){
            if(tData.name == aName){
                return tData;
            }
        }
        throw new KeyNotFoundException("SceneManager:「"+aName+"」なんて名前のシーンはないよ");
    }
    ///シーンを開く
    static public void openScene(string aName, Arg aArg, SendArg aCallback){
        SceneData tData=new SceneData(aName,aArg,aCallback);
        mScenes.Add(tData);
        SceneManager.LoadSceneAsync(aName, LoadSceneMode.Additive);
    }
    ///シーンを閉じる
    static public void closeScene(string aName,Arg aArg){
        SceneData tData = mFrontmostScene;
        if(tData.name != aName)
           throw new KeyNotFoundException("SceneManager:最前面でないシーン「" + aName + "」は閉じちゃダメ");
        SendArg tCallback = tData.callback;
        mScenes.RemoveAt(mScenes.Count - 1);
        SceneManager.UnloadSceneAsync(aName);
        tCallback(aArg);
    }
    ///引数を受け取る
    static public Arg getArg(string aName){
        SceneData tData = findSceneData(aName);
        return tData.arg;
    }
    ///シーンを停止する
    static public void pauseScene(string aName){
        SceneData tData = findSceneData(aName);
        foreach(GameObject tObject in tData.scene.GetRootGameObjects()){
            foreach(MonoBehaviour tBehaviour in tObject.GetComponentsInChildren<MonoBehaviour>()){
                if (tBehaviour.enabled == false) continue;
                tBehaviour.enabled = false;
                tData.pausedBehaviour.Add(tBehaviour);
            }
        }
    }
    ///シーンを再生する
    static public void playScene(string aName){
        SceneData tData = findSceneData(aName);
        foreach(MonoBehaviour tBehaviour in tData.pausedBehaviour){
            tBehaviour.enabled = true;
        }
        tData.pausedBehaviour.Clear();
    }
}