using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private QuestionsHandler handler;
    private List<Question> _questions;
    public GameObject answerPrefab;
    public GameObject questionPrefab;
    public GameObject paragraphPrefab;
    public GameObject codePrefab;
    public GameObject buttonPrefab;
    public GameObject canvas;
    public float answerSpacing = 40;
    private List<GameObject> _answers = new List<GameObject>();
    private Question _activeQuestion;
    void Start()
    { 
        //Making an instance of the QuestionsHandler and loading the questions
        handler = gameObject.AddComponent<QuestionsHandler>();
        handler.loadQuestions();
        _questions = handler.GetQuestions().questions;
        
        
        Question temp = GetQuestion(0);
        displayQuestion(temp);
    }

    private Question GetQuestion(int id)
    {
        return _questions[id];
    }

    private void displayQuestion(Question question)
    {
        _activeQuestion = question;
        //Making a new instance of the Question UI prefab
        var newQuestion = Instantiate(questionPrefab, new Vector2(-290, 0), Quaternion.identity);
        newQuestion.transform.SetParent(canvas.transform, false);
        
        //Setting the title
        newQuestion.transform.Find("Title").GetComponentInChildren<Text>().text = question.title;
        
        //Displaying the description
        GameObject desc = displayParagraph(newQuestion, question.description, 0, 251);
        float bottom = desc.GetComponent<RectTransform>().anchoredPosition.y - desc.GetComponent<RectTransform>().sizeDelta[1];
        
        //Display the code snippet
        bottom = desc.GetComponent<RectTransform>().anchoredPosition.y - desc.GetComponent<RectTransform>().sizeDelta[1];
        desc = displayCode(newQuestion, question.codeSnippet, 0, bottom);
        
        //Displaying the question
        bottom = desc.GetComponent<RectTransform>().anchoredPosition.y - desc.GetComponent<RectTransform>().sizeDelta[1];
        desc = displayParagraph(newQuestion, question.question, 0, bottom);
        
        //Displaying all the answer possibilities
        bottom = desc.GetComponent<RectTransform>().anchoredPosition.y - desc.GetComponent<RectTransform>().sizeDelta[1];
        foreach (var answer in question.answers)
        {
            GameObject temp = displayAnswer(newQuestion, answer.value, 0, bottom - answerSpacing);
            bottom = temp.GetComponent<RectTransform>().anchoredPosition.y - desc.GetComponent<RectTransform>().sizeDelta[1];
        }
        
        //Displaying the submit button
        desc = displayButton(newQuestion, 0, bottom - answerSpacing);
    }

    private GameObject displayParagraph(GameObject parent, string text, float x = 0f, float y = 0f)
    {        
        int lines = text.Split('\n').Length;
        lines *= 11;
        GameObject temp = Instantiate(paragraphPrefab, new Vector2(x, y), Quaternion.identity);
        temp.transform.SetParent(parent.transform, false);
        temp.GetComponentInChildren<Text>().text = text;
        RectTransform rectTransform = temp.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(520, lines);
        return temp;
    }

    private GameObject displayCode(GameObject parent, string text, float x = 0f, float y = 0f)
    {        
        int lines = text.Split('\n').Length + 2;
        lines *= 13;
        GameObject temp = Instantiate(codePrefab, new Vector2(x, y - lines/2), Quaternion.identity);
        temp.transform.SetParent(parent.transform, false);
        temp.GetComponentInChildren<TextMeshProUGUI>().text = text;
        RectTransform rectTransform = temp.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(520, lines);
        rectTransform = temp.transform.GetChild(0).GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(500, lines-13);
        return temp;
    }

    private GameObject displayAnswer(GameObject parent, string text, float x = 0f, float y = 0f)
    {
        GameObject newAnswer = Instantiate(answerPrefab, new Vector2(x, y), Quaternion.identity);
        newAnswer.transform.SetParent(parent.transform, false);
        newAnswer.GetComponentInChildren<Text>().text = text;
        _answers.Add(newAnswer);
        return newAnswer;
    }

    private GameObject displayButton(GameObject parent, float x = 0f, float y = 0f)
    {
        GameObject newButton = Instantiate(buttonPrefab, new Vector2(x, y), Quaternion.identity);
        newButton.transform.SetParent(parent.transform, false);
        newButton.GetComponentInChildren<Text>().text = "Submit answer";
        newButton.GetComponent<Button> ().onClick.AddListener ( delegate { checkAnswer(); });
        return newButton;
    }

    private void checkAnswer()
    {
        bool correct = true;
        for(var i = 0; i < _answers.Count; ++i)
        {
            if (_answers[i].GetComponentInChildren<Toggle>().isOn != _activeQuestion.answers[i].correct)
            {
                correct = false;
                break;
            }
        }

        if (correct)
        {
            Debug.Log("Correct answers!!");
        }
    }
    
    void Update()
    {
        
    }
}
