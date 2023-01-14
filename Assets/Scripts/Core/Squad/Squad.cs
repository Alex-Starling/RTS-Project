using RtsEngine.DataBase;
using RtsEngine.Units;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RtsEngine.AI
{
    public class Squad : BaseUnit
    {
        public BaseUnit[] UnitsInSquad { get; set; }
        public Action EventOnStartMove { get; set; }
        public override BaseBehaviour CurrentBehaviour { get; protected set; }
        public bool CaneMove { get; set; }

        [HideInInspector] public int maxObjectsPerRow = 2;
        [HideInInspector] public int UnitsCount;

        private DbEntityUnit unitEntity;

        protected override void Start()
        {
            base.Start();
            InitUnits();
        }

        public void SwitchUnitsBehaviour()
        {

        }

        public void InitUnits()
        {
            if (Key == "" || Key == null)
                return;
            var entity = DataBaseManager.GetUnit(Key);

            if (!entity)
                return;

            unitEntity = entity;

            Size = unitEntity.UnitSize;
            UnitsCount = unitEntity.UnitsCount;
            maxObjectsPerRow = unitEntity.UnitsInLine;
            UnitsInSquad = new BaseUnit[UnitsCount];

            PopulateSquad();
        }

        private void PopulateSquad()
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
                selectComp.ParentGroupAI = GetComponent<SquadSelectHandler>();
                UnitsInSquad[i] = go.GetComponent<BaseUnit>();

                ICanSpeak speacComp = go.GetComponent<ICanSpeak>();
                if (speacComp != null)
                    speacComp.DisableAuido = true;

                n++;
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
