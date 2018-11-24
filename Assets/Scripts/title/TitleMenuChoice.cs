using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleMenuChoice : MonoBehaviour {
    [SerializeField]
    private string mKey="";
    private bool mSelected=false;
    //透明度変更用
    private MeshRenderer mMeshRenderer;
    //透明度変更タイミング決定用
    private int mTimer=0;

	// Use this for initialization
	void Start () {
        mMeshRenderer = GetComponent<MeshRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        if (!mSelected)
            return;
        
        mTimer++;
        if (mTimer < 3)
            return;
        mTimer = 0;
        Color tColor = mMeshRenderer.material.color;
        mMeshRenderer.material.color = (tColor.a == 0f) ? new Color(tColor.r, tColor.g, tColor.b, 1) : new Color(tColor.r, tColor.g, tColor.b, 0);
	}
    private void OnMouseDown(){
        Subject.sendMessage(new Message("selectTitleMenu",new Arg(new Dictionary<string, object>() { { "key", mKey } }),"title"));
    }
    ///選択された
    public void selected(){
        mSelected = true;
    }
}
