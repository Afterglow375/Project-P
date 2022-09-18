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
        private readonly string[] _levels = {
            Scenes.Level1, Scenes.Level2, Scenes.Level3, Scenes.Level4,
            Scenes.Level5, Scenes.Level6, Scenes.BonusLevel1, Scenes.BonusLevel2
        };

        private void Start()
        {
            Transform grid = transform.Find("LevelContainer/Grid");
            for (int i = 0; i < _levels.Length; i++)
            {
                // turn on level buttons for all the levels the player has reached
                grid.GetChild(i).GetComponent<Button>().interactable = true;
                if (LevelsHelper.GetFurthestLevel() == _levels[i])
                {
                    break;
                }
                
            }
        }

        public void LoadLevel(string levelName)
        {
            GameManager.Instance.ChangeScene(levelName);
        }
    }
}
