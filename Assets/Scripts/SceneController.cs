using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void NextScene(string next_Scene)
    {
        SceneManager.LoadScene(next_Scene);
    }
}
