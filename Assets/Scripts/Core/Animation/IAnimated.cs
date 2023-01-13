using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAnimated 
{
    public void PlayAnimation(AnimType animType);
}
public enum AnimType 
{ 

    Ilde,
    Walk,
    Run,
    Die,
    Attack,
    Hit,
}

