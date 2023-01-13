using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICanMove
{
    public Action EventOnStartMove { get; set; }
    public void MoveToPosition(Vector3 _position) { }
}
