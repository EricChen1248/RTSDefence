using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace Buildable_Components
{
    public class EntranceAnimator : MonoBehaviour
    {

        public MoverType MoveType;
        public DirectionType Direction;
        public GameObject GameObject;
        public Transform ChangePivot;
        public int ChangeAmount;
        public int ChangeSpeed;
        public int PauseTime;

        private int _changed;
        private IEnumerator _openRoutine;
        

        // Update is called once per frame
        private void OnTriggerEnter (Collider other)
        {
            print("TriggerEnter");

            if (_openRoutine != null)
            {
                StopCoroutine(_openRoutine);
            }

            switch (MoveType)
            {
                case MoverType.Rotate:
                    _openRoutine = Rotate();
                    break;
                case MoverType.Move:
                    break;
                case MoverType.Shrink:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            StartCoroutine(_openRoutine);
        }

        private IEnumerator Rotate()
        {
            Vector3 rotation;

            switch (Direction)
            {
                case DirectionType.Up:
                    rotation = Vector3.up;
                    break;
                case DirectionType.Down:
                    rotation = Vector3.down;
                    break;
                case DirectionType.Left:
                    rotation = Vector3.left;
                    break;
                case DirectionType.Right:
                    rotation = Vector3.right;
                    break;
                case DirectionType.Forward:
                    rotation = Vector3.forward;
                    break;
                case DirectionType.Back:
                    rotation = Vector3.back;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            for (; _changed < ChangeAmount / ChangeSpeed; _changed++)
            {
                GameObject.transform.RotateAround(ChangePivot.position, rotation, ChangeSpeed);
                yield return new WaitForFixedUpdate();
            }

            for (int i = 0; i < PauseTime; i++)
            {
                yield return new WaitForFixedUpdate();
            }

            for (; _changed > 0; _changed--)
            {
                GameObject.transform.RotateAround(ChangePivot.position, rotation, -ChangeSpeed);
                yield return new WaitForFixedUpdate();
            }

            _openRoutine = null;
        }

        public enum MoverType
        {
            Rotate,
            Move,
            Shrink
        }

        public enum DirectionType
        {
            Up,
            Down,
            Left,
            Right,
            Forward,
            Back,
        }
    }
}
