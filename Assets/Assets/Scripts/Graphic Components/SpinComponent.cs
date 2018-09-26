using UnityEngine;

namespace Graphic_Components
{
    public class SpinComponent : MonoBehaviour
    {
        public float RotationSpeed = 1.5f;
        public Vector3 Rotation = new Vector3(0,1,0);
        
        public void FixedUpdate()
        {
            transform.RotateAround(transform.position ,Rotation, RotationSpeed);
        }
    }
}
