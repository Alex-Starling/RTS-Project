using RTSEngine.Abilities;
using RTSEngine.DataBase;
using RTSEngine.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RTSEngine.UI
{
    public class InGameUI : MonoBehaviour
    {
        [Header("Abilities")]
        public GameObject AbilityButtonPrefab;
        public GameObject AbilitiesPanel;
        [Header("MiniMap")]
        public GameObject MiniMap;
        [Header("SelectedObject")]
        public GameObject SelectedObjectPanel;
        public Image SelectedObjectIcon;
        public Text SelectedObjectName;
        [Header("DefaultConfig")]
        public Sprite EmptyImage;

        private void Start()
        {
            InitEvents();
        }

        private void UpdateAbilitesPanel(UnitModel _selectedObject)
        {
            ClearAbilitesPanel();
            if (EntityIsEmpty(_selectedObject))
            {
                SelectedObjectName.text = "";
                SelectedObjectIcon.sprite = EmptyImage;

                return;
            }
            BaseAbility[] abilities = null;
            switch (_selectedObject.UnitType)
            {
                case UnitType.Character:
                    abilities = _selectedObject.Abilites;
                    break;
                case UnitType.Building:
                    abilities = _selectedObject.Abilites;
                    break;
                default:
                    break;
            }

            if (abilities != null)
            {
                foreach (var item in abilities)
                {
                    var go = GameObject.Instantiate(AbilityButtonPrefab, AbilitiesPanel.transform);
                }
            }

        }

        private void ClearAbilitesPanel()
        {
            foreach (Transform item in AbilitiesPanel.transform)
            {
                GameObject.Destroy(item.gameObject);
            }
        }

        private bool EntityIsEmpty(UnitModel _selectedObject)
        {
            if (!_selectedObject)
            {
                return true;
            }
            if (_selectedObject.Key == null || _selectedObject.Key == "")
            {
                return true;
            }
            return false;
        }

        private void InitEvents()
        {
            SelectionEvents.eventSelectObject += UpdateSelectObjectPanel;
            SelectionEvents.eventSelectObject += UpdateAbilitesPanel;
        }

        private void ClearSelectionPanel()
        {
            SelectedObjectName.text = "";
            SelectedObjectIcon.sprite = EmptyImage;
        }

        private void UpdateSelectObjectPanel(UnitModel _selectedObject)
        {
            string ObjectName = null;
            Sprite ObjectIcon = null;

            if (!_selectedObject || _selectedObject.IsEmpty())
            {
                ClearSelectionPanel();
                return;
            }

            switch (_selectedObject.UnitType)
            {
                case UnitType.Character:
                    ObjectName = _selectedObject.BaseInformation.Name;
                    ObjectIcon = _selectedObject.BaseInformation.Icon;
                    break;
                case UnitType.Building:
                    ObjectName = _selectedObject.BaseInformation.Name;
                    ObjectIcon = _selectedObject.BaseInformation.Icon;
                    break;
                default:
                    break;
            }

            SelectedObjectName.text = ObjectName;
            SelectedObjectIcon.sprite = ObjectIcon;

        }
    }

}
