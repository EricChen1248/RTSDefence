using UnityEngine;
using System.Collections;

public class TrackingProjectile : BaseProjectile
{
    GameObject m_target;


    // Update is called once per frame
    void Update()
    {
        if(m_target){
            transform.position = Vector3.MoveTowards(transform.position, m_target.transform.position, speed * Time.deltaTime);
        }
    }

    public override void FireProjectile(GameObject launcher, GameObject target, int damage, float attackSpeed)
    {
        if (target)
        {
            m_target = target;
          
        }
    }
}