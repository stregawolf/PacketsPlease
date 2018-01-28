using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CustomerData", menuName = "Data/CustomerData", order = 1)]
public class CustomerData : ScriptableObject {
    public NameGen.Name m_name;
    public float m_dataUsage;
    public int m_daysActive;
    public ActivityData.Activity m_activity;
    public bool m_male = false; // Deprecated

    public void Generate()
    {
        m_male = Random.value > 0.5f;
        m_name = NameGen.GetName(m_male);
        m_dataUsage = Random.Range(0.0f, 100.0f);
        m_daysActive = Random.Range(0, 10000);
        m_activity = ActivityData.GetActivity();
    }
}
