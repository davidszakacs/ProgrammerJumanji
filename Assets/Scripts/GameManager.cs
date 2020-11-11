using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject answerPrefab;
    public GameObject questionPrefab;
    public GameObject paragraphPrefab;
    public GameObject codePrefab;
    public GameObject buttonPrefab;
    public GameObject canvas;
    public GameObject healthObject;
    public float answerSpacing = 40;
    
    
    private QuestionsHandler handler;
    private List<Question> _questions;
    private List<GameObject> _answers = new List<GameObject>();
    private Question _activeQuestion;
    private PlayerData _playerData = new PlayerData();
    private AudioSource audioData;
    private bool questionLoaded = false;
    public float lastUIBottom = 0f;
    private GameObject questionUI;
    
    void Start()
    { 
        //Making an instance of the QuestionsHandler and loading the questions
        handler = gameObject.AddComponent<QuestionsHandler>();
        handler.loadQuestions();
        _questions = handler.GetQuestions().questions;


        audioData = GetComponent<AudioSource>();
        
    }

    public void eventDisplayQuestion(int id)
    {
        displayQuestion(_questions[id]);
    }
    
    private void displayQuestion(Question question)
    {
        if (questionLoaded)
        {
            return;
        }
        _activeQuestion = question;
        //Making a new instance of the Question UI prefab
        questionUI = Instantiate(questionPrefab, new Vector2(-290, 0), Quaternion.identity);
        questionUI.transform.SetParent(canvas.transform, false);
        
        //Setting the title
        questionUI.transform.Find("Title").GetComponentInChildren<Text>().text = question.title;
        
        //Displaying the description
        GameObject desc = displayParagraph(questionUI, question.description, 0, 251);
        lastUIBottom = desc.GetComponent<RectTransform>().anchoredPosition.y - desc.GetComponent<RectTransform>().sizeDelta[1];
        
        //Display the code snippet
        lastUIBottom = desc.GetComponent<RectTransform>().anchoredPosition.y - desc.GetComponent<RectTransform>().sizeDelta[1];
        desc = displayCode(questionUI, question.codeSnippet, 0, lastUIBottom);
        
        //Displaying the question
        lastUIBottom = desc.GetComponent<RectTransform>().anchoredPosition.y - desc.GetComponent<RectTransform>().sizeDelta[1];
        desc = displayParagraph(questionUI, question.question, 0, lastUIBottom);
        
        //Displaying all the answer possibilities
        lastUIBottom = desc.GetComponent<RectTransform>().anchoredPosition.y - desc.GetComponent<RectTransform>().sizeDelta[1];
        foreach (var answer in question.answers)
        {
            GameObject temp = displayAnswer(questionUI, answer.value, 0, lastUIBottom - answerSpacing);
            lastUIBottom = temp.GetComponent<RectTransform>().anchoredPosition.y - desc.GetComponent<RectTransform>().sizeDelta[1];
        }
        
        //Displaying the submit button
        desc = displayButton(questionUI, 0, lastUIBottom - answerSpacing);
        lastUIBottom = desc.GetComponent<RectTransform>().anchoredPosition.y - desc.GetComponent<RectTransform>().sizeDelta[1];
        
        questionLoaded = true;
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
        int wrongId = 0;
        for(int i = 0; i < _answers.Count; ++i)
        {
            if (_answers[i].GetComponentInChildren<Toggle>().isOn != _activeQuestion.answers[i].correct)
            {
                correct = false;
                wrongId = i;
                break;
            }
        }

        if (!correct && _playerData.health > 0)
        {
            displayParagraph(questionUI, _activeQuestion.answers[wrongId].hint, lastUIBottom - answerSpacing);
            _playerData.health--;
            healthObject.transform.GetChild(_playerData.health).GetComponent<Image>().enabled = false;
            audioData.Play();
        }
        
        for(var i = 0; i < _answers.Count; ++i)
        {
            _answers[i].GetComponentInChildren<Toggle>().isOn = false;
        }
    }
    
    void Update()
    {
        
    }
}
