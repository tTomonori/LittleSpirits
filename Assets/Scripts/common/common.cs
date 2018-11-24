using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public delegate void SendArg(Arg arg);
public delegate void SendMessage(Message message);
public delegate bool FilterMessage(Message message);
public delegate void ReturnArg(Arg? arg = null);
public delegate void VoidFunction();

public struct Direction{
    public direction value;
    public string str{
        get { return Enum.GetName(typeof(direction), value); }
    }
    public enum direction{
        Up,Down,Left,Right,none
    }
    public Direction(direction aDirection){
        value = aDirection;
    }
    public Direction(string aDirection){
        value = (direction)Enum.Parse(typeof(direction), aDirection, true);
    }
    public Direction(Vector2 aDirection){
        if(aDirection==Vector2.zero){
            value = direction.none;
            return;
        }
        if(Math.Abs(aDirection.x)>Math.Abs(aDirection.y)){
            if (aDirection.x < 0) value = direction.Left;
            else value = direction.Right;
            return;
        }else{
            if (aDirection.y < 0) value = direction.Down;
            else value = direction.Up;
            return;
        }
    }
}