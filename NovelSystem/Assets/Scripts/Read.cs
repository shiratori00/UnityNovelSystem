using UnityEngine;
using System;               
using System.Collections;   
using System.IO;
using System.Text;

public class Read : SingletonMonoBefaviour<Read>
{

    //テキストファイル読み込み
    public string ReadFile(string path)
    {
        string mTxts = "";
        FileInfo fi = new FileInfo(Application.dataPath + "/TextFiles/" + path);

        try
        {
            //一行読み込み
            using (StreamReader sr = new StreamReader(fi.OpenRead(), Encoding.UTF8))
            {
                mTxts = sr.ReadToEnd();
            }
        }
        catch (Exception e)
        {
            mTxts += SetDefaultText();
        }

        return mTxts;
    }

    //バイナリで画像ファイルを読み込む（バイナリだし別に画像ファイルに限定する必要はない？
    public byte[] LoadBin(string path)
    {
        FileStream fs = new FileStream(Application.dataPath + "/ImageFiles/" + path, FileMode.Open);
        BinaryReader br = new BinaryReader(fs);
        byte[] buf = br.ReadBytes((int)br.BaseStream.Length);
        br.Close();
        return buf;
    }

    //改行コード処理（意味不明なので先生に聞く
    string SetDefaultText()
    {
        return "C#あ\n";
    }
}