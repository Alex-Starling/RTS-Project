using RTSEngine.DataBase;
using RTSEngine.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.Universal;
using static UnityEngine.Rendering.DebugUI.Table;

namespace RTSEngine.SelectionSystem
{
    public class SelectionController : MonoBehaviour
    {
        public List<ICanSelected> unitList = new List<ICanSelected>();
        public List<ICanSelected> Units = new List<ICanSelected>();

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

        public float spacing = 2; // distance between units in formation

        private void Start()
        {
            InitEvents();
            InitGizmo();
            InitVisual();
        }

        private void InitGizmo()
        {
            gizmo_enabled = true;
        }

        private void InitVisual()
        {
            selectionBox = new SelectionBox();
            selectionProjector.enabled = false;
            startMousePosition = Vector2.zero;
            endMousePosition = Vector2.zero;
            SelectPointFX.transform.position = Vector3.zero;
            DrawSelectionProjector();
        }

        private void Update()
        {
            if (mouseHold)
                LeftMouseHold();
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

            if (Units.Count <= 0)
                return;


            SelectPointFX.transform.position = hitCameraPoint.point;
            SelectPointFX.Play(true);

            MoveCommand(hitCameraPoint.point);
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
                var comp = overlappedObject.GetComponent<ICanSelected>();

                if (comp != null)
                {
                    Units.Add(comp);
                    comp.OnSelect();
                }

            }

            SelectObject();

            startPointPos = Vector3.zero;
            dragPointPos = Vector3.zero;
            selectionBox.Reset();

            DrawSelectionProjector();

            selectionProjector.enabled = false;
        }
        bool[] positionsTaken;
        void BuildFormation(Vector3 targetPosition)
        {
            // Calculate the number of rows and columns
            int rows = (int)Mathf.Ceil(Mathf.Sqrt(Units.Count()));
            int columns = (int)Mathf.Ceil(Units.Count() / (float)rows);
            for (int i = 0; i < Units.Count(); i++)
            {
                int row = i / columns;
                int col = i % columns;
                Units[i].Unit.Move?.MoveToPosition(CalcPosition(targetPosition, col, columns, i, row, rows));
            }
        }

        private Vector3 CalcPosition(Vector3 targetPosition, int col, int columns, int i, int row, int rows)
        {
            return targetPosition + new Vector3(((col - (columns - 1) / 2.0f) * spacing) + (Units[i].Unit.Size / 2), 0, ((row - (rows - 1) / 2.0f) * spacing) + (Units[i].Unit.Size / 2));
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
                var comp = hit.transform.GetComponent<ICanSelected>();

                if (comp != null)
                {
                    Units.Add(comp);
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
        private void MoveCommand(Vector3 _pos)
        {
            if (Units.Count <= 0)
                return;

            BuildFormation(_pos);
            Units[0].Unit.Speak?.SpeakOnStartMove?.Invoke();
        }

        private void DrawSelectionProjector()
        {
            Draw2DSelectionBox();
        }

        private void Draw2DSelectionBox()
        {
            if (!SelectBoxVisual)
                return;

            Vector2 boxStart = startMousePosition;
            Vector2 boxEnd = endMousePosition;

            Vector2 boxCenter = (boxStart + boxEnd) / 2;
            SelectBoxVisual.position = boxCenter;

            Vector2 boxSize = new Vector2(Mathf.Abs(boxStart.x - boxEnd.x), Mathf.Abs(boxStart.y - boxEnd.y));
            SelectBoxVisual.sizeDelta = boxSize;
        }

        private void DeSelectObjects()
        {
            foreach (var unit in Units)
            {
                ICanSelected unitSelectComp = null;
                unitSelectComp = unit;
                unitSelectComp?.DeSelect();
            }

            Units.Clear();
        }


        private void SelectObject()
        {
            if (Units.Count <= 0)
            {
                SelectionEvents.CallOnSelectObject(null);
                return;
            }

            Units[0].Unit?.Speak?.SpeakOnSelected?.Invoke();

            string selectedObjectKey = Units[0].GetInfoAboutSelectedObject().Item1;
            var selectedObjectType = Units[0].GetInfoAboutSelectedObject().Item2;

            Units[0].OnSelect();

            if (selectedObjectKey == null)
            {
                SelectionEvents.CallOnSelectObject(null);
                return;
            }

            UnitModel selectedUnitEntity;

            selectedUnitEntity = DataBaseManager.GetUnit(selectedObjectKey);
            SelectionEvents.CallOnSelectObject(selectedUnitEntity);
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
