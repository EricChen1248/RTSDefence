using System.Collections;
using Scripts.Controllers;
using UnityEngine;

namespace Scripts.Resources
{
    public class ResourceHolderComponent : MonoBehaviour
    {
        public GameObject ModelHolder;

        public ResourceTypes HeldResource;

        public int HeldCount;

        public void ChangeResources(ResourceTypes type)
        {
            HeldResource = type;
            HeldCount = 0;
            foreach (Transform child in ModelHolder.transform)
            {
                Destroy(child.gameObject);
            }

            var model = Instantiate(ResourceController.ModelDictionary[type]);
            model.transform.parent = ModelHolder.transform;
            model.transform.localPosition = Vector3.zero;
        }

        public void MoveTo(Vector3 start, Vector3 end, float seconds)
        {
            StartCoroutine(MoveLerp(start, end, seconds));
        }

        private IEnumerator MoveLerp(Vector3 start, Vector3 end, float seconds)
        {
            seconds *= 100;
            for (var i = 0; i < seconds; i++)
            {
                transform.position = Vector3.Lerp(start, end, i / seconds);
                yield return new WaitForSeconds(0.01f);
            }

        }
    
    }
}
