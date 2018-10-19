using UnityEngine;

namespace Interface
{
    public interface IPlayerControllable
    {
        bool HasFocus { get; }

        void Focus();
        void LostFocus();

        void RightClick(Vector3 clickPosition);
    }

}
