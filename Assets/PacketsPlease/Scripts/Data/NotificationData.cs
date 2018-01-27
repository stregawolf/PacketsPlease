using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NotificationData", menuName = "Data/NotificationData", order = 1)]
public class NotificationData : ScriptableObject
{
    public string m_title;
    public string m_message;
    public string m_ResponseA;
    public string m_ResponseB;

    public void Generate()
    {

    }

    public void GenerateStrike(int number)
    {
        m_title = string.Format("This is strike #{0}", number);
    }
}
