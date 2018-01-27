using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CustomerUI : MonoBehaviour {
    public TextMeshProUGUI m_name;
    public TextMeshProUGUI m_dataUsage;

    public CustomerData m_data { get; protected set; }
    
    public void Init(CustomerData data)
    {
        m_data = data;
        Init(data.m_name, data.m_dataUsage);
    }

    public void Init(string name = "", float dataUsage = 0.0f)
    {
        m_name.text = name;
        m_dataUsage.text = string.Format("Usage: {0:N2} GB", dataUsage);
    }

    public void DestroySelf()
    {
        StartCoroutine(HandleDestroySelf());
        // TODO: make this cooler
    }

    protected IEnumerator HandleDestroySelf()
    {
        LeanTween.moveLocalX(gameObject, -Screen.width*0.5f, 1.0f).setEaseInOutBack();
        yield return new WaitForSeconds(1.0f);
        Destroy(gameObject);
    }
}
