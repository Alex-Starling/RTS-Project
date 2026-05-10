using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RtsEngine.DataBase;
using RtsEngine.Buildings;
using System;

namespace RtsEngine
{
    [RequireComponent(typeof(BaseSelectHandler))]
    [RequireComponent(typeof(BaseSelectHandler))]
    [RequireComponent(typeof(AudioSource))]
    public class SpeechController : MonoBehaviour, ICanSpeak
    {
        public Action SpeakOnSelected { get; set; }
        public Action SpeakOnStartMove { get; set; }
        public bool DisableAuido { get; set; }

        private BaseUnit unit;
        private BaseBuilding building;
        private ICanMove componentCanMove;

        private BaseSelectHandler selectHandler;
        private AudioSource audioSource;

        private RTSObject rtsObject;

        private void Start()
        {
            InitComponenets();
            InitEvents();
            InitRtsObject();
        }

        private void InitEvents()
        {
            SpeakOnSelected += PlayAudioHello;
            SpeakOnStartMove += PlayAudioCommand;     
        }

        private void InitRtsObject()
        {
            if (unit)
                rtsObject = DataBaseManager.GetUnit(unit.Key);
            if (building)
                rtsObject = DataBaseManager.GetBuilding(building.Key);
        }

        private void InitComponenets()
        {
            selectHandler = GetComponent<BaseSelectHandler>();
            audioSource = GetComponent<AudioSource>();
            unit = GetComponent<BaseUnit>();
            building = GetComponent<BaseBuilding>();
            componentCanMove = GetComponent<ICanMove>();
        }

        private void PlayAudioHello()
        {
            if (DisableAuido)
                return;

            if (!rtsObject || rtsObject.OnSelectClips == null || (rtsObject.OnSelectClips.Length <= 0))
                return;

            int randomInt = UnityEngine.Random.Range(0, rtsObject.OnSelectClips.Length);
            audioSource.clip = rtsObject.OnSelectClips[randomInt];

            if (rtsObject.OnSelectClips[randomInt])
                audioSource.Play();
        }

        private void PlayAudioCommand()
        {
            if (DisableAuido || !rtsObject)
                return;

            int randomInt = UnityEngine.Random.Range(0, rtsObject.OnGetCommandClips.Length);
            audioSource.clip = rtsObject.OnGetCommandClips[randomInt];

            if (rtsObject.OnSelectClips[randomInt])
                audioSource.Play();

        }

        private void PlayAudioCantExecuteCommand()
        {
            if (DisableAuido || !rtsObject)
                return;

            int randomInt = UnityEngine.Random.Range(0, rtsObject.OnCantExecuteCommandClips.Length);
            audioSource.clip = rtsObject.OnCantExecuteCommandClips[randomInt];

            if (rtsObject.OnSelectClips[randomInt])
                audioSource.Play();
        }
    }
    public interface ICanSpeak
    {
        public Action SpeakOnSelected { get; set; }
        public Action SpeakOnStartMove { get; set; }
        public bool DisableAuido { get; set; }
    }
}
