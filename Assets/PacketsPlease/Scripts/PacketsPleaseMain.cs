using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacketsPleaseMain : Singleton<PacketsPleaseMain> {

    public CustomerListUI m_customerListUI;
    public ActionPanelUI m_actionPanelUI;
    public NotificationUI m_notificationUI;

    public CustomerData m_testData;

    protected void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            m_testData.Generate();
            m_customerListUI.AddCustomer(m_testData);
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            m_customerListUI.RemoveCustomerTopCustomer();
        }
    }
}
