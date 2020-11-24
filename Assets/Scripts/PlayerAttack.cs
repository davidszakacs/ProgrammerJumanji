using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private float timeBtwAttack;
    public float startTimeBtwAttack;
    
    private float timeBtwDraw;
    public float startTimeBtwDraw;

    public Transform attackPos;
    public float attackRange;

    public LayerMask whatIsEnemies;
    public float damage;
    public bool swordDrawn;
    public bool canUseSword;
    
    public Animator _animator;
    
    private void Update()
    {
        
        if (timeBtwDraw <= 0 && Input.GetKeyDown(KeyCode.Z) && canUseSword)
        {
            if (!swordDrawn)
            {
                _animator.SetTrigger("DrawSword");
                _animator.SetBool("SwordDrawn", true);
            }
            else
            {
                _animator.SetTrigger("HideSword");
                _animator.SetBool("SwordDrawn", false);
            }
            swordDrawn = !swordDrawn;
            timeBtwDraw = startTimeBtwDraw;
        }
        else
        {
            timeBtwDraw -= Time.deltaTime;
        }
        if (timeBtwAttack <= 0 && swordDrawn)
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsEnemies);
                for (int i = 0; i < enemiesToDamage.Length; i++)
                {
                    enemiesToDamage[i].GetComponent<Enemy>().TakeDamage(damage);
                }
                timeBtwAttack = startTimeBtwAttack;
                _animator.SetTrigger("SwingSword");
            }
        }
        else
        {
            timeBtwAttack -= Time.deltaTime;
        }
    }

    private void OnDrawGizmosSelected()
    {
      Gizmos.color = Color.magenta;
      Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }
}
