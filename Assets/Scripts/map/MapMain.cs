using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMain : MonoBehaviour{
    public LocalMap mMap;
	// Use this for initialization
	void Start () {
        Arg tArg = MySceneManager.getArg("map");
        mMap = LocalMapCreater.create(tArg.get<string>("mapName"));
        //ユーザ
        MapCharacter tPlayer = MapCharacterCreater.create("player", "player/player",new Direction(tArg.get<string>("direction")), "player");
        tPlayer.setPosition(new Vector2(tArg.get<int>("positionX"), tArg.get<int>("positionY")));
        mMap.addCharacter(tPlayer);

        GameObject tCamera = GameObject.Find("MapCamera");
        tCamera.transform.parent = tPlayer.transform;
        tCamera.transform.localPosition = new Vector3(0, 0, -100);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
