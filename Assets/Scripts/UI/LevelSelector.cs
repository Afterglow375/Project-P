using System;
using System.Collections.Generic;
using Managers;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

namespace UI
{
    public class LevelSelector : MonoBehaviour
    {
        private void Start()
        {
            Transform grid = transform.Find("LevelContainer/Grid");
            for (int i = 0; i < LevelsHelper.Levels.Length; i++)
            {
                // turn on level buttons for all the levels the player has reached
                grid.GetChild(i).GetComponent<Button>().interactable = true;
                if (LevelsHelper.GetFurthestLevel() == LevelsHelper.Levels[i])
                {
                    break;
                }
                
            }
        }

        public void LoadLevel(string levelName)
        {
            SceneLoader.Instance.LoadScene(levelName);
        }
    }
}
