using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTSEngine.DataBase
{
    public static class DataBaseManager
    {
        private static UnitModel[] dbEntityUnits;

        private static bool Init()
        {
            dbEntityUnits = Resources.LoadAll<UnitModel>("ScriptableObjects/");
            return true;
        }
        public static UnitModel[] GetAllUnits()
        {
            if (dbEntityUnits != null)
            {
                return dbEntityUnits;
            }
            else
            {
                if (Init())
                {
                    return dbEntityUnits;
                }
            }
            return null;
        }
        public static UnitModel GetUnit(string _key)
        {
            if (dbEntityUnits == null)
            {
                Init();
            }

            if (dbEntityUnits != null)
            {
                foreach (var item in dbEntityUnits)
                {
                    if (_key == item.Key)
                    {
                        return item;
                    }
                }
            }
            return null;
        }
    }
}
