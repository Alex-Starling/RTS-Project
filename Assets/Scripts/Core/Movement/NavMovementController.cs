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
        [field:SerializeField] public NavMeshAgent Agent { get; private set; }

        public Action EventOnStartMove { get; set; }

        private void Awake()
        {
            Agent = GetComponent<NavMeshAgent>();

        }
        public void MoveToPosition(Vector3 _position)
        {
            EventOnStartMove?.Invoke();
            Agent.SetDestination(_position);
        }
    }

}