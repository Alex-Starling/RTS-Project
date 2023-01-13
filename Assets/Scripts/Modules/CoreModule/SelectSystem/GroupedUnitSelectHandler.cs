using RtsEngine.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RtsEngine.Units
{
    [RequireComponent(typeof(GroupAI))]
    public class GroupedUnitSelectHandler : BaseSelectHandler
    {
        private GroupAI groupAI;

        private void Awake()
        {
            groupAI = GetComponent<GroupAI>();
        }

        public override (string, RTSObjectType) GetInfoAboutSelectedObject()
        {
            if (CanSelect)
                return (baseUnit.Key, RTSObjectType.Unit);
            else
                return (null, RTSObjectType.Unit);
        }

        public override void OnSelect()
        {
            if (!CanSelect)
                return;

            this.EventOnUnitSelected?.Invoke();
          
            foreach (var _unit in groupAI.UnitsInSquad)
            {
                var selectComp = _unit.GetComponent<BaseSelectHandler>();
                selectComp.SwitchSelectVisual(true);
            }
        }

        public override void DeSelect()
        {
            foreach (var _unit in groupAI.UnitsInSquad)
                _unit.GetComponent<BaseSelectHandler>().SwitchSelectVisual(false);
        }

        protected override void Init()
        {
            baseUnit = GetComponentInParent<GroupAI>();
        }

        public override void OnAddedToSelectList()
        {
            if (CanSelect)
            {
                foreach (var _unit in groupAI.UnitsInSquad)
                {
                    _unit.GetComponent<BaseSelectHandler>().SwitchSelectVisual(true);
                }
            }
        }
    }
}
