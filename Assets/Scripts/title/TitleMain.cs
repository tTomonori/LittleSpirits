using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleMain : MyBehaviour {
    
	// Use this for initialization
	void Start () {
        Subject.addObserver(new Observer("TitleMenu", (aMessage) =>{
            if (aMessage.name!="selectTitleMenu")
                return;
            switch(aMessage.getParameter<string>("key")){
                case "begin"://初めから
                    TitleMenuChoice tChoiceb = GameObject.Find("begin").GetComponent<TitleMenuChoice>();
                    tChoiceb.selected();
                    Subject.removeObserver("TitleMenu");
                    setTimeout(1.5f, () =>{
                        MySceneManager.closeScene("title", new Arg(new Dictionary<string, object>() { { "select", "begin" } }));
                    });
                    break;
                case "continue"://続きから
                    TitleMenuChoice tChoicec = GameObject.Find("continue").GetComponent<TitleMenuChoice>();
                    tChoicec.selected();
                    Subject.removeObserver("TitleMenu");
                    setTimeout(1.5f, () => {
                        MySceneManager.closeScene("title", new Arg(new Dictionary<string, object>() { { "select", "continue" } }));
                    });
                    break;
                case "option"://オプション
                    break;
                default:break;
            }
        }));
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
