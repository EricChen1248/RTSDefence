using UnityEngine;

namespace Scripts.GUI{
	public class SpawnButton : MonoBehaviour {
		public GameObject SpawnObject;
		public int PosXLowerBound = -10;
		public int PosXUpperBound = 10;
		public int PosZLowerBound = -10;
		public int PosZUpperBound = 10;
		public static System.Random rnd = new System.Random();

		public void ClickEvent()
		{
			//...
			Vector3 v = new Vector3(rnd.Next(PosXLowerBound, PosXUpperBound), (float)0.75, rnd.Next(PosZLowerBound, PosZUpperBound));
			Instantiate(SpawnObject, v, Quaternion.identity);
		}
	}
}