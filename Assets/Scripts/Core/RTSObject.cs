using RtsEngine.Abilities;
using RtsEngine.DataBase;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RtsEngine
{
    public abstract class RTSObject : ScriptableObject
    {
        public string Key;
        public BaseInfo BaseInformation;
        public BaseAbility[] Abilites;
        public StatsData Stats;
        public RTSObjectType ObjectType;

        public GameObject[] Model;
        public AnimatorOverrideController AnimatorOverride;

        public AudioClip[] OnSelectClips;
        public AudioClip[] OnGetCommandClips;
        public AudioClip[] OnCantExecuteCommandClips;
        public bool IsEmpty()
        {
            if (Key == null || Key == "")
            {
                return true;
            }
            return false;
        }
    }

}
