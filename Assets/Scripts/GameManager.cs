using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private QuestionsHandler handler;
    private List<Question> _questions;
    public GameObject questionPrefab;
    public GameObject canvas;
    public float answerSpacing = 40;
    void Start()
    { 
        handler = gameObject.AddComponent<QuestionsHandler>();
        handler.loadQuestions();
        _questions = handler.GetQuestions().questions;
        float buttonHeight = questionPrefab.GetComponent<RectTransform>().rect.height;
        float lastY = 0;
        foreach (var question in _questions[0].answers.answers)
        {
            Debug.Log(question.value);
            var newQuestion = Instantiate(questionPrefab, new Vector2(0, lastY+answerSpacing), Quaternion.identity);
            newQuestion.transform.SetParent(canvas.transform, false);
            newQuestion.GetComponentInChildren<Text>().text = question.value;
            lastY += answerSpacing;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
