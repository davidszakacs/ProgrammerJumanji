using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Question
{
    public string title;
    public string description;
    public string question;
    public string codeSnippet;
    public string correctText;
    public float readDelay;
    public bool autoLoad;
    public List<Answer> answers = new List<Answer>();
}
