using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NotificationData", menuName = "Data/NotificationData", order = 1)]
public class NotificationData : ScriptableObject
{
    public const string SENDER_BOSS = "Mr. Bossman Your Boss";
    public const string SENDER_HR = "hr@cosmocast.com";
    public const string SENDER_COMPLIANCE = "compliance@cosmocast.com";

    public int m_ruleIndex = 0;

    public string m_title;
    [HideInInspector]
    public string m_message;
    public string m_sender;
    public bool m_pinned;         // Pinned notifications go back to the top of the stack when closed
    
    public enum IconType
    {
        regular = 0,
        strike,
        endOfDay,
        gameOver,
    }
    public IconType m_iconType = IconType.regular;

    public bool m_autoOpen = false;
    public bool m_isStrike = false;
    public bool m_forceToTop;

    [HideInInspector]
    public StoryData m_parentStory;

    public enum ResolutionAction
    {
        None = 0,
        TransitionDay,
        GameOver,
        EndStory,
        Strike,
        PostNotificationA,
        PostNotificationB,
        PostNotificationC,
        PostCustomerA,
        PostCustomerB,
        PostCustomerC
    }

    public enum StrikeReason
    {
        None = 0,
        WrongAction,
        QueueFull,
        HRViolation
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
    
    public void GenerateStrike(int number, StrikeReason reason = StrikeReason.None, string customMessage = "")
    {
        string reasonString = "";
        switch (reason)
        {
            case StrikeReason.QueueFull:
                {
                    m_title = "Queue Full Violation";
                    reasonString = "Resolution queue full. Please process customers in a more timely manner.";
                    m_sender = SENDER_COMPLIANCE;
                }
                break;
            case StrikeReason.HRViolation:
                {
                    m_title = "HR Violation";
                    reasonString = "CosmoCast Employees are expected to behave respectfully and courteously to all staff.";
                    m_sender = SENDER_HR;
                }
                break;
            case StrikeReason.WrongAction:
                {
                    m_title = "Rule Compliance Violation";
                    reasonString = customMessage;
                    m_sender = SENDER_COMPLIANCE;
                    m_response = new Response("Acknowledged", "Eat my butt", Response.CorrectResponse.CHOICE_A);
                    m_response.m_strikeOnIncorrect = true;
                }
                break;
        }

        m_message = string.Format("<color=#FFAAAA>WARNING</color>: Performance dismerit <color=#FFAAAA>{0}</color>/<color=#FFAAAA>{1}</color>. Failure to correct performance will result in immediate termination.\n\n{2}",
            number, PacketsPleaseMain.Instance.m_maxStrikes, reasonString);
        m_iconType = IconType.strike;
        m_autoOpen = true;
        m_isStrike = true;
    }

    public void GenerateEndOfDay(int day, int numCorrectChoices, int totalNumCustomers)
    {
        m_title = string.Format("End of Day {0}", day);
        m_sender = SENDER_BOSS;
        float performance = (float)numCorrectChoices / (float)totalNumCustomers;
        m_message = string.Format("You have completed day {0}.\n\nPerformance report:\n{1:P0}\n\nCorrect choices:\n{2}\nTotal customers:\n{3}", day, performance, numCorrectChoices, totalNumCustomers);
        m_response = null;
        m_iconType = IconType.endOfDay;
        m_correctResponseAction = ResolutionAction.TransitionDay;
        m_incorrectResponseAction = ResolutionAction.None;
        m_autoOpen = true;
    }

    public void GenerateGameOver()
    {
        m_title = "You're FIRED!";
        m_sender = SENDER_BOSS;
        m_message = "That's the last strike!\n\nYou're not cut out to work at this company.\n\nYOU ARE FIRED!!!";
        m_response = null;
        m_iconType = IconType.gameOver;
        m_correctResponseAction = ResolutionAction.GameOver;
        m_incorrectResponseAction = ResolutionAction.None;
        m_autoOpen = true;
    }

    public void GenerateCredits()
    {
        m_title = "Credits";
        m_sender = "Global Game Jam 2018 - Transmission";
        m_message =
            "Cosmic Adventure Squad (@squad_cosmic)\nwww.cosmicadventuresquad.com\n\n" +
            "Gameplay Code - Vu Ha (@stregawolf)\n" +
            "Artist - Rose Peng (@ouroborose)\n" +
            "System Design & Code - Ted DiNola (@esdin)\n" +
            "Code & Testing - Jordan Cazamias (jaycaz.carbonmade.com​)\n" +
            "Audio - Niko Korolog (www.soundcloud.com/nikokorolog)\n" +
            "Character Artist - Chris Palacios (www.christopherjpalacios.com)\n" +
            "Intern - Andrew Lee (@alee12131415)";

        m_response = null;
        m_iconType = IconType.regular;
        m_pinned = true;
        m_correctResponseAction = ResolutionAction.None;
        m_incorrectResponseAction = ResolutionAction.None;
        m_autoOpen = false;
    }
    
}
