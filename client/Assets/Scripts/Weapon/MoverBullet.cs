using UnityEngine;
using System.Collections;

public class MoverBullet : MonoBehaviour
{
    public AudioClip[] Sounds;
    public int Lifetime;
    public bool Projectile;
    public float Speed = 80;
    public float SpeedMax = 80;
    public float SpeedMult = 1;

    private void Start()
    {
        Destroy(gameObject, Lifetime);

    }

    private void Update()
    {
        if (!Projectile)
        {

            GetComponent<Rigidbody>().velocity += transform.forward*Time.deltaTime*Speed;
        }
        if (Speed < SpeedMax)
        {
            Speed += SpeedMult;
        }


    }

}
