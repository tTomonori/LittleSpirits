using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

static public class ConversationWindow  {
    static public void show(Arg aArg,Action aCallback){
        aCallback();
        //MySceneManager.openScene("conversation", aArg, (_) =>{
        //    aCallback();
        //});
    }
}
