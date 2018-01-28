using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class NameGen {

    private static List<string> boy_names = new List<string>();
    private static List<string> girl_names = new List<string>();
    private static List<string> last_names = new List<string>();

    public struct Name
    {
        public string m_first;
        public string m_last;
        public Name(string first, string last)
        {
            m_first = first;
            m_last = last;
        }

        public string LastFirst { get { return m_last + ", " + m_first; } }
        public string FirstLast { get { return m_first + ", " + m_last; } }

        public void Set(string name)
        {
            string[] split = name.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries );
            m_last = split[split.Length - 1];
            m_first = "";
            for(int i=0; i<split.Length-1; i++)
            {
                m_first += split[0];
            }
        }
    }

    static NameGen() {
        TextAsset t = Resources.Load("names") as TextAsset;
        if (t != null) {
            string data = t.text;
            string[] lines = data.Split(new Char[] { }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string line in lines) {
                string[] fl = line.Split(new Char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if(fl.Length == 1)
                {
                    last_names.Add(fl[0]);
                }
                else if (fl[3].Contains("boy"))
                {
                    boy_names.Add(fl[1].Replace("\"", ""));
                }
                else
                { 
                    girl_names.Add(fl[1].Replace("\"", ""));
                }
            }
        }
        else {
            Debug.LogWarning("Missing names in Resources");
        }
    }

    public static Name GetName(bool boy) {
        string first = boy ? boy_names[UnityEngine.Random.Range(0, boy_names.Count)] : girl_names[UnityEngine.Random.Range(0, girl_names.Count)];
        string last = last_names[UnityEngine.Random.Range(0, last_names.Count)];
        return new Name(first, last);
    }
}