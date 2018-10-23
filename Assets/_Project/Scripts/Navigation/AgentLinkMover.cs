using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Scripts.Navigation
{
    public enum OffMeshLinkMoveMethod
    {
        Teleport,
        NormalSpeed,
        Parabola,
        Curve
    }

    [RequireComponent(typeof(NavMeshAgent))]
    public class AgentLinkMover : MonoBehaviour
    {
        public OffMeshLinkMoveMethod MMethod = OffMeshLinkMoveMethod.Parabola;
        public AnimationCurve MCurve = new AnimationCurve();

        private IEnumerator Start()
        {
            var agent = GetComponent<NavMeshAgent>();
            agent.autoTraverseOffMeshLink = false;
            while (true)
            {
                if (agent.isOnOffMeshLink)
                {
                    switch (MMethod)
                    {
                        case OffMeshLinkMoveMethod.NormalSpeed:
                            yield return StartCoroutine(NormalSpeed(agent));
                            break;
                        case OffMeshLinkMoveMethod.Parabola:
                            yield return StartCoroutine(Parabola(agent, 2.0f, 0.5f));
                            break;
                        case OffMeshLinkMoveMethod.Curve:
                            yield return StartCoroutine(Curve(agent, 0.5f));
                            break;
                        case OffMeshLinkMoveMethod.Teleport:
                            throw new NotImplementedException();
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    agent.CompleteOffMeshLink();
                }
                yield return null;
            }
        }

        private static IEnumerator NormalSpeed(NavMeshAgent agent)
        {
            var data = agent.currentOffMeshLinkData;
            var endPos = data.endPos + Vector3.up * agent.baseOffset;
            while (agent.transform.position != endPos)
            {
                agent.transform.position = Vector3.MoveTowards(agent.transform.position, endPos, agent.speed * Time.deltaTime);
                yield return null;
            }
        }

        private static IEnumerator Parabola(NavMeshAgent agent, float height, float duration)
        {
            var data = agent.currentOffMeshLinkData;
            var startPos = agent.transform.position;
            var endPos = data.endPos + Vector3.up * agent.baseOffset;
            var normalizedTime = 0.0f;
            while (normalizedTime < 1.0f)
            {
                var yOffset = height * 4.0f * (normalizedTime - normalizedTime * normalizedTime);
                agent.transform.position = Vector3.Lerp(startPos, endPos, normalizedTime) + yOffset * Vector3.up;
                normalizedTime += Time.deltaTime / duration;
                yield return null;
            }
        }

        private IEnumerator Curve(NavMeshAgent agent, float duration)
        {
            var data = agent.currentOffMeshLinkData;
            var startPos = agent.transform.position;
            var endPos = data.endPos + Vector3.up * agent.baseOffset;
            var normalizedTime = 0.0f;
            while (normalizedTime < 1.0f)
            {
                var yOffset = MCurve.Evaluate(normalizedTime);
                agent.transform.position = Vector3.Lerp(startPos, endPos, normalizedTime) + yOffset * Vector3.up;
                normalizedTime += Time.deltaTime / duration;
                yield return null;
            }
        }
    }
}