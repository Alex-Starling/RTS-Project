using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RTSEngine
{
    [RequireComponent(typeof(NavMeshAgent))]

    public class NavMovementController : MonoBehaviour, ICanMove
    {
        public NavMeshAgent _navAgent { get; private set; }

        public Action EventOnStartMove { get; set; }

        private void Awake()
        {
            _navAgent = GetComponent<NavMeshAgent>();

        }
        public void MoveToPosition(Vector3 _position)
        {
            EventOnStartMove?.Invoke();
            _navAgent.SetDestination(_position);
        }
    }

}