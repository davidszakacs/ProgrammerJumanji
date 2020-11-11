using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class TriggerHandler : MonoBehaviour
{
    public UnityEvent m_MyEvent = new UnityEvent();


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "Player")
        {
            m_MyEvent.Invoke();
        }
    }
}
