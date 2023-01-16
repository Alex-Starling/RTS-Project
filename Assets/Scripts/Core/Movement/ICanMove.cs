using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public interface ICanMove
{
    public NavMeshAgent Agent { get; }
    public Action EventOnStartMove { get; set; }
    public void MoveToPosition(Vector3 _position) { }
}
