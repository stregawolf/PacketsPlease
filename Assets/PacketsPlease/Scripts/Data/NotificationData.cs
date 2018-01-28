using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NotificationData", menuName = "Data/NotificationData", order = 1)]
public class NotificationData : ScriptableObject
{
    public string m_title;
    public string m_message;
    public string m_sender;
    public bool m_pinned;         // Pinned notifications go back to the top of the stack when closed

    public class Response
    {
        public enum CorrectResponse
        {
            NONE=0,
            CHOICE_A,
            CHOICE_B
        };

        public string m_ChoiceA;
        public string m_ChoiceB;
        public CorrectResponse m_correctResponse;

        public Response(string choiceA, string choiceB, CorrectResponse correctResponse)
        {
            m_ChoiceA = choiceA; m_ChoiceB = choiceB; m_correctResponse = correctResponse;
        }
    }

    public Response m_response = null;

    public void Generate()
    {
        ///////////////////////////////////////
        // TEST - Delete this later
        ///////////////////////////////////////
        float coin = Random.value;
        if (coin < 0.2f)
        {
            m_title = "CHOOSE A";
            m_message = "Choose the right response!";
            m_response = new Response("A", "B", Response.CorrectResponse.CHOICE_A);
        } else if (coin < 0.4f)
        {
            m_title = "CHOOSE B";
            m_message = "Choose the right response!";
            m_response = new Response("A", "B", Response.CorrectResponse.CHOICE_B);
        } else if (coin < 0.7f)
        {
            m_title = "Choice Doesn't Matter";
            m_message = "Choose either of the below";
            m_response = new Response("A", "B", Response.CorrectResponse.NONE);
        } else
        {
            m_title = "No Choice";
            m_message = "This should just be showing the cancel button";
            m_response = null;
        }
        m_sender = "TestBot 3000";
        ////////////////////////////////////////////////////////////////////
    }

    public void GenerateStrike(int number)
    {
        m_title = string.Format("This is strike #{0}", number);
        m_message = string.Format("You've got #{0} strikes!", number);
        m_response = new Response("I'm sorry", "Eat my butt", Response.CorrectResponse.CHOICE_A);
        m_sender = "Mr. Bossman Your Boss";
    }
}
