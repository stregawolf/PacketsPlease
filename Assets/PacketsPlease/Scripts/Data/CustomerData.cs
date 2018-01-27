using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CustomerData", menuName = "Data/CustomerData", order = 1)]
public class CustomerData : ScriptableObject {
    public string m_name;
    public float m_dataUsage;
    public ActivityData.Activity m_activity;

    public void Generate()
    {
        m_name = "McPoopyPants";
        m_dataUsage = Random.Range(0.0f, 100.0f);
        m_activity = ActivityData.Activies[Random.Range(0, ActivityData.Activies.Length)];
    }
}
