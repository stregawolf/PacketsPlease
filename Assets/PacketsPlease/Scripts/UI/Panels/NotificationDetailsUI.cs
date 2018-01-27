using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NotificationDetailsUI : NotificationUI {

    public Text responseA;
    public Text responseB;

    public GameObject responseParent;

    public TextMeshProUGUI m_message;
    public TextMeshProUGUI m_sender;

    public override void Init(NotificationData data)
    {
        base.Init(data);
        m_message.text = m_data.m_message;
        m_sender.text = m_data.m_sender;

        if(data.m_response != null)
        {
            responseParent.gameObject.SetActive(true);
            responseA.text = m_data.m_response.m_ChoiceA;
            responseB.text = m_data.m_response.m_ChoiceB;
        }
        else
        {
            responseParent.gameObject.SetActive(false);
        }

    }
}
