using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class BackGroundMgr : SingletonMonoBefaviour<BackGroundMgr>
{
    List<Texture2D> mBG = new List<Texture2D>();

    public Image mBackGround;

    public int mCnt { get; set; }

    public int mIdx { get; set; }

    protected override void Awake()
    {
        base.Awake();
        mCnt = 0;
        mIdx = 0;
    }

    public void ShowBG()
    {
        mBackGround.sprite = Sprite.Create(mBG[mIdx], new Rect(0, 0, 800, 600), Vector2.zero);
    }

    void Update()
    {
        //ShowBG();
    }

    public void SetBG(string path)
    {
        mBG.Add(new Texture2D(0, 0));
        mBG[mCnt].LoadImage(Read.Instance.LoadBin(path));
        mCnt++;
    }

    public void DeleteAll()
    {
        mCnt = 0;
        mIdx = 0;
        mBG.Clear();
        mBackGround = null;
    }
}