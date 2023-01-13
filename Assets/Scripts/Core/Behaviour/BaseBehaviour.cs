using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseBehaviour : ScriptableObject
{
    public GameObject actor;
    public BaseInfo BaseInformation;
    public abstract bool IsFinished { get; protected set; }
    public abstract void Run();
    public virtual void Start() { }
    public virtual void Stop() { }
}
