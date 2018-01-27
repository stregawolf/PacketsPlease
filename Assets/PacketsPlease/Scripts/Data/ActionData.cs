using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActionType
{
    Throttle,
    Boost,
    Disconnect
}

public class ActionData : ScriptableObject {

    public CustomerData customer;

    public ActionType actionType;

    public ActionData(CustomerData customer, ActionType actionType)
    {
        this.customer = customer;
        this.actionType = actionType;
    }

}
