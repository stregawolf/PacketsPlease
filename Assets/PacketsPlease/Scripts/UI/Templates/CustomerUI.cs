using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomerUI : MonoBehaviour {
    public Text m_name;
    
    public void Init(CustomerData data)
    {

    }

    public void Init(string name)
    {
        m_name.text = name;
    }
}
