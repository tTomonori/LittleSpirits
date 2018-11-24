using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player {
    static public Player ins = new Player();
    ///名前
    public string mName;
    ///所持金
    public int mMoney;

    private Player(){

    }
    public void load(){
        mName = SaveData.getPlayerName();
        mMoney = SaveData.getMoney();
    }
}
