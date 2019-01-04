using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HandleString {
    ///指定文字列中の最後のseparatorより前の部分を抽出
    public static string cutOff(string aString,string aSeparator){
        int tIndex = aString.LastIndexOf(aSeparator);
        if (tIndex < 0) { return ""; }
        return aString.Substring(0, tIndex-1);
    }
}
