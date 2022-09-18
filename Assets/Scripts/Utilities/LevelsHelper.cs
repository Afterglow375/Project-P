using System.Collections.Generic;

namespace Utilities
{
    public static class LevelsHelper
    {
        // the furthest level the player has reached
        private static string _furthestLevel = Scenes.Level1;
        
        public static readonly string[] Levels = {
            Scenes.Level1, Scenes.Level2, Scenes.Level3, Scenes.Level4,
            Scenes.Level5, Scenes.Level6, Scenes.BonusLevel1, Scenes.BonusLevel2
        };

        public static readonly Dictionary<string, string> LevelOrderDict = new()
        {
            { Scenes.Level1, Scenes.Level2 },
            { Scenes.Level2, Scenes.Level3 },
            { Scenes.Level3, Scenes.Level4 },
            { Scenes.Level4, Scenes.Level5 },
            { Scenes.Level5, Scenes.Level6 },
            { Scenes.Level6, Scenes.BonusLevel1 },
            { Scenes.BonusLevel1, Scenes.BonusLevel2 }
        };

        public static string GetFurthestLevel()
        {
            return _furthestLevel;
        }
        
        public static void SetFurthestLevel(string levelName)
        {
            _furthestLevel = levelName;
        }
    }
}