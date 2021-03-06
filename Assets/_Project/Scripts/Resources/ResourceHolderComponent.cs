﻿using System.Collections;
using Scripts.Controllers;
using UnityEngine;

namespace Scripts.Resources
{
    public class ResourceHolderComponent : MonoBehaviour
    {
        public int HeldCount;

        public ResourceTypes HeldResource;
        public GameObject ModelHolder;

        public void ChangeResources(ResourceTypes type, int count)
        {
            HeldResource = type;
            HeldCount = count;
            foreach (Transform child in ModelHolder.transform) Destroy(child.gameObject);

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