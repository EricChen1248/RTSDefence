using UnityEngine;
using Scripts.Entity_Components;
using Scripts.Entity_Components.Ais;

namespace Scripts.Scriptable_Objects
{
	[CreateAssetMenu]
	public class GroupData : ScriptableObject
	{
		#region Used By GroupComponent

		public String Characteristics;

		public void CompileGroupProperty(out GroupComponent.GroupDataProperty p){
			p = new GroupComponent.GroupDataProperty();
			p.Characteristics = Characteristics;
		}

		#endregion

		#region Used By Group AI

		// Set <= 0 and no temp target will be cleared.
		public float TimeToClearTempTarget;

		public void CompileAIProperty(out GroupAiBase.GroupAIProperty p){
			p = new GroupAiBase.GroupAIProperty();
			if(TimeToClearTempTarget <= 0){
				p.CheckTempTarget = false;
				p.TimeToClearTempTarget = 0;
			}else{
				p.CheckTempTarget = true;
				p.TimeToClearTempTarget = TimeToClearTempTarget;
			}
		}

		#endregion
	}
}
