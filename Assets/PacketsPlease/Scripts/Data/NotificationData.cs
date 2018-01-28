using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NotificationData", menuName = "Data/NotificationData", order = 1)]
public class NotificationData : ScriptableObject
{
    public const string SENDER_BOSS = "Mr. Bossman Your Boss";

    public string m_title;
    [HideInInspector]
    public string m_message;
    public string m_sender;
    public bool m_pinned;         // Pinned notifications go back to the top of the stack when closed
    public Color m_iconColor = Color.white;

    [HideInInspector]
    public StoryData m_parentStory;

    public enum ResolutionAction
    {
        None = 0,
        TransitionDay,
        GameOver,
        EndStory,
        PostNotificationA,
        PostNotificationB,
        PostNotificationC,
        PostCustomerA,
        PostCustomerB,
        PostCustomerC
    }

    public ResolutionAction m_correctResponseAction = ResolutionAction.None;
    public ResolutionAction m_incorrectResponseAction = ResolutionAction.None;

    public List<NotificationData> m_responseNotifications;
    public List<CustomerData> m_responseCustomers;

    [System.Serializable]
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
        public bool m_strikeOnIncorrect = false;
        public bool m_clearMe = true;
    }

    public Response m_response = null;
    
    public void GenerateStrike(int number)
    {
        m_title = string.Format("This is strike #{0}", number);
        m_sender = SENDER_BOSS;
        m_message = string.Format("You've got #{0} strikes!", number);
        m_response = new Response("I'm sorry", "Eat my butt", Response.CorrectResponse.CHOICE_A);
        m_iconColor = Color.red;
        m_correctResponseAction = ResolutionAction.None;
        m_incorrectResponseAction = ResolutionAction.None;
    }

    public void GenerateEndOfDay(int day, int numCorrectChoices, int totalNumCustomers)
    {
        m_title = string.Format("End of Day {0}", day);
        m_sender = SENDER_BOSS;
        float performance = (float)numCorrectChoices / (float)totalNumCustomers;
        m_message = string.Format("You have completed day {0}.\n\nPerformance report:\n{1:P0}\n\nCorrect choices:\n{2}\nTotal customers:\n{3}", day, performance, numCorrectChoices, totalNumCustomers);
        m_response = null;
        m_iconColor = Color.green;
        m_correctResponseAction = ResolutionAction.TransitionDay;
        m_incorrectResponseAction = ResolutionAction.None;
    }

    public void GenerateGameOver()
    {
        m_title = "You're FIRED!";
        m_sender = SENDER_BOSS;
        m_message = "You're not cut out to work at this company.\n\nYou're FIRE!";
        m_response = null;
        m_iconColor = Color.red;
        m_correctResponseAction = ResolutionAction.GameOver;
        m_incorrectResponseAction = ResolutionAction.None;
    }
    
}
