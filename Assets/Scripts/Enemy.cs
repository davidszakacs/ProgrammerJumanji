using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health = 100f;
    public GameObject bloodEffect;
    private float dazedTime;
    public float baseSpeed;
    private float speed;
    public float startDazedTime;
    public float mostLeft;
    public float mostRight;
    private bool m_FacingRight;

    public Animator _animator;

    private void Update()
    {
        if (dazedTime <= 0)
        {
            _animator.SetBool("Dazed", false);
            speed = baseSpeed;
        }
        else
        {
            speed = 0;
            dazedTime -= Time.deltaTime;
        }
        if (health <= 0)
        {
            Destroy(gameObject);
        }

        // If the input is moving the player right and the player is facing left...
        if (m_FacingRight)
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);
        }

        if (Math.Abs(transform.position.x - mostLeft) < 0.4f || Math.Abs(transform.position.x - mostRight) < 0.4f)
        {
            Flip();
        }
    }

    public void TakeDamage(float damage)
    {
        dazedTime = startDazedTime;
        _animator.SetBool("Dazed", true);
        GameObject temp = Instantiate(bloodEffect, transform.position, Quaternion.identity);
        health -= damage;
        Destroy(temp, 1);
    }


    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
