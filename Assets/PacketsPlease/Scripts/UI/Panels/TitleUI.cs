using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleUI : MonoBehaviour {
    public void Hide()
    {
        LeanTween.moveLocalY(gameObject, Screen.height * 2, 0.5f).setEaseInBack();
    }
}
