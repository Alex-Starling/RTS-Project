using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTSEngine
{
    public interface ICanSelected
    {
        public UnitView Unit { get; set; }
        public void OnSelect();
        public void DeSelect();
        public (string, UnitType) GetInfoAboutSelectedObject();
    }

}
