using UnityEngine;
using System.Collections;

public class MusicMgr : SingletonMonoBefaviour<MusicMgr> {
    public AudioClip mBGM;
    public AudioSource mSouce;

    void Start()
    {
        mSouce.clip = mBGM;
    }

    public void SetSouce()
    {
        mSouce.clip = mBGM;
    }

    public void Change(bool b)
    {
        if (b)
        {
            mSouce.Play();
        }
        else
        {
            mSouce.Stop();
        }
    }
}
