using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapUserInput : MonoBehaviour {
    private bool mInputMenu = false;
    ///メニューを開く
    public bool mMenu{
        get {
            if (Input.GetKeyDown(KeyCode.X))
                return true;
            bool tInput = mInputMenu;
            mInputMenu = false;
            return tInput;
            }
    }

	void Start () {
        Subject.addObserver(new Observer("mapUserInput", (message) =>{
            if(message.name=="menuButton")
                mInputMenu = true;
        },"map"));
	}
    private void LateUpdate(){
        mInputMenu = false;
    }
    ~MapUserInput(){
        Subject.removeObserver("mapUserInput");
    }
}
