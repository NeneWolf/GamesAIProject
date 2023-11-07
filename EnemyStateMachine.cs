using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyStateMachine : StateManager<EnemyStateMachine>
{
    // States of the enemy
    [Header("EnemyStats")]
    int currentHealth;
    [SerializeField] int maxHealth = 100;
    [SerializeField] int damage = 10;



    private void Awake()
    {
        currentState = states[EEnemyState.Idle];
    }
}

