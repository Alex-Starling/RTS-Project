using RTSEngine.Abilities;
using RTSEngine.DataBase;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RTSEngine
{
    [CreateAssetMenu]
    public class UnitModel : ScriptableObject
    {
        [Header("Main")]
        public string Key;
        public BaseInfo BaseInformation;
        public BaseAbility[] Abilites;
        public StatsData Stats;
        public UnitType UnitType;
        [Header("Visual")]
        public GameObject[] Model;
        public AnimatorOverrideController AnimatorOverride;
        public float Size;
        [Header("Speech")]
        public AudioClip[] OnSelectClips;
        public AudioClip[] OnGetCommandClips;
        public AudioClip[] OnCantExecuteCommandClips;
        [Header("Movement")]
        public MovementType MovementType = MovementType.NavMesh;
        public float AgentRadius = 0.25f;

        public bool IsEmpty()
        {
            if (Key == null || Key == "")
            {
                return true;
            }
            return false;
        }
    }
    public enum MovementType
    {
        None,
        NavMesh,
    }
}
