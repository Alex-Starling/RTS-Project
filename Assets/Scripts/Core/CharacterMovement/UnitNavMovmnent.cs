using RtsEngine.AI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(NavMeshAgent))]
public class UnitNavMovmnent : MonoBehaviour, ICanMove
{
    public NavMeshAgent _navAgent { get; private set; }

    public Action EventOnStartMove { get; set; }

    private void Awake()
    {
        _navAgent = GetComponent<NavMeshAgent>();
        
    }
    public void MoveToPosition(Vector3 _position) 
    {
        //Debug.Log($"MOVE to {_position}");
        EventOnStartMove?.Invoke();
        _navAgent.SetDestination(_position);
    }
}
