using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

///マップ属性
public class MapBehaviourAttribute {
    private Attribute mAttribute;
    public Attribute attribute{
        get { return mAttribute; }
    }
    public string str{
        get{return Enum.GetName(typeof(Attribute), mAttribute);}
    }
    public enum AttributeType{
        trout,speaker,mapEvent
    }
    public enum Attribute{
        //地形
        none,
        air,
        flat,
        water,
        magma,
        wall,
        bridge,
        ladder,
        //もの
        ghost,
        accessory,
        character,
        flying,
        pygmy,
        //イベント
        empty,
        environment,
        force
    }
    public MapBehaviourAttribute(Attribute aAttribute){
        mAttribute = aAttribute;
    }
    public MapBehaviourAttribute(string aAttribute){
        mAttribute=(Attribute)Enum.Parse(typeof(Attribute), aAttribute, true);
    }
    //属性のタイプを返す
    public AttributeType getAttributeType(){
        if (mAttribute < Attribute.ghost) return AttributeType.trout;
        if (mAttribute < Attribute.empty) return AttributeType.speaker;
        return AttributeType.mapEvent;
    }
    static public AttributeType getAttributeType(Attribute aAttribute){
        if (aAttribute < Attribute.ghost) return AttributeType.trout;
        if (aAttribute < Attribute.empty) return AttributeType.speaker;
        return AttributeType.mapEvent;
    }
    //属性を重ねる
    public void pile(Attribute aAttribute){
        //重ねられるのは地形のみ
        if (getAttributeType() != AttributeType.trout) return;
        if (MapBehaviourAttribute.getAttributeType(aAttribute) != AttributeType.trout) return;
        if (mAttribute < aAttribute)
            mAttribute = aAttribute;
    }
    public void pile(string aAttribute){
        pile((Attribute)Enum.Parse(typeof(Attribute), aAttribute, true));
    }
    //引数の属性を通過できるか
    public bool canPass(MapBehaviourAttribute aAttribute){
        switch(mAttribute){
            //地形
            case Attribute.none:
            case Attribute.air:
            case Attribute.flat:
            case Attribute.water:
            case Attribute.magma:
            case Attribute.wall:
            case Attribute.bridge:
            case Attribute.ladder:
                return true;
            //もの
            case Attribute.ghost:
                return true;
            case Attribute.accessory:
                return true;
            case Attribute.character:
                switch(aAttribute.attribute){
                    //地形
                    case Attribute.none:return false;
                    case Attribute.air:return false;
                    case Attribute.flat:return true;
                    case Attribute.water:return false;
                    case Attribute.magma:return false;
                    case Attribute.wall:return false;
                    case Attribute.bridge:return true;
                    case Attribute.ladder:return true;
                    //もの
                    case Attribute.ghost:return true;
                    case Attribute.accessory:return false;
                    case Attribute.character:return false;
                    case Attribute.flying:return false;
                    case Attribute.pygmy:return true;
                    //イベント
                    case Attribute.empty:
                    case Attribute.environment:
                    case Attribute.force:
                        return true;
                    default:
                        throw new Exception("MapBehaviourAttribute : 「"+str+"」と「"+aAttribute.str+"」の衝突関係が定義されてないよん");
                }
            case Attribute.flying:
                switch (aAttribute.attribute){
                    //地形
                    case Attribute.none: return false;
                    case Attribute.air: return false;
                    case Attribute.flat: return true;
                    case Attribute.water: return true;
                    case Attribute.magma: return false;
                    case Attribute.wall: return false;
                    case Attribute.bridge: return true;
                    case Attribute.ladder: return false;
                    //もの
                    case Attribute.ghost: return true;
                    case Attribute.accessory: return false;
                    case Attribute.character: return false;
                    case Attribute.flying: return false;
                    case Attribute.pygmy: return true;
                    //イベント
                    case Attribute.empty:
                    case Attribute.environment:
                    case Attribute.force:
                        return true;
                    default:
                        throw new Exception("MapBehaviourAttribute : 「" + str + "」と「" + aAttribute.str + "」の衝突関係が定義されてないよん");
                }
            case Attribute.pygmy:
                switch (aAttribute.attribute){
                    //地形
                    case Attribute.none: return false;
                    case Attribute.air: return false;
                    case Attribute.flat: return true;
                    case Attribute.water: return true;
                    case Attribute.magma: return false;
                    case Attribute.wall: return false;
                    case Attribute.bridge: return true;
                    case Attribute.ladder: return true;
                    //もの
                    case Attribute.ghost: return true;
                    case Attribute.accessory: return false;
                    case Attribute.character: return true;
                    case Attribute.flying: return true;
                    case Attribute.pygmy: return true;
                    //イベント
                    case Attribute.empty:
                    case Attribute.environment:
                    case Attribute.force:
                        return true;
                    default:
                        throw new Exception("MapBehaviourAttribute : 「" + str + "」の衝突関係が定義されてないよん");
                }
            //イベント
            case Attribute.empty:
            case Attribute.environment:
            case Attribute.force:
                return true;
            default:
                throw new Exception("MapBehaviourAttribute : 「" + str + "」と「" + aAttribute.str + "」の衝突関係が定義されてないよん");
        }
    }
}
