using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTSEngine.Events;
using System;

namespace RTSEngine.Units
{
    [RequireComponent(typeof(UnitView))]
    public class SelectController : MonoBehaviour, ICanSelected
    {
        public Action EventOnUnitSelected;

        [SerializeField]
        private GameObject SelectVisualObject;
        public UnitView Unit { get; set; }

        protected void Start()
        {
            Init();
        }

        public void OnSelect()
        {
            SwitchSelectVisual(true);
            EventOnUnitSelected?.Invoke();
        }

        public void DeSelect()
        {
            SwitchSelectVisual(false);
        }

        public void SwitchSelectVisual(bool value)
        {
            if (SelectVisualObject)
                SelectVisualObject.SetActive(value);
        }

        public (string, UnitType) GetInfoAboutSelectedObject()
        {
            return (Unit.Key, UnitType.Character);
        }

        protected void Init()
        {
            if (!Unit)
                Unit = GetComponent<UnitView>();
        }

        public void OnAddedToSelectList()
        {
            SwitchSelectVisual(true);
        }
    }
}

