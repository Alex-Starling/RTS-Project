using RTSEngine.Abilities;
using RTSEngine.Events;
using UnityEngine;
using UnityEngine.UI;

namespace RTSEngine.UI
{
    public class InGameUI : MonoBehaviour
    {
        [Header("Abilities")]
        public AbilityButton AbilityButtonPrefab;
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

        private void UpdateAbilitesPanel(UnitModel selectedObject)
        {
            ClearAbilitesPanel();

            if (!selectedObject || selectedObject.IsEmpty())
                ResetSelectedObjectPanel();
            else
                PopulateAbilityButtons(selectedObject.Abilites);
        }

        private void PopulateAbilityButtons(BaseAbility[] abilities)
        {
            if (abilities == null)
                return;

            foreach (var ability in abilities)
            {
                var button = GameObject.Instantiate(AbilityButtonPrefab, AbilitiesPanel.transform);
                button.Icon.sprite = ability.BaseInformation.Icon;
                button.Text.text = "";
            }
        }

        private void ClearAbilitesPanel()
        {
            foreach (Transform item in AbilitiesPanel.transform)
            {
                GameObject.Destroy(item.gameObject);
            }
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

        private void ResetSelectedObjectPanel()
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
