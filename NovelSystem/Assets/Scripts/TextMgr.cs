using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Text;
using System;
using System.Linq;

public class TextMgr : SingletonMonoBefaviour<TextMgr> {
    //txtファイル名
    [SerializeField]
    public string mPath;

    //使わなくなった
    //TextAsset mTextFile;

    //ひとかたまりごとに突っ込む
    Stack<MediaCtrl> mTextList = new Stack<MediaCtrl>();

    //UIのテキストフィールド
    [SerializeField]
    Text mText;

    //キャラネーム
    public Text mName;

    //改行コード
    char[] kugiri = { '\r', '\n' };

    string mTxts = "";

    //いま表示しているmTextListの添字
    int mListIdx = 0;

    //読み込み完了してるかのフラグ
    bool mCompleteFlg = false;

    //分岐ボタンが出現してるかどうかのフラグ
    bool mBranch = false;

    //分岐ボタン後の共通ルートのラベル名
    string mCommonLabelName = "";

    //生成したボタンを保持するリスト
    List<GameObject> mButton = new List<GameObject>();

    //ログ
    Stack<MediaCtrl> mLogs = new Stack<MediaCtrl>();

    //一時バッファ
    //Stack<MediaCtrl> mBufs = new Stack<MediaCtrl>();

    public bool mMainFlg = false;
	
	// Update is called once per frame
	void Update () {
        if (!mMainFlg)
            return;

        if (!mCompleteFlg)
            return;
        ShowText();
        BackGroundMgr.Instance.ShowBG();
	}

    public void setFilePath(string n)
    {
        mCompleteFlg = false;
        mPath = n;
        //Debug.Log(mPath + "Get Name");
        mTxts = Read.Instance.ReadFile(mPath);
        AnalyzeText();
    }

    void ShowText()
    {
        if (mTextList.Count == 0)
            return;

        //現在のラベルで表示するキャラクタがいたら表示する
        if (mTextList.Peek().mLabel != "" || CharaMgr.Instance.mCharaObjs.Count != 0)
        {
            foreach (string name in CharaMgr.Instance.mCharaObjs.Keys)
            {
                if (CharaMgr.Instance.mCharaObjs[name].mLabelQueue.Count == 0)
                    continue;
                if (CharaMgr.Instance.mCharaObjs[name].mLabelQueue.Peek() != mTextList.Peek().mLabel)
                    continue;
                CharaMgr.Instance.CreateCharaObject(name, mTextList.Peek().mLabel);
            }
        }

        mText.text = mTextList.Peek().ShowMedias();

        if (mTextList.Peek().mBranchFlg > 0)
        {
            BranchButtonCreate();
        }
    }

    void BranchButtonCreate()
    {
        MediaCtrl m = mTextList.Peek();
        GameObject g;
        string name;
        for(int i = 0; i < m.mBranchFlg; ++i){
            g = UIMgr.Instance.UIObject.GetComponent<CreateButton>().Create(m.mBranchNameList[i], m.mToLabelNameList[i], m.mBranchPosList[i], UIMgr.ButtonState.BRANCH, m.ToCommonLabelName[i]);
            name = g.GetComponent<MyButton>().mToCommonLabelName;
            if(mCommonLabelName != "" && mCommonLabelName != name)
            {
                mCommonLabelName = name;
            }else if(mCommonLabelName == "")
            {
                mCommonLabelName = name;
            }
            mButton.Add(g);
        }
        m.mBranchFlg = 0;
        mBranch = true;
    }

    public void Branch(string tolabelname)
    {
        int cnt = 0;
        foreach(MediaCtrl m in mTextList)
        {
            if(m.mLabel == tolabelname)
            {
                mBranch = false;
                for (int i = 0; i < cnt; ++i)
                {
                    mTextList.Pop();
                }
                for (int j = 0; j < mButton.Count; ++j)
                {
                    Destroy(mButton[j]);
                }
                mButton.Clear();
                mListIdx += cnt;
                break;
            }
            cnt++;
        }
        /*for (int i = 0; i < mTextList.Count; ++i)
        {
            if (mTextList[i].mLabel == tolabelname)
            {
                mListIdx = i;
                mBranch = false;
                for (int j = 0; j < mButton.Count; ++j)
                {
                    Destroy(mButton[j]);
                }
                mButton.Clear();
                break;
            }
        }*/
    }

    public void ControllListIdx(int n)
    {
        //分岐ボタンが出現してるときは処理を飛ばす
        if (mBranch)
            return;
        int result = mListIdx;
        //進むボタンが押された時リストの最大値を超えてるなら処理しない
        if (n > 0)
        {
            //全部吐き出してたら残るのは１なので
            if(mTextList.Count == 1)
                 return;
            //mCommonLabelNameに値が入ってたら
            //一つポップする
            mLogs.Push(mTextList.Pop());
        }
        //また戻るボタンが押された時0未満ならばこちらも処理をしない
        if (n < 0)
        {
            //ログがゼロならまだ一つも進んでないので戻れない
            if(mLogs.Count == 0)
            {
                return;
            }
            mTextList.Push(mLogs.Pop());
            result += n;
        }

        //mCommonLabelNameが空じゃなかったら分岐後なのでそこに飛ぶ
        if (n > 0 && mCommonLabelName != "")
        {
            int cnt = 0;
            foreach (MediaCtrl m in mTextList)
            {
                if(m.mLabel == mCommonLabelName)
                {
                    for (int i = 0; i < cnt; i++)
                    {
                        mTextList.Pop();
                    }
                    result += cnt;
                    mCommonLabelName = "";
                    break;
                }
                cnt++;
            }
            /*for (int i = mListIdx; i < mTextList.Count; ++i)
            {
                if (mCommonLabelName == mTextList[i].mLabel)
                {
                    result = i;
                    mCommonLabelName = "";
                    break;
                }
            }*/
        }

        mListIdx = result;

        //イメージの表示と削除をラベルで判断する
        ImageMgr.Instance.CreateImageObject(mTextList.Peek().mLabel);
        ImageMgr.Instance.DestroyImageObject(mTextList.Peek().mLabel);

        //進むボタンのときは進んだ先のMediaCtrlのBGフラグが立っているのなら処理続行
        if (mTextList.Peek().mBgFlg < 0 && n > 0 )
            return;

        //ｎがマイナスならば戻るボタン
        if (n < 0)
        {
           // BackGroundMgr.Instance.mIdx = mTextList[mListIdx].mBgFlg - 1;
        }
            //プラスならば進むボタン
        else if(n > 0)
        {
            BackGroundMgr.Instance.mIdx = mTextList.Peek().mBgFlg;
        }
    }

    void AnalyzeText()
    {
        string[] sTxts = mTxts.Split(kugiri);

        string text = "";
        //string ifilepath = "";
        int bgidx = 0;
        int branchNum = 0;
        List<string> branchName = new List<string>();
        List<Vector3> branchPos = new List<Vector3>();
        List<string> toLabelName = new List<string>();
        string labelName = "";
        List<string> common = new List<string>();

        for (int i = 0; i < sTxts.Length; ++i )
        {
            //最初の文字が#であればリストに突っ込んで一時変数を初期化して次の塊へ
            if (sTxts[i].StartsWith("#"))
            {
                if (text != "")
                    mTextList.Push(new MediaCtrl(text, labelName, bgidx, branchNum, branchName, branchPos, toLabelName, common));
                text = "";
                common.Clear();
                branchName.Clear();
                branchPos.Clear();
                toLabelName.Clear();
                labelName = "";
                bgidx = -1;
                branchNum = 0;
                continue;
            }
            //[]があれば再びSplit
            if(sTxts[i].StartsWith("["))
            {
                //空白ごとに分割する
                string[] s = sTxts[i].Split(" "[0]);
                
                //[の次の単語によって変わる。
                //背景。
                if(s[1] == "bg")
                {
                    bgidx = BackGroundMgr.Instance.mCnt;
                    BackGroundMgr.Instance.SetBG(s[2]);
                }
                //ラベル名
                if (s[1] == "label")
                {
                    labelName = s[2];
                }
                //分岐ボタン
                if (s[1] == "branch")
                {
                    branchName.Add(s[2]);
                    toLabelName.Add(s[3]);
                    Vector3 vec3 = Vector3.zero;
                    if (s.Length > 4)
                    {
                        int x = int.Parse(s[4]);
                        int y = int.Parse(s[5]);
                        int z = 0;
                        vec3 = new Vector3(x, y, z);
                    }
                    branchPos.Add(vec3);
                    common.Add(s[6]);
                    branchNum++;
                }
                //画像
                if(s[1] == "image")
                {
                    string filepath = s[2];
                    Vector3 pos = Vector3.zero;
                    int x = int.Parse(s[3]);
                    int y = int.Parse(s[4]);
                    pos = new Vector3(x, y, 0.0f);
                    string InstLabelName = s[5];
                    string DeleteLabelname = s[6];
                    x = int.Parse(s[7]);
                    y = int.Parse(s[8]);
                    Vector2 size = new Vector2(x, y);
                    ImageMgr.Instance.CreateImageTexture(filepath, pos, InstLabelName, DeleteLabelname, size);
                    filepath = "";
                }
                //キャラ関連
                if(s[1] == "chara")
                {
                    //画像をセットする
                    if (s[2] == "set")
                    {
                        string name = s[3];
                        string pose = s[4];
                        string filepath = s[5];
                        CharaMgr.Instance.CreateCharaTex(name, pose, filepath);
                    }
                    //画像を表示・変更する
                    else if(s[2] == "change")
                    {
                        string name = s[3];
                        string label = s[4];
                        string pose = s[5];
                        Vector3 pos = Vector3.zero;
                        Vector2 size = Vector2.zero;
                        //s[6]以上に何か入ってたら座標
                        if (s[6] != "]")
                        {
                            int x = int.Parse(s[6]);
                            int y = int.Parse(s[7]);
                            pos = new Vector3(x, y, 0.0f);
                        }
                        if (s.Length > 9)
                        {
                            if (s[8] != "]")
                            {
                                int w = int.Parse(s[8]);
                                int h = int.Parse(s[9]);
                                size = new Vector2(w, h);
                            }
                        }
                        //名前のキャラがいなかったら死ぬ(処理を飛ばす
                        CharaObj obj = CharaMgr.Instance.mCharaObjs[name];
                        if (obj == null)
                            continue;
                        CharaMgr.Instance.mCharaObjs[name].SetChangeTiming(label, pose, pos, size);
                    }
                }
            }
            else if (sTxts[i] != "")
            {
                text += sTxts[i]+"\n";
            }
            //s += SetDefaultText();
            //最終行ならば問答無用でぶちこむ
            if (i == sTxts.Length-1)
            {
                if (text != "")
                    mTextList.Push(new MediaCtrl(text, labelName, bgidx, branchNum, branchName, branchPos, toLabelName, common));
                text = "";
                bgidx = -1;
                branchNum = 0;
                common.Clear();
                branchName.Clear();
                branchPos.Clear();
                toLabelName.Clear();
                labelName = "";
            }
        }

        mCompleteFlg = true;
        MediaCtrl[] buf = mTextList.ToArray();
        mTextList.Clear();

        foreach(MediaCtrl m in buf)
        {
            mTextList.Push(m);
        }
    }

    internal void setStringName(string mName)
    {
        throw new NotImplementedException();
    }

    public void SetTextField(GameObject g)
    {
        mText = g.GetComponent<Text>();
    }

    public void SetNameField(GameObject g)
    {
        mName = g.GetComponent<Text>();
    }

    public void DeleteAll()
    {
        mTextList.Clear();
        mCommonLabelName = "";
        mCompleteFlg = false;
        mBranch = false;
        mButton.Clear();
        mLogs.Clear();
    }
}
