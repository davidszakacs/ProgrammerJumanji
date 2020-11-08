using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class QuestionsHandler : MonoBehaviour
{
    private Questions questions = new Questions();
    
    void Start()
    {
        loadQuestions();
    }

    public void loadQuestions()
    {
        using (StreamReader file = new StreamReader(Application.dataPath + "/questions.json"))
        {
            string json = file.ReadToEnd();

            questions = JsonUtility.FromJson<Questions>(json);
        }
    }

    public Questions GetQuestions()
    {
        return questions;
    }
}
