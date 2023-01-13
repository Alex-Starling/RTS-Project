using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RtsEngine.UI 
{
    public class MainMenuUI : MonoBehaviour
    {
        public void OnClickStartSoloBattleButton()
        {
            SceneManager.LoadScene(1);
        }
    }
}

