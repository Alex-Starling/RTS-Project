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
            int randomIndx = UnityEngine.Random.Range(0, unitModel.Model.Length);
            UnitView uv;

            if (unitModel.UnitType == UnitType.Character)
                uv = GameObject.Instantiate<UnitView>(characterTemplate, transform.position, Quaternion.identity);
            else
                uv = GameObject.Instantiate<UnitView>(buildingTemplate, transform.position, Quaternion.identity);

            var go = GameObject.Instantiate(unitModel.Model[randomIndx], uv.model.transform);

            uv.Key = key;

            uv.Move.Agent.radius = unitModel.AgentRadius;
            go.layer = 0;
            go.transform.localPosition = Vector3.zero;
            go.transform.localRotation = Quaternion.identity;
        }

    }

}