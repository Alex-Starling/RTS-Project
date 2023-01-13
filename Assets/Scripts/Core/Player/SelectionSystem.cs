using RtsEngine.AI;
using RtsEngine.DataBase;
using RtsEngine.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.Universal;
using static UnityEngine.Rendering.DebugUI.Table;

namespace RtsEngine.SelectionSystem
{
    public class SelectionSystem : MonoBehaviour
    {
        public List<GroupAI> unitList = new List<GroupAI>();
        public List<GroupAI> Squads = new List<GroupAI>();

        [SerializeField]
        private ParticleSystem SelectPointFX;
        [SerializeField]
        private RectTransform SelectBoxVisual;
        [SerializeField]
        private Camera myCam;
        [SerializeField]
        private SelectionBox selectionBox;
        [SerializeField]
        private Collider[] selectionColliders;
        [SerializeField]
        private DecalProjector selectionProjector;
        [SerializeField]
        private LayerMask layerCursorClickMask;
        [SerializeField]
        private LayerMask layerSelectableMask;

        private bool mouseHold;
        private bool gizmo_enabled;

        private Vector3 startPointPos;
        private Vector3 dragPointPos;

        private Vector2 startMousePosition;
        private Vector2 endMousePosition;

        private RaycastHit hitCameraPoint;
        private Ray cameraRay;
        private bool MouseDownClicked;
        private bool SelectOnlyOneUnit;

        private void Start()
        {
            InitEvents();
            DrawSelectionProjector();

            gizmo_enabled = true;
            selectionBox = new SelectionBox();
            selectionProjector.enabled = false;
            startMousePosition = Vector2.zero;
            endMousePosition = Vector2.zero;
            SelectPointFX.transform.position = Vector3.zero;
        }
        private void Update()
        {
            if (mouseHold)
            {
                LeftMouseHold();
            }
        }
        private void InitEvents()
        {
            InputEvents.eventClickPass += CheckClick;
            InputEvents.eventRightClick += HandlerOnMouseRightClick;
        }
        private void CheckClick(bool MouseUp)
        {
            if (MouseUp)
            {
                mouseHold = false;
                LeftMouseUp();
            }
            else
            {
                mouseHold = true;
                LeftMouseDown();
            }
        }
        private void HandlerOnMouseRightClick()
        {
            if (EventSystem.current.IsPointerOverGameObject()) { return; }
            cameraRay = myCam.ScreenPointToRay(RTSInput.Input.Player.MousePosition.ReadValue<Vector2>());
            Physics.Raycast(cameraRay, out hitCameraPoint, 100f, layerCursorClickMask);

            MouseDownClicked = false;

            if (Squads.Count > 0)
            {
                SelectPointFX.transform.position = hitCameraPoint.point;
                SelectPointFX.Play(true);

                MoveCommand(hitCameraPoint.point);
            }
        }

        private void MoveCommand(Vector3 _pos)
        {
            if (Squads.Count <= 0)
                return;

            BuildFormation(_pos);
             Squads[0].Speak?.SpeakOnStartMove?.Invoke();
            //    Squads[index].Move?.MoveToPosition(newPosition);
            //     Squads[i].UnitsInSquad[totalIndex].Move?.MoveToPosition(newPosition);
        }

        void BuildFormation(Vector3 targetPosition)
        {
            Vector3 squadPosition = targetPosition;
            int squadInRow = 0;

            // Iterate through all squads
            for (int i = 0; i < Squads.Count(); i++)
            {
                // Iterate through all units in the current squad
                for (int j = 0; j < Squads[i].UnitsInSquad.Length; j++)
                {
                    // Calculate the position of the current unit in the squad formation
                    Vector3 unitPosition = squadPosition + new Vector3((j % 4) * Squads[i].UnitsInSquad[j].Size, 0, (j / 4) * Squads[i].UnitsInSquad[j].Size);

                    // Set the position of the current unit
                    Squads[i].UnitsInSquad[j].Move?.MoveToPosition(unitPosition);
                }
                // Update the squad position for next squad
                if (squadInRow < 3)
                {
                    squadPosition += new Vector3(4 * Squads[i].UnitsInSquad[0].Size, 0, 0);
                    squadInRow++;
                }
                else
                {
                    squadPosition = targetPosition + new Vector3(0, 0, 4 * Squads[i].UnitsInSquad[0].Size);
                    squadInRow = 0;
                }
            }
        }

        private void LeftMouseDown()
        {
            if (EventSystem.current.IsPointerOverGameObject()) { return; }
            DeSelectObjects();
            startMousePosition = RTSInput.Input.Player.MousePosition.ReadValue<Vector2>();

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(startMousePosition);

            if (Physics.Raycast(ray, out hit, 100f, layerSelectableMask))
            {
                var comp = hit.transform.GetComponent<BaseUnit>();

                if (comp != null)
                {
                    if (comp.Select.GroupedUnit)
                    {
                        if (!Squads.Contains(comp.Select.GroupParent))
                        {
                            Squads.Add(comp.Select.GroupParent);
                        }
                    }

                    SelectObject();

                    SelectOnlyOneUnit = true;
                    return;
                }
            }

            SelectOnlyOneUnit = false;
            MouseDownClicked = false;
        }
        private void LeftMouseHold()
        {
            if (EventSystem.current.IsPointerOverGameObject() || SelectOnlyOneUnit)
                return;

            endMousePosition = RTSInput.Input.Player.MousePosition.ReadValue<Vector2>();
            cameraRay = myCam.ScreenPointToRay(RTSInput.Input.Player.MousePosition.ReadValue<Vector2>());

            Physics.Raycast(cameraRay, out hitCameraPoint, 100f, layerCursorClickMask);

            if (!MouseDownClicked)
            {
                startPointPos = hitCameraPoint.point;
                selectionBox.baseMin = startPointPos;
                selectionProjector.enabled = true;
                MouseDownClicked = true;
            }

            dragPointPos = hitCameraPoint.point;
            selectionBox.baseMax = dragPointPos;

            DrawSelectionProjector();
        }

        private void DrawSelectionProjector()
        {
            Draw2DSelectionBox();
        }

        private void Draw2DSelectionBox()
        {
            if (!SelectBoxVisual)
            {
                return;
            }
            Vector2 boxStart = startMousePosition;
            Vector2 boxEnd = endMousePosition;

            Vector2 boxCenter = (boxStart + boxEnd) / 2;
            SelectBoxVisual.position = boxCenter;

            Vector2 boxSize = new Vector2(Mathf.Abs(boxStart.x - boxEnd.x), Mathf.Abs(boxStart.y - boxEnd.y));
            SelectBoxVisual.sizeDelta = boxSize;
        }

        private void DeSelectObjects()
        {
            foreach (var unit in Squads)
            {
                BaseSelectHandler unitSelectComp = null;
                unitSelectComp = unit.GetComponent<BaseSelectHandler>();
                if (unitSelectComp != null)
                {
                    unitSelectComp.DeSelect();
                }

            }
            Squads.Clear();
        }

        private void LeftMouseUp()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                startPointPos = Vector3.zero;
                dragPointPos = Vector3.zero;

                startMousePosition = Vector2.zero;
                endMousePosition = Vector2.zero;

                selectionBox.Reset();
                DrawSelectionProjector();

                selectionProjector.enabled = false;
                return;
            }

            if (!MouseDownClicked || SelectOnlyOneUnit)
                return;

            startMousePosition = Vector2.zero;
            endMousePosition = Vector2.zero;

            DeSelectObjects();

            selectionColliders = Physics.OverlapBox(selectionBox.Center, selectionBox.Size / 2, Quaternion.identity, layerSelectableMask);

            foreach (var overlappedObject in selectionColliders)
            {
                var comp = overlappedObject.GetComponent<BaseUnit>();
                if (comp && comp.Select.CanSelect)
                {
                    if (comp.Select.GroupedUnit)
                    {
                        if (!Squads.Contains(comp.Select.GroupParent))
                        {
                            Squads.Add(comp.Select.GroupParent);
                            comp.Select.ParentGroupAI.OnAddedToSelectList();
                        }
                    }
                }
            }

            SelectObject();

            startPointPos = Vector3.zero;
            dragPointPos = Vector3.zero;

            selectionBox.Reset();
            DrawSelectionProjector();
            selectionProjector.enabled = false;
        }

        private void SelectObject()
        {
            if (Squads.Count > 0)
            {
                if (Squads[0].Speak != null)
                    Squads[0].Speak.SpeakOnSelected.Invoke();

                var selectComp = Squads[0].Select;

                string selectedObjectKey = selectComp.GetInfoAboutSelectedObject().Item1;
                var selectedObjectType = selectComp.GetInfoAboutSelectedObject().Item2;

                selectComp.OnSelect();

                if (selectedObjectKey == null)
                {
                    SelectionEvents.CallOnSelectObject(null);
                    return;
                }

                DbEntityUnit selectedUnitEntity;
                DBEntityBuilding selectedBuildingEntity;

                switch (selectedObjectType)
                {
                    case RTSObjectType.Unit:
                        selectedUnitEntity = DataBaseManager.GetUnit(selectedObjectKey);
                        SelectionEvents.CallOnSelectObject(selectedUnitEntity);
                        break;
                    case RTSObjectType.Building:
                        selectedBuildingEntity = DataBaseManager.GetBuilding(selectedObjectKey);
                        SelectionEvents.CallOnSelectObject(selectedBuildingEntity);
                        break;
                    default:
                        break;
                }
                return;
            }

            SelectionEvents.CallOnSelectObject(null);
        }

        private void OnDrawGizmos()
        {
            if (gizmo_enabled)
            {
                Gizmos.DrawWireCube(selectionBox.Center, selectionBox.Size);
            }
        }

    }

    [System.Serializable]
    public class SelectionBox
    {
        public Vector3 baseMin, baseMax;
        public Vector3 Center
        {
            get
            {
                Vector3 center = baseMin + (baseMax - baseMin) * 0.5f;
                // center.y = (baseMax - baseMin).magnitude * 0.5f;
                return center;
            }
            private set { }

        }
        public Vector3 Size
        {
            get
            {
                return new Vector3(
                    Mathf.Abs(baseMax.x - baseMin.x),
                    (baseMax - baseMin).magnitude,
                    Mathf.Abs(baseMax.z - baseMin.z));
            }
            private set { }
        }
        public Vector3 Extens
        {
            get
            {
                return Size * 0.5f;
            }
        }
        public void Reset()
        {
            baseMax = Vector3.zero;
            baseMin = Vector3.zero;
            Size = Vector3.zero;
            Center = Vector3.zero;
        }
    }
}
