using UnityEngine;

namespace Scripts.Interface
{
    public interface IClickable
    {
        bool HasFocus { get; }
        string Type { get; }


        void Focus();
        void LostFocus();

        void RightClick(Vector3 vec);
    }
}