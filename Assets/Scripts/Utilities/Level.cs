using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Utilities
{
    public class Level
    {
        public string name;
        public LevelStatus status;
        public bool firstLvl;
        public bool lastLvl;
        public int requirements;
        public List<Level> nextLevels;
        public string[] nextLevelNames;

        public Level(string name, LevelStatus status, bool firstLvl, bool lastLvl, GameObject[] nextLevelObjects)
        {
            this.name = name;
            this.firstLvl = firstLvl;
            this.lastLvl = lastLvl;
            this.status = status;
            nextLevels = new(nextLevelObjects.Length);
            nextLevelNames = new string[nextLevelObjects.Length];
            for (int i = 0; i < nextLevelObjects.Length; i++)
            {
                nextLevelNames[i] = nextLevelObjects[i].GetComponentInChildren<TextMeshProUGUI>().text;
            }
        }

        public void AddNextLevel(Level nextLevel)
        {
            nextLevels.Add(nextLevel);
            nextLevel.requirements++;
        }

        public void LevelCompleted()
        {
            status = LevelStatus.Complete;
            foreach (var nextLevel in nextLevels)
            {
                if (--nextLevel.requirements == 0)
                {
                    nextLevel.status = LevelStatus.Accessible;
                }
            }
        }
    }

    public enum LevelStatus
    {
        Complete,
        Accessible,
        Inaccessible
    }
}