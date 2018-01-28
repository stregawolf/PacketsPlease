using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivityData {

    public struct Activity
    {
        public enum Type
        {
            GAME = 0,
            SITE = 1,
            STREAM = 2,
            SELF = 3
        }

        public string m_name;
        public Type m_type;

        public Activity(string name, Type type)
        {
            m_name = name;
            m_type = type;
        }
    }

    public static Activity[] Activies =
    {
        new Activity("Tremblr",                     Activity.Type.SITE),
        new Activity("2Chains",                     Activity.Type.SITE),
        new Activity("Cheepen",                     Activity.Type.SITE),
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