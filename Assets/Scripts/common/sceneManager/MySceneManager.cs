﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public static class MySceneManager {
    static MySceneManager(){
        //新しくシーンを読み込んだ時
        SceneManager.sceneLoaded += (aScene, aMode) => {
            SceneData tData = findSceneData(aScene.name);
            //SceneDataにSceneを記憶
            tData.scene = aScene;
            //カメラノードのAudioListenerを消す
            foreach (GameObject tObject in aScene.GetRootGameObjects()){
                AudioListener tAudioListener = tObject.GetComponent<AudioListener>();
                if (tAudioListener != null){
                    GameObject.Destroy(tAudioListener);
                    break;
                }
            }
            //開いた時のcallback
            if (tData.opened != null)
                tData.opened(aScene);
        };
        //シーンが閉じられた時
        SceneManager.sceneUnloaded += (aScene) =>{
            string tName = aScene.name;
            for (int i = 0; i < mScenes.Count; i++){
                SceneData tData = mScenes[i];
                if (tData.name != tName) continue;
                mScenes.RemoveAt(i);
                //閉じた時のcallback
                tData.closed(tData.arg);
            }
        };
    }
    public class SceneData{
        public SceneData(string aName,Arg aArg,Action<Scene> aOpened=null,Action<Arg> aClosed=null){
            name = aName;
            arg = aArg;
            opened = aOpened;
            closed = aClosed;
            pausedBehaviour = new List<MonoBehaviour>();
        }
        public string name;//シーンの名前
        public Arg arg;//開いたシーンに渡す引数
        public Action<Scene> opened;//シーンを開いた時のcallback
        public Action<Arg> closed;//シーンを閉じた時のcallback
        public Scene scene;//開いたシーン
        public List<MonoBehaviour> pausedBehaviour;//停止させたbehaviour
    }
    ///開いている全てのシーン
    static private List<SceneData> mScenes = new List<SceneData>();
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
    static public void openScene(string aName, Arg aArg, Action<Scene> aOpened = null, Action<Arg> aClosed = null){
        SceneData tData=new SceneData(aName,aArg,aOpened,aClosed);
        mScenes.Add(tData);
        SceneManager.LoadSceneAsync(aName, LoadSceneMode.Additive);
        //SceneManager.LoadScene(aName,LoadSceneMode.Additive);
    }
    ///シーンを閉じる
    static public void closeScene(string aName,Arg aArg,Action<Arg> aClosed=null){
        for (int i = 0; i < mScenes.Count;i++){
            SceneData tData = mScenes[i];
            if (tData.name != aName) continue;
            tData.arg = aArg;
            if (aClosed != null){
                if (tData.closed != null)
                    Debug.Log("SceneManager:「" + aName + "」ってシーンを閉じた時のcallbak上書きしちゃった");
                tData.closed = aClosed;
            }
            SceneManager.UnloadSceneAsync(aName);
            //SceneManager.UnloadScene(aName);
            return;
        }
        throw new KeyNotFoundException("SceneManager:「" + aName + "」なんて名前のシーンはないから閉じれない");
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