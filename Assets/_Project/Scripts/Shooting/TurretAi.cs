using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TurretAi : MonoBehaviour
{
    public enum AiStates { NEAREST, FURTHEST, WEAKEST, STRONGEST };

    public AiStates aiState = AiStates.NEAREST;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!m_tracker)
            return;

        switch (aiState)
        {
            case AiStates.NEAREST:
                TargetNearest();
                break;
            case AiStates.FURTHEST:
                TargetFurthest();
                break;
            case AiStates.WEAKEST:
                TargetWeakest();
                break;
            case AiStates.STRONGEST:
                TargetStrongest();
                break;
        }
    }

    void TargetNearest()
    {
        List<GameObject> validTargets = null;//m_range.GetValidTargets();

        GameObject curTarget = null;
        float closestDist = 0.0f;

        for (int i = 0; i < validTargets.Count; i++)
        {
            float dist = Vector3.Distance(transform.position, validTargets[i].transform.position);

            if (!curTarget || dist < closestDist)
            {
                curTarget = validTargets[i];
                closestDist = dist;
            }
        }

        m_tracker.SetTarget(curTarget);
    }

    void TargetFurthest()
    {
        List<GameObject> validTargets = null;//m_range.GetValidTargets();

        GameObject curTarget = null;
        float furthestDist = 0.0f;

        for (int i = 0; i < validTargets.Count; i++)
        {
            float dist = Vector3.Distance(transform.position, validTargets[i].transform.position);

            if (!curTarget || dist > furthestDist)
            {
                curTarget = validTargets[i];
                furthestDist = dist;
            }
        }

        m_tracker.SetTarget(curTarget);
    }

    void TargetWeakest()
    {
        /*    List<GameObject> validTargets = m_range.GetValidTargets();

        GameObject curTarget = null;
        int lowestHealth = 0;

        for (int i = 0; i < validTargets.Count; i++)
        {
            int maxHp = validTargets[i].GetComponent<Health>().maxHealth;

            if (!curTarget || maxHp < lowestHealth)
            {
                lowestHealth = maxHp;
                curTarget = validTargets[i];
            }
        }

        m_tracker.SetTarget(curTarget);
        */
    }

    void TargetStrongest()
    {
        /*
        List<GameObject> validTargets = m_range.GetValidTargets();

        GameObject curTarget = null;
        int highestHealth = 0;

        for (int i = 0; i < validTargets.Count; i++)
        {
            int maxHp = validTargets[i].GetComponent<Health>().maxHealth;

            if (!curTarget || maxHp > highestHealth)
            {
                highestHealth = maxHp;
                curTarget = validTargets[i];
            }
        }

        m_tracker.SetTarget(curTarget);
        */
    }
}