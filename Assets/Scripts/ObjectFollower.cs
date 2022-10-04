using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFollower : MonoBehaviour
{
    [SerializeField] float speed = 2f;

    public void StartMoving(Transform target)
    {
        StartCoroutine(startMoving(target));
    }

    IEnumerator startMoving(Transform target)
    {
        while (enabled)
        {
            Vector3 vectorToTarget = target.transform.position - transform.position;
            //vectorToTarget.y = 0;
            transform.position = transform.position + vectorToTarget.normalized * speed * Time.deltaTime;
            yield return new WaitForSeconds(0.01f);
        }
    }
}
