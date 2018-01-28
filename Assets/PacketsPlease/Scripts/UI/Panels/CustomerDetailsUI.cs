using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CustomerDetailsUI : CustomerUI {
    public TextMeshProUGUI m_customerStartDate;

    public override void Init(CustomerData data)
    {
        base.Init(data);
        m_dataUsage.text = string.Format("Data Usage:\n{0:N2} GB", data.m_dataUsage);

        System.DateTime dateStarted = System.DateTime.Now.AddDays(-1 * data.m_daysActive);
    
    }

}
