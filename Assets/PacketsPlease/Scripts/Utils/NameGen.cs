using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class NameGen {

    private static List<string> first_names;
    private static List<string> last_names;

    static NameGen() {
        first_names = new List<string>();
        last_names = new List<string>();
        TextAsset t = Resources.Load("names") as TextAsset;
        if (t != null) {
            string data = t.text;
            string[] lines = data.Split(new Char[] { }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string line in lines) {
                string[] fl = line.Split(new Char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                first_names.Add(fl[0]);
                last_names.Add(fl[1]);
            }
        }
        else {
            Debug.LogWarning("Missing names in Resources");
        }
    }

    public static string GetName() {
        string first = first_names[UnityEngine.Random.Range(0, first_names.Count)];
        string last = last_names[UnityEngine.Random.Range(0, first_names.Count)];
        return first + " " + last;
    }
}