using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RtsEngine.DataBase
{
    public static class DataBaseManager
    {
        private static DbEntityUnit[] dbEntityUnits;
        private static DBEntityBuilding[] dBEntityBuildings;

        private static bool Init()
        {
            dbEntityUnits = Resources.LoadAll<DbEntityUnit>("ScriptableObjects/Units/");
            dBEntityBuildings = Resources.LoadAll<DBEntityBuilding>("ScriptableObjects/Buildings/");
            return true;
        }
        public static DbEntityUnit[] GetAllUnits()
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
        public static DbEntityUnit GetUnit(string _key)
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
        public static DBEntityBuilding GetBuilding(string _key)
        {
            if (dBEntityBuildings == null)
            {
                Init();
            }
            if (dBEntityBuildings != null)
            {
                foreach (var item in dBEntityBuildings)
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
