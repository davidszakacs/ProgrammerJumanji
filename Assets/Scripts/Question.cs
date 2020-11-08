using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Question
{
    public string question;
    public Answers answers = new Answers();
}
