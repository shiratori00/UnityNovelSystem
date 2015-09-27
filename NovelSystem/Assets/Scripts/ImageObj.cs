using UnityEngine;
using System.Collections;

public class ImageObj {
    //表示するImageテクスチャ
    public Texture2D mImageTex = new Texture2D(0, 0);

    //画像オブジェクトの大きさ
    //public float mImageWidth;
    //public float mImageHeight;
 
    //画像ファイルを表示する座標
    public Vector3 mImagePos = Vector3.zero;

    //表示するタイミングのラベル名
    public string mInstLabel = "";

    //消えるタイミングのラベル名
    public string mLabelNames = "";

    //作ったらここにいれておく
    public GameObject mImageObject;

    //画像オブジェクトの大きさ
    public Vector2 mSize;

    public ImageObj(Texture2D tex, Vector3 pos, string instlname, string lname, Vector2 size)
    {
        mImageTex = tex;
        mImagePos = new Vector3(pos.x, pos.y, pos.z);
        mInstLabel = instlname;
        mLabelNames = lname;
        mSize = size;
    }
}
