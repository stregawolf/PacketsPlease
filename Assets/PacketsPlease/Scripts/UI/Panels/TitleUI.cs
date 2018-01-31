using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleUI : MonoBehaviour {
    public Image m_bg;
    public GameObject m_banner;
    public GameObject m_startButton;
    public BaseFadingUI m_fader;

    public void Hide()
    {
        LeanTween.moveLocalY(m_startButton, -Screen.height, 0.33f).setEaseInBack().setOnStart(AudioManager.Instance.PlayNotificationClosed);
        LeanTween.moveLocalY(m_banner, Screen.height, 0.33f).setDelay(0.5f).setEaseInBack().setOnStart(AudioManager.Instance.PlayNotificationClosed);
        m_fader.FadeIn(false, 1.0f, ()=> 
        {
            m_bg.enabled = false;
            PacketsPleaseMain.Instance.StartGame();
            m_fader.FadeOut(false, 0.5f, ()=>
            {
                gameObject.SetActive(false);
            });
        });
    }
}
