using Scripts.Controllers;
using Scripts.Entity_Components.Ais;
using UnityEngine;
using Random = System.Random;

namespace Scripts.DebugScripts
{
    public class SpawnButton : MonoBehaviour
    {
        public static Random Rnd = new Random();
        public int PosXLowerBound = -10;
        public int PosXUpperBound = 10;
        public int PosZLowerBound = -10;
        public int PosZUpperBound = 10;
        public GameObject SpawnObject;

        public void ClickEvent()
        {
            //...
            var v = new Vector3(Rnd.Next(PosXLowerBound, PosXUpperBound), (float) 0.75,
                Rnd.Next(PosZLowerBound, PosZUpperBound));
            var go = Instantiate(SpawnObject, v, Quaternion.identity);

            go.GetComponent<SimpleAi>().Target = CoreController.Instance.CoreGameObject;
        }
    }
}