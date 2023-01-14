using RtsEngine.Abilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RtsEngine.DataBase
{
    [CreateAssetMenu(fileName = "DbEntityUnit", menuName = "RTS_ENGINE/DataBase/DbEntityUnit")]
    public class DbEntityUnit : RTSObject
    {
        public int UnitsInLine;
        public int UnitsCount;
        public float UnitSize;
    }

}
