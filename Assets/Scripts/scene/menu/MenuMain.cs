using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMain : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Subject.addObserver(new Observer("MenuMain",(message) => {
            if (message.name == "back")
                MySceneManager.closeScene("mainMenu",new Arg());
        },"menu"));
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    ~MenuMain(){
        Subject.removeObserver("MenuMain");
    }
}
