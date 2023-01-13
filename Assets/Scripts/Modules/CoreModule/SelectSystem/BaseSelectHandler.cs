using RtsEngine.AI;
using RtsEngine.Buildings;
using RtsEngine.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RtsEngine
{
    public abstract class BaseSelectHandler : MonoBehaviour
    {
        public Action EventOnUnitSelected;

        [HideInInspector] public bool CanSelect = true;
        [HideInInspector] public bool GroupedUnit;
        [SerializeField] protected GameObject SelectVisualObject;

        public GroupAI GroupParent { get; set; }
        //public Transform GroupParentTransform { get; set; }
        public BaseSelectHandler ParentGroupAI { get; set; }
        public BaseUnit baseUnit { get; set; }
        public BaseBuilding baseBuilding { get; set; }

        protected virtual void Start()
        {
            Init();
        }

        protected abstract void Init();

        public abstract (string, RTSObjectType) GetInfoAboutSelectedObject();

        public virtual void OnSelect()
        {
            EventOnUnitSelected?.Invoke();
        }

        public virtual void DeSelect()
        {
            SwitchSelectVisual(false);

        }

        public void SwitchSelectVisual(bool value)
        {
            if (SelectVisualObject)
            {
                SelectVisualObject.SetActive(value);
                
            }
        }

        public virtual void OnAddedToSelectList()
        {
        }
    }
}
