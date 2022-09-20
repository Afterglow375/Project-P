using UnityEngine;

namespace Managers
{
    /// <summary>
    /// Creates any persistent singletons at runtime, before anything else happens.
    /// </summary>
    public static class BootStrapper
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Execute()
        {
            var obj = new GameObject("BootStrapper");
            obj.AddComponent<ManagerBootStrapper>();
            ManagerBootStrapper.Execute();
        }
    }
}