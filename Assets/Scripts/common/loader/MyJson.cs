﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public static class MyJson {
    static public Dictionary<string,object> deserializeFile(string fileName){
        string jsonString = File.ReadAllText(fileName);
        return deserialize(jsonString);
    }
    static public Dictionary<string,object> deserializeResourse(string fileName){
        string jsonString = ((TextAsset)Resources.Load(fileName)).text;
        return deserialize(jsonString);
    }
    static public Dictionary<string,object> deserialize(string jsonString){
        return new Parser().parse(jsonString);
    }
    private class Parser{
        private StringReader reader;
        ///次の文字を読む
        private char nextChar{
            get {
                checkEnd();
                return Convert.ToChar(reader.Read()); 
            }
        }
        ///現在の位置の文字を読む
        private char currentChar{
            get { return Convert.ToChar(reader.Peek()); }
        }
        ///位置文字読み進める
        private void readOneChar() {
            checkEnd();
            reader.Read(); 
        }
        ///ファイルが読み終わっているか判定する(読み終わっていたらエラーを吐く)
        private void checkEnd() { if (reader.Peek() == -1) throw new Exception("不正なjson文字列 : fraudulent file end"); }
        ///次の、意味を持つcharの位置まで読み進める
        private void readToNextSense(){
            while (true){
                switch (currentChar){
                    case ' ': readOneChar(); continue;
                    case '\n': readOneChar(); continue;
                    case '/': reader.ReadLine(); continue;
                    default:
                        if(reader.Peek()==13){//改行文字
                            readOneChar();continue;
                        }
                        return;
                }
            }
        }
        ///次の、意味を持つcharを読む(readerの位置はその次)
        private char nextSense(){
            readToNextSense();
            return nextChar;
        }
        ///次の、意味を持つcharを返す(readerの位置はその文字)
        private char currenSense(){
            readToNextSense();
            return currentChar;
        }
        ///指定した文字の位置まで読み進める(次の、意味を持つcharが指定した文字でないならエラーを吐く)(readerの位置はその文字)
        private void search(char c){
            readToNextSense();
            char sense = currentChar;
            if (sense != c)
                throw new Exception("不正なjson文字列 : invalid char 「" + sense + "」, expected 「"+c+"」");
        }
        private void search(string s){
            readToNextSense();
            char sense = currentChar;
            for (int i = 0; i < s.Length;i++){
                if (sense == s[i]) return;
            }
            throw new Exception("不正なjson文字列 : invalid char 「" + sense + "」, expected 「"+s+"」");
        }
        ///文字列を読む
        private string readString(){
            search('"');
            readOneChar();
            string s = "";
            while(true){
                switch(currentChar){
                    case '\\':
                        readOneChar();
                        s += nextChar;
                        break;
                    case '"':
                        readOneChar();
                        return s;
                    default:
                        s += nextChar;
                        break;
                }
            }
        }
        ///数字を読む
        private object readNumber(){
            search("0123456789+-");
            bool signFlag = false;
            bool pointFlag = false;
            string s = "";
            while(true){
                switch(currentChar){
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                        s += nextChar;
                        break;
                    case '+':
                    case '-':
                        if(signFlag)
                            throw new Exception("不正なjson文字列 : invalid number include 「"+currentChar+"」");
                        s += nextChar;
                        break;
                    case '.':
                        if(pointFlag)
                            throw new Exception("不正なjson文字列 : invalid number include two「.」");
                        pointFlag = true;
                        s += nextChar;
                        break;
                    default:
                        if(pointFlag){
                            float f;
                            float.TryParse(s, out f);
                            return f;
                        }else{
                            int i;
                            int.TryParse(s, out i);
                            return i;
                        }
                }
                signFlag = true;
            }
        }
        ///boolを読む
        private object readBool(){
            search("tf");
            switch(currentChar){
                case 't':
                    if (nextChar != 't') throw new Exception("不正なjson文字列 : invalid value start from 「t」");
                    if (nextChar != 'r') throw new Exception("不正なjson文字列 : invalid value start from 「t」");
                    if (nextChar != 'u') throw new Exception("不正なjson文字列 : invalid value start from 「t」");
                    if (nextChar != 'e') throw new Exception("不正なjson文字列 : invalid value start from 「t」");
                    return true;
                case 'f':
                    if (nextChar != 'f') throw new Exception("不正なjson文字列 : invalid value start from 「f」");
                    if (nextChar != 'a') throw new Exception("不正なjson文字列 : invalid value start from 「f」");
                    if (nextChar != 'l') throw new Exception("不正なjson文字列 : invalid value start from 「f」");
                    if (nextChar != 's') throw new Exception("不正なjson文字列 : invalid value start from 「f」");
                    if (nextChar != 'e') throw new Exception("不正なjson文字列 : invalid value start from 「f」");
                    return false;
                default:
                    throw new Exception("想定不能なエラー : 「"+currentChar+"」 != t nor f");
            }
        }
        ///Listを読む
        private object readList(){
            search('[');
            readOneChar();
            if (currenSense() == ']')
                return new List<object>();
            
            object o = readValue();
            Type firstElementType = o.GetType();
            Type listType = Type.GetType("System.Collections.Generic.List`1["+firstElementType.ToString()+"]");
            IList valueList = (IList)Activator.CreateInstance(listType);
            valueList.Add(o);

            object e;
            while(true){
                switch(currenSense()){
                    case ',':
                        readOneChar();
                        e = readValue();
                        if (e.GetType() != firstElementType) goto readObjectList;
                        valueList.Add(e);
                        break;
                    case ']':
                        readOneChar();
                        return valueList;
                    default:
                        throw new Exception("不正なjson文字列 : invalid list separator 「"+currentChar+"」");
                }
            }
        readObjectList:
            //リスト内の型が統一されていない
            List<object> objectList = new List<object>();
            foreach(object v in valueList){
                objectList.Add(v);
            }
            objectList.Add(e);
            while (true){
                switch (currenSense()){
                    case ',':
                        readOneChar();
                        e = readValue();
                        objectList.Add(e);
                        break;
                    case ']':
                        readOneChar();
                        return objectList;
                    default:
                        throw new Exception("不正なjson文字列 : invalid list separator 「" + currentChar + "」");
                }
            }
        }
        ///Dictionary(string,object)を読む
        private object readDictionaryOfObject(){
            search('{');
            readOneChar();
            Dictionary<string, object> dic = new Dictionary<string, object>();
            if (currenSense() == '}'){
                readOneChar();//}を読み飛ばす
                return dic;
            }
            while(true){
                string key = readString();
                //:を読み飛ばす
                search(':');
                readOneChar();
                object value = readValue();
                dic[key] = value;
                if(currenSense()==','){
                    readOneChar();//,を読み飛ばす
                }else if (currenSense() == '}'){
                    readOneChar();//}を読み飛ばす
                    return dic;
                }
                else{
                    throw new Exception("不正なjson文字列 : not found value separator");
                }
            }
        }
        ///Dictionary(string,)を読む
        private object readDictionary(){
            search('{');
            readOneChar();
            if(currenSense()=='}'){
                readOneChar();
                return new Dictionary<string, object>();
            }
            string firstKey = readString();
            //:を読み飛ばす
            search(':');
            readOneChar();
            object firstValue = readValue();
            //Dictionary<string,T>生成
            Type firstValueType = firstValue.GetType();
            Type dicType = Type.GetType("System.Collections.Generic.Dictionary`2[System.String,"+firstValueType.ToString()+"]");
            IDictionary valueDic = (IDictionary)Activator.CreateInstance(dicType);
            valueDic[firstKey] = firstValue;
            //Dictionary<string,object>生成
            Dictionary<string, object> objectDic = new Dictionary<string, object>();
            objectDic[firstKey] = firstValue;

            bool inconsistencyFlag = false;
            while (true){
                if (currenSense() == ','){
                    readOneChar();//,を読み飛ばす
                }else if (currenSense() == '}'){
                    readOneChar();//}を読み飛ばす
                    return (inconsistencyFlag) ? objectDic : valueDic;
                }else{
                    throw new Exception("不正なjson文字列 : not found value separator, instedad 「"+currenSense()+"」 was readed");
                }
                string key = readString();
                //:を読み飛ばす
                search(':');
                readOneChar();
                object value = readValue();
                if(inconsistencyFlag){
                    //valueの型が不統一
                    objectDic[key] = value;
                }else{
                    //valueの型が統一
                    if(value.GetType()==firstValueType){
                        valueDic[key] = value;
                        objectDic[key] = value;
                    }else{
                        inconsistencyFlag = true;
                        objectDic[key] = value;
                    }
                }
            }
        }
        ///valueを取得
        private object readValue(){
            switch (currenSense()){
                case '"'://文字列
                    return readString();
                case '{'://Dictionary
                    return readDictionary();
                case '['://配列
                    return readList();
                case 't'://bool
                case 'f':
                    return readBool();
                case '0'://数字
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                case '+':
                case '-':
                    return readNumber();
                default://不正
                    throw new Exception("不正なjson文字列 : invalid value start from 「"+currenSense()+"」");
            }
        }
        public Dictionary<string,object> parse(string jsonString){
            reader = new StringReader(jsonString);
            return (Dictionary<string, object>)readDictionaryOfObject();
        }
    }
}
