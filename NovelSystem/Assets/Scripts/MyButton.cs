using UnityEngine;
using System.Collections;

public class MyButton : MonoBehaviour {
    //ファイル名やジャンプ先のラベル名を所持する変数
    [SerializeField]
    private string mName;

    public string mToCommonLabelName;

    //切り替え先のステートを所有。画面切り替えをしないのならばNONE
    [SerializeField]
    UIMgr.State mState = UIMgr.State.NONE;

    //このボタンはファイル読み込みを実行するのか分岐ボタンなのか
    [SerializeField]
    UIMgr.ButtonState mButtonState;

    public void OnClick() {
        //Debug.Log(gameObject.name + " Click!");
        UIMgr.Instance.SetButtonState(mButtonState);
        UIMgr.Instance.ChangeState(mState);
        UIMgr.Instance.SetStringName(mName);
    }

    public void SetStringName(string n) {
        mName = n;
    }

    public void SetButtonState(UIMgr.ButtonState bs)
    {
        mButtonState = bs;
    }
}
