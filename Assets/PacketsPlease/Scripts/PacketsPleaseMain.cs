using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacketsPleaseMain : Singleton<PacketsPleaseMain> {

    public CustomerListUI m_customerListUI;
    public ActionPanelUI m_actionPanelUI;
    public NotificationListUI m_notificationUI;

    public float m_timeBetweenCustomers = 1.0f;
    public float m_customerTimer = 0.0f;
    public int m_maxNumCustomer = 10;

    protected bool m_isHandlingCustomer = false;
    protected int m_currentStrike = 0;

    protected void Update()
    {
        m_customerTimer += Time.deltaTime;
        if(m_customerTimer >= m_timeBetweenCustomers)
        {
            m_customerTimer -= m_timeBetweenCustomers;
            if (m_customerListUI.GetNumCustomers() < m_maxNumCustomer)
            {
                CustomerData data = ScriptableObject.CreateInstance<CustomerData>();
                data.Generate();
                m_customerListUI.AddCustomer(data);
            }
            else
            {
                // give strike for max customers reached
                GiveStrike();
            }
        }
        
        if (Input.GetKeyDown(KeyCode.Return))
        {
            m_customerListUI.RemoveCustomerTopCustomer();
        }
    }

    protected void GiveStrike()
    {
        m_currentStrike++;
        m_notificationUI.AddStrikeNotification(m_currentStrike);
    }

    public void ThrottleCustomer()
    {
        if (m_isHandlingCustomer || m_customerListUI.GetNumCustomers() <= 0)
        {
            return;
        }
        StartCoroutine(HandleThrottlingTopCustomer());
    }

    protected IEnumerator HandleThrottlingTopCustomer()
    {
        m_isHandlingCustomer = true;
        if(m_customerListUI.GetTopCustomer().m_data.m_dataUsage < 50.0f)
        {
            GiveStrike();
        }
        yield return new WaitForSeconds(0.33f);
        m_customerListUI.RemoveCustomerTopCustomer();
        m_isHandlingCustomer = false;
    }

    public void BoostCustomer()
    {
        if(m_isHandlingCustomer || m_customerListUI.GetNumCustomers() <= 0)
        {
            return;
        }
        StartCoroutine(HandleBoostingTopCustomer());
    }

    protected IEnumerator HandleBoostingTopCustomer()
    {
        m_isHandlingCustomer = true;
        if (m_customerListUI.GetTopCustomer().m_data.m_dataUsage >= 50.0f)
        {
            GiveStrike();
        }
        yield return new WaitForSeconds(0.33f);
        m_customerListUI.RemoveCustomerTopCustomer();
        m_isHandlingCustomer = false;
    }
}
