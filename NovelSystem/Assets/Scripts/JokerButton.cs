using UnityEngine;
using System.Collections;

public class JokerButton : MonoBehaviour {

    public string mScenename;

    public void OnClick()
    {
        Application.LoadLevel(mScenename);
    }
}
