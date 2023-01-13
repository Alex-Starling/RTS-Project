using RtsEngine.DataBase;
using RtsEngine.Units;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RtsEngine.AI
{
    public class GroupAI : BaseUnit, ICanMove
    {
        public int maxObjectsPerRow = 2;
        public override BaseBehaviour CurrentBehaviour { get; protected set; }

        public int UnitsCount;
        public BaseUnit[] UnitsInSquad { get; set; }
        public Action EventOnStartMove { get; set; }
        public bool CaneMove { get; set; }

        private DbEntityUnit unitEntity;

        protected override void Start()
        {
            base.Start();
            InitUnits();
        }
     
        public void MoveToPosition(Vector3 _position)
        {
            Debug.Log("111");
            int n = 0;
            bool CalledEventOnStartMove = false;
            int rows = Mathf.CeilToInt((float)UnitsInSquad.Length / maxObjectsPerRow);

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < maxObjectsPerRow; j++)
                {
                    int index = i * maxObjectsPerRow + j;
                    if (index >= UnitsInSquad.Length)
                    {
                        break;
                    }

                    float posX = j * 1.5f - (maxObjectsPerRow - 1) * 0.75f;
                    float posZ = i * 1.5f - (rows - 1) * 0.75f;
                    Vector3 newPosition = _position + new Vector3(posX, 0, posZ);

                    UnitsInSquad[index].Move?.MoveToPosition(newPosition);

                    if (!CalledEventOnStartMove)
                    {
                        CalledEventOnStartMove = true;
                        EventOnStartMove?.Invoke();
                    }
                }
            }
        }

        public void SwitchUnitsBehaviour()
        {

        }

        public void InitUnits()
        {
            UnitsInSquad = new BaseUnit[UnitsCount];

            if (Key == "" || Key == null)
                return;

            if (unitEntity = DataBaseManager.GetUnit(Key))
            {
                int n = 0;
                for (int i = 0; i < UnitsCount; i++)
                {
                    int randomIndx = UnityEngine.Random.Range(0, unitEntity.Model.Length);

                    var go = GameObject.Instantiate(unitEntity.Model[randomIndx], transform);
                    go.transform.position += new Vector3(0, 0, 0);
                    go.GetComponent<HumanoidUnit>().Key = Key;

                    var selectComp = go.GetComponent<BaseSelectHandler>();
                    selectComp.baseUnit = this;
                    selectComp.GroupedUnit = true;
                    selectComp.GroupParent = this;
                    selectComp.ParentGroupAI = GetComponent<GroupedUnitSelectHandler>();
                    UnitsInSquad[i] = go.GetComponent<BaseUnit>();

                    ICanSpeak speacComp = go.GetComponent<ICanSpeak>();
                    if (speacComp != null)
                        speacComp.DisableAuido = true;

                    n++;
                }
            }

        }

        public void ClearUnits()
        {
            foreach (var item in UnitsInSquad)
            {
                GameObject.Destroy(item.gameObject);
            }
        }
    }
}
