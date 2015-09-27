using UnityEngine;
using System.Collections;

public class NextBackButton : MonoBehaviour {

    enum ButtonType
    {
        BACK = -1,
        NEXT = 1,
    }

    [SerializeField]
    ButtonType num;

    public void OnClick()
    {
        TextMgr.Instance.ControllListIdx((int)num);
    }
}
