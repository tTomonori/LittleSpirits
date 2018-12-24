using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiButton : MyBehaviour {
    [SerializeField] protected string mName;
    [SerializeField] protected Dictionary<string, object> mParameters=new Dictionary<string, object>();
    [SerializeField] protected string mGroup=null;
    private void OnMouseUp(){
        Subject.sendMessage(new Message(mName, new Arg(mParameters), mGroup));

        StartCoroutine(scaleBy(0.1f, 0.05f,() => {
            StartCoroutine(scaleBy(-0.1f, 0.1f));
        }));
    }
}
