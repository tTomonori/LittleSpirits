using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//new Arg(new Dictionary<string, object>() { { "key", "value" } })
public struct Arg{
    private IDictionary arg;
    public Arg(IDictionary a){
        arg = a;
    }
    public Arg(object a = null){
        arg = new Dictionary<string, object>();
    }
    static public Arg loadJson(string fileName){
        return new Arg(MyJson.deserializeFile(fileName));
    }
    public T get<T>(string key){
        if (arg[key] is T)
            return (T)arg[key];
        if (!(this is T)){
            if((float)1 is T){
                object o = (float)(int)arg[key];
                return (T)o;
            }else if((int)1 is T){
                object o = (int)(float)arg[key];
                Debug.Log("Arg : float型の値をint型に変換しちゃってるけどいいの？");
                return (T)o;
            }
            throw new Exception(arg[key].GetType().ToString() + "型を指定した型にキャストできないよ");
        }
        Arg a = new Arg((IDictionary)arg[key]);
        arg[key] = a;
        return (T)arg[key];
    }
    public bool ContainsKey(string key){
        return arg.Contains(key);
        //return arg.ContainsKey(key);
    }
}





//public struct Arg{
//    private Dictionary<string, object> arg;
//    public Arg(Dictionary<string, object> a){
//        arg = a;
//    }
//    public Arg(object a = null){
//        arg = new Dictionary<string, object>();
//    }
//    static public Arg loadJson(string fileName){
//        return new Arg(MyJson.deserializeFile(fileName));
//    }
//    public T get<T>(string key){
//        if (arg[key] is T)
//            return (T)arg[key];
//        if(!(this is T)){
//            throw new Exception(arg[key].GetType().ToString()+"型を指定した型にキャストできないよ");
//        }
//        Arg a = new Arg((Dictionary<string, object>)arg[key]);
//        arg[key] = a;
//        return (T)arg[key];
//    }
//    public bool ContainsKey(string key){
//        return arg.ContainsKey(key);
//    }
//}