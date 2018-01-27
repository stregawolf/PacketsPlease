using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionPanelUI : MonoBehaviour {

    public CustomerDetailsUI m_customerDetails;

	// Use this for initialization
	void Start () {
        m_customerDetails.gameObject.SetActive(false);
	}
	
    public void SetCustomer(CustomerUI customer)
    {
        if(customer == null)
        {
            m_customerDetails.gameObject.SetActive(false);
        }
        else
        {
            m_customerDetails.gameObject.SetActive(true);
            m_customerDetails.Init(customer.m_data);
        }
    }
}
