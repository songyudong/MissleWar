using UnityEngine;
using System.Collections;

public class DamageManager : MonoBehaviour
{
    public AudioClip[] hitsound;
    public GameObject effect;
    public int HP_MAX = 200;
    public int HP;

    void Awake()
    {        
    }

    void OnDestroy()
    {
        EventManager.Notify(SCEvent.Battle_Unit_Dead, this.gameObject);
    }

    private void Start()
    {
        HP = HP_MAX;
        EventManager.Notify(SCEvent.Battle_Unit_Spawn, this.gameObject);
    }

    public void ApplyDamage(int damage)
    {
		if(HP<0)
		return;
	
        if (hitsound.Length > 0)
        {
            AudioSource.PlayClipAtPoint(hitsound[Random.Range(0, hitsound.Length)], transform.position);
        }
        HP -= damage;
        if (HP <= 0)
        {
            Dead();
        }
    }

    private void Dead()
    {
        if (effect)
            Instantiate(effect, transform.position, transform.rotation);
        Destroy(this.gameObject);
    }

}
