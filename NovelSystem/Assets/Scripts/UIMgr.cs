using UnityEngine;
using System.Collections;

public class UIMgr : SingletonMonoBefaviour<UIMgr> {

    string mName = "";

    //UIのプレハブを格納する変数
    [SerializeField]
    GameObject MainCanvas;

    [SerializeField]
    GameObject TitleCanvas;

    //現在表示してるUIのゲームオブジェクトを格納する変数
    public GameObject UIObject;

    public enum ButtonState
    {
        NONE = -1,
        FILE,       //ファイル読み込み
        BRANCH,     //分岐ボタン
    }

    public enum State
    {
        NONE = -1,
        TITLE,          //タイトル画面
        MAIN,           //コンテンツ画面
    }

    [SerializeField]
    ButtonState mButtonState;        //今表示されているボタンの状態

    [SerializeField]
    public State mState;                    //アプリの今の状態

    void Start()
    {
    }

    //ラベル名やファイルパスを受け取ってテキストマネージャに投げる
    public void SetStringName(string n)
    {
        mName = n;
        switch (mButtonState)
        {
            case ButtonState.BRANCH:
                TextMgr.Instance.Branch(mName);
                break;
            case ButtonState.FILE:
                TextMgr.Instance.setFilePath(mName);
                break;
            case ButtonState.NONE:
            default:
                //Debug.LogError("ARIENAI");
                break;
        }
    }

    //タイトルとゲーム画面を切り替える
    public void ChangeState(State s) {
        mState = s;
        switch (mState)
        {
            case State.TITLE:
                ChangeUI(TitleCanvas);
                TextMgr.Instance.DeleteAll();
                ImageMgr.Instance.DeleteAll();
                CharaMgr.Instance.DeleteAll();
                BackGroundMgr.Instance.DeleteAll();
                MusicMgr.Instance.Change(false);
                TextMgr.Instance.mMainFlg = false;
                break;
            case State.MAIN:
                ChangeUI(MainCanvas);
                GameObject g = UIObject.GetComponent<TextField>().mTextField;
                GameObject n = UIObject.GetComponent<charaname>().mNameText;
                BackGroundMgr.Instance.mBackGround = UIObject.GetComponent<BackGroundObj>().mBackGround;
                TextMgr.Instance.SetTextField(g);
                TextMgr.Instance.SetNameField(n);
                MusicMgr.Instance.Change(true);
                TextMgr.Instance.mMainFlg = true;
                break;
            case State.NONE:
                break;
            default:
                break;
        }
    }

    //渡されてるのはラベル名かファイル名かを設定
    public void SetButtonState(ButtonState bs)
    {
        mButtonState = bs;
    }

    //ゲーム画面に切り替わるときの処理
    void ChangeUI(GameObject Act)
    {
        Destroy(UIObject);
        UIObject = (GameObject)Instantiate(Act, Vector3.zero, Quaternion.identity);
    }
}
