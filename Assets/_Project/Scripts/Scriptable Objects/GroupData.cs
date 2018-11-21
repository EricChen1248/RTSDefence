using UnityEngine;

namespace Scripts.Scriptable_Objects
{
    public class GroupData : ScriptableObject
    {
    	#region Used By GroupComponent

    	#endregion

    	#region Used By Group AI

    	// Set <= 0 and no temp target will be cleared.
        public float TimeToClearTempTarget;

        #endregion
    }
}
