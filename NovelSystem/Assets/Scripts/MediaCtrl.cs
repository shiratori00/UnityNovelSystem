using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MediaCtrl {

    //ラベル名を格納する
    public string mLabel{ get; set; }

    //分岐ボタン用の名前リスト
    public List<string> mBranchNameList = new List<string>();

    //分岐ボタンが向かうラベル名リスト
    public List<string> mToLabelNameList = new List<string>();

    //分岐後の共通ルートへ飛ぶ用のラベル
    public List<string> ToCommonLabelName = new List<string>();

    //分岐ボタン用の座標リスト
    public List<Vector3> mBranchPosList = new List<Vector3>();

    //紐付ける画像ファイルを入れる
    //Texture2D mImage = new Texture2D(0, 0);

    //画像ファイルの貼るImageオブジェクトの大きさ
    //float mImageWidth;
    //float mImageHeight;

    //画像ファイルを表示する座標
    //public Vector3 mImagePos = Vector3.zero;

    //紐付ける画像ファイルのパス。読み込みはこっちで行う
    //public string mImageFilePath { get; set; }

    //表示するテキスト
    public string mShowText{ get; set; }

    //背景フラグ。0以上ならばその値に応じたリストの背景を表示する
    public int mBgFlg { get; set; }

    //分岐フラグ。1以上ならばその値に応じた数のボタンを生成する
    public int mBranchFlg { get; set; }

    public MediaCtrl(string text, string lname, int idx, int bnum, List<string> bname, List<Vector3> bpos, List<string> tolabelname, List<string> common)
    {
        mShowText = text;
        //mImageFilePath = ipath;
        mBgFlg = idx;
        mBranchFlg = bnum;
        mBranchNameList = new List<string>(bname);
        mBranchPosList = new List<Vector3>(bpos);
        mLabel = lname;
        mToLabelNameList = new List<string>(tolabelname);
        ToCommonLabelName = new List<string>(common);
    }

    //引数は戻るボタンで表示することになる場合trueとなる
    public string ShowMedias()
    {
        return mShowText;
    }
}
