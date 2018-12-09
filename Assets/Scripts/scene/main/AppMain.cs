using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System;

public class AppMain : MyBehaviour {

	// Use this for initialization
	void Start () {
        MySceneManager.openScene("title", new Arg(), null, titleCallback);

        ////////////


	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void titleCallback(Arg aArg){
        string tSelect = aArg.get<string>("select");
        Debug.Log(tSelect);
        switch(tSelect){
            case "begin"://はじめから
                break;
            case "continue"://つづきから
                SaveData.load();
                Player.ins.load();
                MySceneManager.openScene("map",
                                         new Arg(new Dictionary<string, object>() { 
                    { "mapName", SaveData.getMapName() },
                    {"positionX",SaveData.getPositionX()},
                    {"positionY",SaveData.getPositionY()},
                    {"direction",SaveData.getDirection()}
                }),
                                         (Arg)=>{});
                break;
            default:
                throw new KeyNotFoundException("AppMain:「" + tSelect + "」なんて選択肢はないよ");
        }
    }
}
namespace test{
    class testest{
        
    }
}