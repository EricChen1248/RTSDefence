using System.Collections;
using Scripts.Controllers;
using Scripts.Entity_Components.Misc;
using Scripts.Graphic_Components;
using Scripts.GUI;
using Scripts.Interface;
using Scripts.Scriptable_Objects;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Scripts.Buildable_Components
{
    [SelectionBase]
    [RequireComponent(typeof(HealthComponent))]
    public class Buildable : MonoBehaviour, IClickable
    {
        public BuildData Data;
        public int ID;
        public string Type => Data.Name;

        public bool HasFocus { get; protected set; }

        public virtual void Focus()
        {
            HasFocus = true;

            var omg = ObjectMenuGroupComponent.Instance;

            omg.SetButton(0, "Destroy", () => Destroy(true));
            omg.SetButtonImage(0, UnityEngine.Resources.Load<Texture>("Sprites/destroy"));
            omg.Show();

            StartCoroutine(DetectLoseFocus());
        }

        public virtual void LostFocus()
        {
            HasFocus = false;

            ObjectMenuGroupComponent.Instance.Hide();
        }

        public virtual void RightClick(Vector3 pos)
        {
        }

        public virtual void Start()
        {
            var health = GetComponent<HealthComponent>();
            health.MaxHealth = Data.Health;
            ID = GetInstanceID();
            health.OnDeath += e => Destroy();
        }

        public virtual void Destroy(bool returnResource)
        {
            if (returnResource)
                foreach (var resource in Data.Recipe)
                    ResourceController.AddResource(resource.Resource, resource.Amount);
            var health = GetComponentInChildren<HealthBarComponent>();
            if (health != null) health.Hide();
            var dc = gameObject.AddComponent<DestroyComponent>();
            dc.size = Data.Size.y;
            WaveController.Instance.AddScore(-Data.Points);

            if (CoreController.MouseController.FocusedItem.Contains(this))
                CoreController.MouseController.SetFocus(null);
        }

        public virtual void Destroy()
        {
            Destroy(false);
        }

        public void OnMouseUpAsButton()
        {
            CoreController.MouseController.SetFocus(this);
        }

        private IEnumerator DetectLoseFocus()
        {
            while (true)
            {
                if (Input.GetMouseButtonDown(0))
                    if (!EventSystem.current.IsPointerOverGameObject())
                        if (Camera.main != null)
                        {
                            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                            RaycastHit hit;
                            if (Physics.Raycast(ray, out hit))
                                if (hit.collider.gameObject != gameObject)
                                {
                                    CoreController.MouseController.SetFocus(null);
                                    yield break;
                                }
                        }

                yield return null;
            }
        }
    }
}