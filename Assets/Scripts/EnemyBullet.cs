using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] float strengthDamage = 25f;

    private void OnTriggerEnter(Collider other)
    {
        string tag = other.tag;
        if(tag == "Player")
        {
            Player player = other.gameObject.GetComponent<Player>();
            player.ApplyStrenghtChanges(strengthDamage);
        }
        
        DestroyObject(gameObject);
    }
}
