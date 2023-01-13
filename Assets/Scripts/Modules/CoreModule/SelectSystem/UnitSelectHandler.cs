using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RtsEngine.Events;

namespace RtsEngine.Units
{
    [RequireComponent(typeof(BaseUnit))]
    public class UnitSelectHandler : BaseSelectHandler
    {


        public override (string, RTSObjectType) GetInfoAboutSelectedObject()
        {
            if (CanSelect)
            {
                SwitchSelectVisual(true);
                return (baseUnit.Key, RTSObjectType.Unit);
            }
            else
            {
                return (null, RTSObjectType.Unit);
            }

        }

        protected override void Init()
        {
            if (!baseUnit)
            {
                baseUnit = GetComponent<BaseUnit>();
            }

        }
        public override void OnAddedToSelectList()
        {
            if (CanSelect)
            {
                SwitchSelectVisual(true);
            }

        }
    }
}

