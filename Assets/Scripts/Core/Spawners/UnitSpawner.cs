using RTSEngine.DataBase;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.EventSystems.EventTrigger;

namespace RTSEngine
{
    public class UnitSpawner : MonoBehaviour
    {
        [SerializeField] private string key;
        [SerializeField] private UnitView characterTemplate;
        [SerializeField] private UnitView buildingTemplate;

        private UnitModel unitModel;

        private void Start()
        {
            Init();
        }

        public void Init()
        {
            if (key == "" || key == null)
                return;

            var entity = DataBaseManager.GetUnit(key);

            if (!entity)
                return;

            unitModel = entity;

            Spawn();
        }

        private void Spawn()
        {
            UnitView go;
            int randomIndx = UnityEngine.Random.Range(0, unitModel.Model.Length);

            go = GameObject.Instantiate<UnitView>(unitModel.Model[randomIndx], transform.position, Quaternion.identity);

        }

    }

}