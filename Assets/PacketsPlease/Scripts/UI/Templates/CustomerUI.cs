using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CustomerUI : MonoBehaviour {

    public Image m_bg;
    public TextMeshProUGUI m_name;
    public TextMeshProUGUI m_dataUsage;
    public TextMeshProUGUI m_dateStarted;

    public CustomerData m_data { get; protected set; }
    
    public virtual void Init(CustomerData data)
    {
        m_data = data;
        // Set Text Fields
        m_name.text = data.m_name.LastFirst;
        m_dataUsage.text = string.Format("Usage: {0:N2} GB", data.m_dataUsage);

        System.DateTime dateStarted = System.DateTime.Now.AddDays(-1 * data.m_daysActive);
        m_dateStarted.text = dateStarted.Month + "/" + dateStarted.Day + "/" + dateStarted.Year;
    }
    
    public void SetBGColor(Color c)
    {
        m_bg.color = c;
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
