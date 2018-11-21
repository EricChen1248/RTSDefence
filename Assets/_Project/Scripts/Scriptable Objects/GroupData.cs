using UnityEngine;
using Scripts.Entity_Components;
using Scripts.Entity_Components.Ais;

namespace Scripts.Scriptable_Objects
{
	public class GroupData : ScriptableObject
	{
		#region Used By GroupComponent

		public void CompileGroupProperty(out GroupComponent.GroupDataProperty p){
			p = new GroupComponent.GroupDataProperty();
		}

		#endregion

		#region Used By Group AI

		// Set <= 0 and no temp target will be cleared.
		public float TimeToClearTempTarget;

		public void CompileAIProperty(out GroupAiBase.GroupAIProperty p){
			p = new GroupAiBase.GroupAIProperty();
			if(TimeToClearTempTarget <= 0){
				// ?? = false;
				// ?? = null?
			}else{
				// ?? = true;
				// ?? = TimeToClearTempTarget;
			}
		}

		#endregion
	}
}
