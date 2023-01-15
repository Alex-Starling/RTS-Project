using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTSEngine.DataBase;
using System;

namespace RTSEngine
{
    [RequireComponent(typeof(AudioSource))]
    public class SpeechController : MonoBehaviour, ICanSpeak
    {
        public Action SpeakOnSelected { get; set; }
        public Action SpeakOnStartMove { get; set; }
        public bool DisableAuido { get; set; }

        private UnitView unit;
        private ICanMove componentCanMove;

        private AudioSource audioSource;

        private UnitModel rtsObject;

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
        }

        private void InitComponenets()
        {
            audioSource = GetComponent<AudioSource>();
            unit = GetComponent<UnitView>();
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
            if (DisableAuido || !rtsObject || (rtsObject.OnGetCommandClips.Length == 0))
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
