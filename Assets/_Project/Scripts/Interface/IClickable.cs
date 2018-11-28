using UnityEngine;

namespace Scripts.Interface
{
    public interface IClickable
    {
        bool HasFocus { get; }

        void Focus();
        void LostFocus();

        void RightClick(Vector3 clickPosition);
    }

}
