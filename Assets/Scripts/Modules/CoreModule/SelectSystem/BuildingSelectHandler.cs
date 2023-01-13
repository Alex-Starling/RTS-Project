
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RtsEngine.Buildings
{
    public class BuildingSelectHandler : BaseSelectHandler
    {


        public override (string, RTSObjectType) GetInfoAboutSelectedObject()
        {

            if (CanSelect)
            {
                if (SelectVisualObject)
                {
                    SelectVisualObject.SetActive(true);
                    EventOnUnitSelected?.Invoke();
                }

                return (baseBuilding.Key, RTSObjectType.Building);
            }
            else
            {
                return (null, RTSObjectType.Building);
            }

        }
        protected override void Init()
        {
            baseBuilding = GetComponent<BaseBuilding>();
        }

    }
}

