﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerListUI : MonoBehaviour {
    public GameObject m_customerUIPrefab;
    public float m_separationDist = 10.0f;
    public float m_slideSpeed = 3.0f;

    public int m_totalCustomers { get; protected set; }
    protected Queue<CustomerUI> m_customerUIs = new Queue<CustomerUI>();
    
    protected void Update()
    {
        int i = 0;
        foreach(var ui in m_customerUIs)
        {
            // update positioning
            ui.transform.localPosition = Vector3.Lerp(ui.transform.localPosition, new Vector3(0.0f, i * m_separationDist, 0.0f), Time.deltaTime * m_slideSpeed);
            i++;
        }
    }

    public void ResetList()
    {
        m_totalCustomers = 0;
        EmptyList();
    }

    public void EmptyList()
    {
        foreach (var ui in m_customerUIs)
        {
            ui.DestroySelf();
        }

        m_customerUIs.Clear();
    }

    public int GetNumCustomers()
    {
        return m_customerUIs.Count;
    }

    public void AddCustomer(CustomerData data)
    {
        GameObject customerUIObj = Instantiate(m_customerUIPrefab, transform);
        customerUIObj.transform.SetAsFirstSibling();
        CustomerUI customerUI = customerUIObj.GetComponent<CustomerUI>();
        customerUI.Init(data);
        m_customerUIs.Enqueue(customerUI);
        customerUI.transform.localPosition = Vector3.up * Screen.height *2;
        m_totalCustomers++;
    }

    public CustomerUI GetTopCustomer()
    {
        if (m_customerUIs.Count == 0)
            return null;
        return m_customerUIs.Peek();
    }

    public void RemoveCustomerTopCustomer()
    {
        if(m_customerUIs.Count > 0)
        {
            CustomerUI top = m_customerUIs.Dequeue();
            top.DestroySelf();
        }
    }
}
