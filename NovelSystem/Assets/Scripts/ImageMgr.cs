using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class ImageMgr : SingletonMonoBefaviour<ImageMgr> {
    [SerializeField]
    //生成するImageオブジェクトのプレハブ
    GameObject mImagePrefab;

    //画像オブジェクトクラスのリスト
    List<ImageObj> mImageObjs = new List<ImageObj>();

    //今表示している画像がリストどものどの位置かを保持するIdx
    int mIdx = 0;

    //画像ファイルのパスを見つけるたびにテクスチャを生成してリストにぶっこむ
    public void CreateImageTexture(string filepath, Vector3 pos, string instlname, string lname, Vector2 size)
    {
        //int num = mImageObjs.Count;
        Texture2D tex = new Texture2D(0, 0);
        tex.LoadImage(Read.Instance.LoadBin(filepath));
        mImageObjs.Add(new ImageObj(tex, pos, instlname, lname, size));
    }

    //表示するタイミングのインデックスに到達したら処理を行う
    public void CreateImageObject(string lname)
    {
        for (int i = mIdx; i < mImageObjs.Count; ++i)
        {
            if(mImageObjs[i].mInstLabel == lname)
            {
                
                mImageObjs[i].mImageObject = (GameObject)Instantiate(mImagePrefab, mImageObjs[i].mImagePos, Quaternion.identity);
                mImageObjs[i].mImageObject.transform.SetParent(UIMgr.Instance.UIObject.GetComponent<BackGroundObj>().mBackGround.transform, false);
                RawImage image = mImageObjs[i].mImageObject.GetComponent<RawImage>();
                image.rectTransform.sizeDelta = mImageObjs[i].mSize;
                image.texture = mImageObjs[i].mImageTex;
            }
        }
    }

    //もしラベル名に到達したらぶっ殺す
    public void DestroyImageObject(string lname)
    {
        //ラベル名を持ってないなら処理はしない
        if (lname == "")
            return;

        for (int i = mIdx; i < mImageObjs.Count; ++i)
        {
            if(mImageObjs[i].mLabelNames == lname)
            {
                Destroy(mImageObjs[i].mImageObject);
                //殺したら再利用するつもりはないのでmIdxをすすめる。
                //リストを残しておくのは一応また戻るボタンのことを考えて。
                //なおできるといってない模様
                mIdx++;
            }
        }
    }

    public void DeleteAll()
    {

        mImageObjs.Clear();
        mIdx = 0;
    }
}
