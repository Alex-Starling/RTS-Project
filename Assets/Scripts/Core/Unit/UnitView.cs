using RTSEngine.Units;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTSEngine
{
    public class UnitView : MonoBehaviour
    {
        public string Key;
        [HideInInspector]
        public float Size;
        public BaseBehaviour CurrentBehaviour { get; protected set; }
        public BaseBehaviour StartBehaviour;

        [field: SerializeField] public ICanSpeak Speak { get; protected set; }
        [field: SerializeField] public ICanMove Move { get; protected set; }
        [field: SerializeField] public ICanSelected Select { get; protected set; }

        protected virtual void Start()
        {
            InitComponents();
            Init(StartBehaviour);
        }

        private void InitComponents()
        {
            Speak = GetComponent<ICanSpeak>();
            Move = GetComponent<ICanMove>();
            Select = GetComponent<ICanSelected>();
        }

        private void Init(BaseBehaviour _behaviour)
        {
            CurrentBehaviour = _behaviour;
            CurrentBehaviour.actor = gameObject;
            if (CurrentBehaviour)
            {
                CurrentBehaviour.Start();
            }
        }
        private void Update()
        {
            if (CurrentBehaviour && !CurrentBehaviour.IsFinished)
            {
                CurrentBehaviour.Run();
            }
            else
            {

            }
        }
        private void SwitchBehavior()
        {

        }
        private void SetBehaviour(BaseBehaviour _behaviour)
        {
            CurrentBehaviour.Stop();
            CurrentBehaviour = _behaviour;
            CurrentBehaviour.actor = gameObject;
            CurrentBehaviour.Start();
        }
    }

}