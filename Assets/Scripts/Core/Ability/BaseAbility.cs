using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RTSEngine.Abilities
{
    public abstract class BaseAbility : ScriptableObject
    {
        public BaseInfo BaseInformation;
        public abstract void Activate();
        public virtual void DeActivate() { }
    }

}
