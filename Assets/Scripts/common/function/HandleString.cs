using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HandleString {
    public static string cutOff(string aString,string aSeparator){
        int tIndex = aString.LastIndexOf(aSeparator);
        if (tIndex < 0) { return ""; }
        return aString.Substring(0, tIndex-1);
    }
}
