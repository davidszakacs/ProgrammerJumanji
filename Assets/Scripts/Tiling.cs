using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tiling : MonoBehaviour
{
    public Transform target;
    private Vector2 m_LastTargetPosition;
    // Start is called before the first frame update
    void Start()
    {
        m_LastTargetPosition = target.position;
        transform.parent = null;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
