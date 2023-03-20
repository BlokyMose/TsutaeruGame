using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : ScriptableObject
{
    List<Hiragana> condition = new List<Hiragana>() {new Hiragana("tsu") };

    List<Hiragana> filledCondition;

    bool CheckCondition(Hiragana hiragana)
    {
        if (condition[filledCondition.Count] == hiragana)
        {
            filledCondition.Add(hiragana);
            return true;
        }

        return false;
    }

    public class Hiragana
    {
        public string name = "tsu";

        public Hiragana(string name)
        {
            this.name = name;
        }
    }
}

