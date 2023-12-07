using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public event EventHandler OnDead;

    [SerializeField] private float heath = 100;


    public void Damage(int damageAmount)
    {
        heath -= damageAmount;
        if (heath < 0)
        {
            heath = 0;
        }

        if(heath == 0){
            Die();
        }
        Debug.Log(heath);
    }

    private void Die(){
        OnDead?.Invoke(this,EventArgs.Empty);
    }

}
