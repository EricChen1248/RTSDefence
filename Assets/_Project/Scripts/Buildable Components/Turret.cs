using Scripts.Entity_Components;
using UnityEngine;

namespace Scripts.Buildable_Components
{
    
    [RequireComponent(typeof(FieldOfViewComponent))]
    public class Turret : MonoBehaviour
    {
        public int ReloadSpeed = 1;
        public int ReloadTime = 100;
        public float CurrentReload;

        public Transform FirePoint;
        public GameObject Bullet;

        private FieldOfViewComponent _fovComponent;

        // Use this for initialization
        protected void Start ()
        {
            _fovComponent = GetComponent<FieldOfViewComponent>();
        }
	
        // Update is called once per frame
        private void FixedUpdate ()
        {
            if (CurrentReload > 0)
            {
                CurrentReload -= ReloadSpeed * Time.deltaTime;
            }
            var target = _fovComponent.FindClosestTarget();
            if (target == null) return;
            Follow(target);
            Fire(target);
        }


        private const int FollowSpeed = 5;
        private void Follow(Component col)
        {
            var targetRotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(col.transform.position - transform.position, Vector3.up));
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, FollowSpeed * Time.deltaTime);
        }

        private void Fire(Component col)
        {
            if (CurrentReload > 0)
            {
                return;
            }
            if (!_fovComponent.TargetInField(col.transform.position)) return;
            var bullet = Instantiate(Bullet, FirePoint.position, Quaternion.identity);
            bullet.GetComponent<BulletScript>().Target = col.transform;
            CurrentReload = ReloadTime;
        }
    }
}
