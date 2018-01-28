using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class NameGen {

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
            string[] split = name.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            m_last = split[split.Length - 1];
            m_first = "";
            for (int i = 0; i < split.Length - 1; i++)
            {
                m_first += split[0];
            }
        }
        public static bool operator ==(Name n1, Name n2) {
            return (n1.m_first.Equals(n2.m_first) && n1.m_last.Equals(n2.m_last));
        }

        public static bool operator !=(Name n1, Name n2) {
            return (!n1.m_first.Equals(n2.m_first) || !n1.m_last.Equals(n2.m_last));
        }
    }

    static string[] first_names = { "Andrew", "Adam", "Allen", "Arush", "Akira", "Brad", "Ben", "Bill", "Chen", "Chad", "Chris", " Carl",
    "Dan", "Dave", "Clark", "Dakota", "Evan", "Ethan", "George", "Giovanni", "Gary", "Henry", "Harold", "Ivan", "Ishmael", "Jose", "John", "Jim", "Juan", "Jordan", "Kent", "Kyle", "Kenji",
    "Lee", "Li", "Leonard", "Mike", "Matt", "Matthew", "Mark", "Nigel", "Otis", "Pete", "Phil", "Quincy", "Ron", "Rami", "Rich", "Steven", "Sam", "Stan", "Ted", "Tim",
    "Uriel", "Vlad", "Victor", "William", "Yancy", "Alice", "Anita", "Beth", "Barbara", "Cathy", "Candice", "Delilah", "Dolly", "Edith", "Enid", "Faye", "Fran",
    "Gina", "Heather", "Harriet", "Ima", "Ilsa", "Jane", "Jennifer", "Julia", "Kathy", "Kelly", "Kate", "Kim", "Lucy", "Lola", "Mei", "Maria", "Margaret", "Nancy", "Natalia",
    "Ophelia", "Peggy", "Penny", "Patricia", "Quinn", "Rose", "Rachel", "Susan", "Shelly", "Tabitha", "Tori", "Uma", "Valerie", "Victoria", "Wendy", "Yolanda", "Zelda"};
    static string[] last_names = { "Ha", "Peng", "Lee", "Cazamias", "DiNola", "Palacios", "Korolog",
        "Smith", "Jones", "Anderson", "Baldwin", "Garcia", "Brown", "Wilson", "Thomas", "Taylor", "Lee",
        "Moore", "Jackson", "Martin", "White", "Flores", "King", "Scott", "Adams", "Nelson", "Mitchel", "Diaz",
        "Turner", "Carter", "Gomez", "Wright", "Hall", "Young", "Baker", "Morgan", "Ortiz", "Morales", "Dicaprio",
        "Foster", "Price", "Vasquez", "Chavez", "Cooper", "Bell", "Reed", "Williams", "Wood", "Brooks", "Stark",
        "Potter", "Lannister", "Yronwode", "Kardashian", "Kirk", "Picard", "Wick", "Kent", "Wayne", "Parker", "Hughes",
        "Kelly"
    };

    public static Name GetName() {
        string first = first_names[UnityEngine.Random.Range(0, first_names.Length)];
        string last = last_names[UnityEngine.Random.Range(0, last_names.Length)];
        return new Name(first, last);
    }
}