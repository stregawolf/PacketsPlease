using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivityData {

    public struct Activity {
        public enum Type : int {
            GAME = 0,
            SITE = 1,
            STREAM = 2,
            SELF = 3
        }

        public string m_name;
        public Type m_type;

        public Activity(string name, Type type) {
            m_name = name;
            m_type = type;
        }

        public override string ToString() {
            return string.Format(
                "Activity(Name {0} Type {1})",
                m_name, m_type
            );
        }
    }
    
    public static Activity GetActivity(string name)
    {
        if (name != "")
        {
            for (int i = 0; i < Activities.Length; i++)
            {
                if (name.ToLower() == Activities[i].m_name.ToLower())
                {
                    return Activities[i];
                }
            }
        }
        return GetActivity();
    }

    public static Activity GetActivity(Activity? exclude = null)
    {
        if (exclude != null)
        {
            for(int i=0; i<Activities.Length; i++)
            {
                if(((Activity)exclude).m_name == Activities[i].m_name)
                {
                    return Activities[(i + Random.Range(1, Activities.Length - 2)) % Activities.Length];
                }
            }
        }
        return Activities[Random.Range(0, ActivityData.Activities.Length)];
    }
    
    public static Activity GetActivityByType(ActivityData.Activity.Type type)
    {
        int[] randIndex = ShuffleUtils.MakeShuffledIntArray(Activities.Length);

        for(int i = 0; i < randIndex.Length; i++)
        {
            if(Activities[i].m_type == type)
            {
                return Activities[i];
            }
        }

        throw new System.Exception("GetActivityByType: Could not find activity of type " + type);
    }

    public static Activity GetActivityByName(string name)
    {
        for(int i = 0; i < Activities.Length; i++)
        {
            if(Activities[i].m_name.ToLower() == name.ToLower())
            {
                return Activities[i];
            }
        }

        throw new System.Exception("GetActivityByName: Could not find activitiy with name " + name);
    }

    public static Activity[] Activities =
    {
        new Activity("Tremblr",                     Activity.Type.SITE),
        new Activity("2Chains",                     Activity.Type.SITE),
        new Activity("Birbsite",                    Activity.Type.SITE),
        new Activity("Zzzfeed",                     Activity.Type.SITE),
        new Activity("dBuy",                        Activity.Type.SITE),
        new Activity("Sahara",                      Activity.Type.SITE),
        new Activity("Gargoyl",                     Activity.Type.SITE),
        new Activity("Facezine",                    Activity.Type.SITE),
        new Activity("Gametaco",                    Activity.Type.SITE),
        new Activity("Hoop",                        Activity.Type.STREAM),
        new Activity("Flick-Net",                   Activity.Type.STREAM),
        new Activity("TWatch",                      Activity.Type.STREAM),
        new Activity("Tubesite",                    Activity.Type.STREAM),
        new Activity("Just a TV",                   Activity.Type.STREAM),
        new Activity("War of Worldcraft",           Activity.Type.GAME),
        new Activity("Guild of Greats",             Activity.Type.GAME),
        new Activity("Person Unheard's Bubble Gum", Activity.Type.GAME),
        new Activity("Vapor",                       Activity.Type.GAME),
        new Activity("Uberwatch",                   Activity.Type.GAME),
        new Activity("CosmoCAST",                   Activity.Type.GAME),
    };
}