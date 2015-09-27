using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//ボタンを生成する
public class CreateButton : MonoBehaviour {
    //生成するボタンのプレハブ
    [SerializeField]
    GameObject mButtonPrefab;

    //生成
    //bname:ボタンのテキストに入れる文字列
    //name:MyButtonの参照すべきラベルの名前or参照するファイルのパス
    public GameObject Create(string bname, string name, Vector3 pos, UIMgr.ButtonState bstate, string common)
    {
        GameObject g = (GameObject)Instantiate(mButtonPrefab, pos, Quaternion.identity);
        //g.transform.parent = this.transform;
        g.transform.SetParent(this.transform, false);
        g.gameObject.GetComponentInChildren<Text>().text = bname;
        g.GetComponent<MyButton>().SetStringName(name);
        g.GetComponent<MyButton>().SetButtonState(bstate);
        g.GetComponent<MyButton>().mToCommonLabelName = common;
        return g;
    }
}
