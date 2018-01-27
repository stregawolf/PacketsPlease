using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CustomerUI : MonoBehaviour {
    public TextMeshProUGUI m_name;
    public TextMeshProUGUI m_dataUsage;
    public TextMeshProUGUI m_dateStarted;

    public CustomerData m_data { get; protected set; }
    
    public void Init(CustomerData data)
    {
        m_data = data;
        Init(data.m_name.LastFirst, data.m_dataUsage, data.m_daysActive);
    }

    public void Init(string name = "", float dataUsage = 0.0f, int daysActive = 0)
    {
        m_name.text = name;
        m_dataUsage.text = string.Format("Usage: {0:N2} GB", dataUsage);
        System.DateTime dateStarted = System.DateTime.Now.AddDays(-1 * daysActive);
        m_dateStarted.text = dateStarted.Month + "/" + dateStarted.Day + "/" + dateStarted.Year;
    }

    public void Copy(CustomerUI toCopy)
    {
        if(toCopy == null)
        {
            m_name.text = "";
            m_dataUsage.text = "";
            m_dateStarted.text = "";
        }
        else
            Init(toCopy.m_data);
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
