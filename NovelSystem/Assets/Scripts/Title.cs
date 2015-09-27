using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Title : MonoBehaviour {

    [SerializeField]
    string[] mButtonString;

    [SerializeField]
    Text[] mButtonText;

    [SerializeField]
    Button[] mButton;

	// Use this for initialization
	void Start () {
        for (int i = 0; i < mButtonText.Length; ++i)
        {
            mButtonText[i].text = mButtonString[i];
        }
	}
}
