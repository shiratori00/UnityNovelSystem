using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class CharaMgr : SingletonMonoBefaviour<CharaMgr> {

    //第一引数：キャラの名前　第二引数：キャラのテクスチャやらもろもろ
    public Dictionary<string, CharaObj> mCharaObjs = new Dictionary<string, CharaObj>() { };

    public GameObject mImagePrefab;

    public Vector3[] mPos;

    public int mCharaNum = 0;

    public void CreateCharaTex(string name, string pose, string texpath)
    {
        Texture2D tex = new Texture2D(0, 0);
        tex.LoadImage(Read.Instance.LoadBin(texpath));

        //キャラが既に１体以上存在していル場合
        if (mCharaObjs.Count > 0)
        {
            bool isExist = false;
            //すでに存在しているキャラクターならば新しく作らない
            foreach (string k in mCharaObjs.Keys)
            {
                if (k == name)
                {
                    isExist = true;
                    break;
                }
            }

            if (isExist)
            {
                mCharaObjs[name].Create(pose, tex);
                return;
            }
        }

        //キャラが存在していないならば新しく作る
        CharaObj obj = new CharaObj();
        mCharaObjs.Add(name, obj);
        mCharaObjs[name].Create(pose, tex);
        mCharaObjs[name].mCharaName = name;
    }

    public void CreateCharaObject(string charaname, string label)
    {
        int num = mCharaObjs[charaname].mSizeQueue.Count;
        //キューのカウントがゼロなら処理はしない（サイズなのはなんかこいつが悪いから
        if (num == 0)
            return;
        //表示するポーズの名前
        string pose = mCharaObjs[charaname].mPoseNameQueue.Peek();

        //ポーズが空なら他のも空なので処理はしない
        if (pose == "" || pose == null)
            return;
        GameObject c = mCharaObjs[charaname].mChara;

        //deleteならばこのキャラを消す
        if (pose == "delete")
        {
            //Destroy(c);
            c.SetActive(false);
            //mCharaObjs.Remove(charaname);
            mCharaNum--;
            mCharaObjs[charaname].mLabelQueue.Dequeue();
            mCharaObjs[charaname].mPoseNameQueue.Dequeue();
            mCharaObjs[charaname].mPosQueue.Dequeue();
            mCharaObjs[charaname].mSizeQueue.Dequeue();
            foreach(string key in mCharaObjs.Keys)
            {
                if(mCharaObjs[key].mChara.activeSelf)
                {
                    TextMgr.Instance.mName.text = key;
                    break;
                }
            }
            return;
        }
        //GameObject c = UIMgr.Instance.UIObject.GetComponent<CharaParent>().mCharaParent.transform.FindChild(charaname).gameObject;

        //mChara変数にゲームオブジェクトが入ってたらそいつのテクスチャを変更する
        if(c != null)
        {
            //消えてたら再表示する
            if (!c.activeSelf)
            {
                c.SetActive(true);
            }

            RawImage chara = c.GetComponent<RawImage>();
            //テクスチャ・座標の設定をして、使った座標たちはキューから消す
            chara.texture = mCharaObjs[charaname].mCharaTexs[pose];
            if (mCharaObjs[charaname].mSizeQueue.Peek() == Vector2.zero)
            {
                chara.SetNativeSize();
                mCharaObjs[charaname].mSizeQueue.Dequeue();
            }
            else
            {
                chara.rectTransform.sizeDelta = mCharaObjs[charaname].mSizeQueue.Dequeue();
            }
            if(mCharaObjs[charaname].mPosQueue.Count != 0)
                chara.transform.position = mCharaObjs[charaname].mPosQueue.Dequeue();
            mCharaObjs[charaname].mLabelQueue.Dequeue();
            mCharaObjs[charaname].mPoseNameQueue.Dequeue();
            TextMgr.Instance.mName.text = mCharaObjs[charaname].mCharaName;
            return;
        }

        //存在してないなら新しくRawImageObjを作成する
        GameObject g = (GameObject)Instantiate(mImagePrefab, mCharaObjs[charaname].mPosQueue.Dequeue(), Quaternion.identity);
        g.GetComponent<RawImage>().texture = mCharaObjs[charaname].mCharaTexs[pose];
        if (mCharaObjs[charaname].mSizeQueue.Peek() == Vector2.zero)
        {
            g.GetComponent<RawImage>().SetNativeSize();
            mCharaObjs[charaname].mSizeQueue.Dequeue();
        }
        else
        {
            g.GetComponent<RawImage>().rectTransform.sizeDelta = mCharaObjs[charaname].mSizeQueue.Dequeue();
        }

        if (mCharaObjs[charaname].mPosQueue.Count != 0)
        {
            g.transform.position = mCharaObjs[charaname].mPosQueue.Dequeue();
        }
        else 
        {
            g.transform.position = mPos[mCharaNum];
        }
        g.transform.SetParent(UIMgr.Instance.UIObject.GetComponent<CharaParent>().mCharaParent.transform, false);
        g.name = charaname;
        mCharaObjs[charaname].mLabelQueue.Dequeue();
        mCharaObjs[charaname].mPoseNameQueue.Dequeue();
        //作ったキャラを格納する
        mCharaObjs[charaname].mChara = g;
        TextMgr.Instance.mName.text = mCharaObjs[charaname].mCharaName;
        mCharaNum++;
    }

    //タイトル戻る時とか
    public void DeleteAll()
    {
        foreach(string key in mCharaObjs.Keys)
        {
            Destroy(mCharaObjs[key].mChara);
        }
        mCharaObjs.Clear();
    }
}
