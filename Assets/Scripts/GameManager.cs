using System.Collections;
using System.Collections.Generic;
using DTInventory;
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
    public bool questionLoaded = false;
    public int progress = 0;
    
    private QuestionsHandler handler;
    private List<Question> _questions;
    private List<GameObject> _answers = new List<GameObject>();
    private Question _activeQuestion;
    private PlayerData _playerData = new PlayerData();
    private AudioSource audioData;
    public float lastUIBottom = 0f;
    private GameObject questionUI;
    private InventoryManager _inventory;
    private GameObject player;
    private Vector2 linearBackup;
    private float angularBackup;
    private GameObject lastHint;
    private AnswerHandler ans_handler;
    private int questionLoadedID;
    
    void Start()
    { 
        //Making an instance of the QuestionsHandler and loading the questions
        handler = gameObject.AddComponent<QuestionsHandler>();
        ans_handler = GetComponent<AnswerHandler>();
        handler.loadQuestions();
        _questions = handler.GetQuestions().questions;
        
        _inventory = FindObjectOfType<InventoryManager>();
        player = GameObject.Find("Player");
        audioData = GetComponent<AudioSource>();
        
        UnityEngine.EventSystems.EventSystem.current.sendNavigationEvents = !UnityEngine.EventSystems.EventSystem.current.sendNavigationEvents;
        
        for (int i = 0; i < progress; ++i)
        {
            ans_handler.executeAnswer(i);
        }
        
    }

    public void eventDisplayQuestion(int id)
    {
        if (progress <= id)
        {
            displayQuestion(_questions[id]);
            questionLoadedID = id;
            progress = id+1;
        }
    }
    
    private void displayQuestion(Question question)
    {
        if (questionLoaded || questionUI != null)
        {
            return;
        }
        linearBackup = player.GetComponent<Rigidbody2D>().velocity;
        angularBackup = player.GetComponent<Rigidbody2D>().angularVelocity;
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        player.GetComponent<Rigidbody2D>().angularVelocity = 0f;
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        
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
        desc = displayCode(questionUI, question.codeSnippet, 0, lastUIBottom-10);
        
        //Displaying the question
        lastUIBottom = desc.GetComponent<RectTransform>().anchoredPosition.y - desc.GetComponent<RectTransform>().sizeDelta[1];
        desc = displayParagraph(questionUI, question.question, 0, lastUIBottom-10);
        
        //Displaying all the answer possibilities
        lastUIBottom = desc.GetComponent<RectTransform>().anchoredPosition.y - desc.GetComponent<RectTransform>().sizeDelta[1];
        foreach (var answer in question.answers)
        {
            GameObject temp = displayAnswer(questionUI, answer.value, 0, lastUIBottom - 3 - answerSpacing);
            lastUIBottom = temp.GetComponent<RectTransform>().anchoredPosition.y - desc.GetComponent<RectTransform>().sizeDelta[1];
        }
        
        //Displaying the submit button
        desc = displayButton(questionUI, 0, lastUIBottom - 10 - answerSpacing);
        lastUIBottom = desc.GetComponent<RectTransform>().anchoredPosition.y - desc.GetComponent<RectTransform>().sizeDelta[1];
        
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        
        questionLoaded = true;
    }

    private void hideQuestion(float delay = 0f, bool loadNext = false)
    {
        questionLoaded = false;
        if (questionUI != null)
        {
            Destroy(questionUI, delay);
        }
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        player.GetComponent<Rigidbody2D>().velocity = linearBackup;
        player.GetComponent<Rigidbody2D>().angularVelocity = angularBackup;
        _activeQuestion = null;
        _answers.Clear();
        if(!_inventory.isOpen)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    private GameObject displayParagraph(GameObject parent, string text, float x = 0f, float y = 0f)
    {        
        int lines = text.Split('\n').Length;
        lines *= 11;
        GameObject temp = Instantiate(paragraphPrefab, new Vector2(x, y), Quaternion.identity);
        temp.transform.SetParent(parent.transform, false);
        temp.GetComponentInChildren<TextMeshProUGUI>().text = text;
        Vector2 textSize = temp.GetComponentInChildren<TextMeshProUGUI>().GetPreferredValues(text);
        temp.GetComponent<RectTransform>().sizeDelta = new Vector2(520, textSize.y);
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
        if (!questionLoaded || _answers.Count == 0)
        {
            return;
        }
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
            if (lastHint != null)
            {
                Destroy(lastHint, 0f);
                lastHint = null;
            }
            lastHint = displayParagraph(questionUI, _activeQuestion.answers[wrongId].hint, 0, lastUIBottom - answerSpacing);
            _playerData.health--;
            healthObject.transform.GetChild(_playerData.health).GetComponent<Image>().enabled = false;
            audioData.Play();
            for(var i = 0; i < _answers.Count; ++i)
            {
                _answers[i].GetComponentInChildren<Toggle>().isOn = false;
            }
        }
        else if (correct)
        {
            if (lastHint != null)
            {
                Destroy(lastHint, 0f);
                lastHint = null;
            }

            ans_handler.executeAnswer(questionLoadedID);
            lastHint = displayParagraph(questionUI, _activeQuestion.correctText, 0, lastUIBottom - answerSpacing);
            if (_activeQuestion.autoLoad && questionLoadedID < _questions.Count - 1)
            {
                eventDisplayQuestion(questionLoadedID+1);
            }
            hideQuestion(_activeQuestion.readDelay);
        }
    }
    
    void Update()
    {
        
    }
}
