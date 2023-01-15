using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RTSEngine.UI 
{
    public class MainMenuUI : MonoBehaviour
    {
        public void OnClickStartSoloBattleButton()
        {
            SceneManager.LoadScene(1);
        }
    }
}

