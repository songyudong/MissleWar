using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoverBallistic : MonoBehaviour
{
    public GameObject target;
    public string targettag;
    public float damping = 3;
    public float Speed = 80;
    public float SpeedMax = 80;
    public float SpeedMult = 1;
    public Vector3 Noise = new Vector3(20, 20, 20);
    private bool locked;
    public int distanceLock = 70;
    public float DulationLock = 1.0f;
    private float timetorock;
    public bool Seeker;
    public float LifeTime = 5.0f;
    private float timeCount = 0;
    public float targetlockdirection = 0.5f;

    private void Start()
    {
        timeCount = Time.time;
        Destroy(gameObject, LifeTime);
        timetorock = 0;
        target = null;
    }

    private void Update()
    {
        if (Time.time >= (timeCount + LifeTime) - 0.5f)
        {
            if (GetComponent<Damage>())
            {
                GetComponent<Damage>().Active();
            }
        }
        if (Seeker)
        {
            if (timetorock > DulationLock)
            {
                if (!locked && !target)
                {
                    float distance = int.MaxValue;
                    if (GameObject.FindGameObjectsWithTag(targettag).Length > 0)
                    {
                        GameObject[] objs = GameObject.FindGameObjectsWithTag(targettag);

                        for (int i = 0; i < objs.Length; i++)
                        {
                            if (objs[i])
                            {
                                float dis = Vector3.Distance(objs[i].transform.position, transform.position);
                                if (distanceLock > dis)
                                {
                                    if (distance > dis)
                                    {
                                        distance = dis;
                                        target = objs[i];
                                    }
                                    locked = true;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                timetorock += Time.deltaTime;
                //Debug.LogErrorFormat("timelock = {0}", timetorock);
            }

            if (target)
            {
                damping += 0.9f;
                Vector3 vCenter = Vector3.zero;
                if (target.GetComponent<BoxCollider>())
                    vCenter = target.GetComponent<BoxCollider>().center;
                if (target.GetComponent<CharacterController>())
                    vCenter = target.GetComponent<CharacterController>().center;
                Vector3 vTargetCenter = target.transform.position + vCenter;
                Quaternion rotation = Quaternion.LookRotation(vTargetCenter - transform.transform.position);

                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
                Vector3 dir = (target.transform.position - transform.position).normalized;
                float direction = Vector3.Dot(dir, transform.forward);
                if (direction < targetlockdirection)
                {
                    target = null;
                }
            }
            else
            {
                locked = false;

            }
        }
        if (Speed < SpeedMax)
        {
            Speed += SpeedMult;
        }


        GetComponent<Rigidbody>().velocity = new Vector3(transform.forward.x * Speed * Time.deltaTime,
                                         transform.forward.y * Speed * Time.deltaTime,
                                         transform.forward.z * Speed * Time.deltaTime);
        GetComponent<Rigidbody>().velocity += new Vector3(Random.Range(-Noise.x, Noise.x), Random.Range(-Noise.y, Noise.y),
                                          Random.Range(-Noise.z, Noise.z));

    }
}
