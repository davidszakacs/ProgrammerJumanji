using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnswerHandler : MonoBehaviour
{
    public GameObject Player;
    public GameObject checkPoints;
    public void executeAnswer(int code)
    {
        switch (code)
        {
            case 0:
            {
                Player.GetComponent<CharacterMovement>().enabled = true;
                break;
            }
            case 1:
            {
                Player.GetComponent<CharacterMovement>().canJump = true;
                break;
            }
            case 2:
            {
                Player.GetComponent<PlayerAttack>().canUseSword = true;
                break;
            }
        }
        Destroy(checkPoints.transform.Find("CheckPoint ("+code+")").gameObject, 0);
    }
}
