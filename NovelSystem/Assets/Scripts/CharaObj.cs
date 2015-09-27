using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CharaObj {
    //第一引数：ポーズ名　第二引数：テクスチャ
    public Dictionary<string, Texture2D> mCharaTexs = new Dictionary<string, Texture2D>() { };

    //表示するポーズとタイミングは紐付けるほうがいいのではと思って最初は辞書型にしたがぶっちゃけ
    //座標が持てないから全部キューに順番に突っ込んで使えってなった
    //public Dictionary<string, string> mChangeTiming = new Dictionary<string,string>(){};

    //表示するタイミングのラベル
    public Queue<string> mLabelQueue = new Queue<string>();

    //表示するポーズ
    public Queue<string> mPoseNameQueue = new Queue<string>();

    //表示する座標
    public Queue<Vector3> mPosQueue = new Queue<Vector3>();

    //表示するサイズ
    public Queue<Vector2> mSizeQueue = new Queue<Vector2>();

    //生成されてる場合この変数にゲームオブジェクトを入れておく
    public GameObject mChara;

    public string mCharaName = "";

    public void Create(string key, Texture2D tex)
    {
        mCharaTexs.Add(key, tex);
    }

    public void SetChangeTiming(string label, string pose, Vector3 pos, Vector2 size)
    {
        mLabelQueue.Enqueue(label);
        mPoseNameQueue.Enqueue(pose);
        mPosQueue.Enqueue(pos);
        mSizeQueue.Enqueue(size);
    }
}
