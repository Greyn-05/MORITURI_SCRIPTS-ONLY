using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockOnUI : MonoBehaviour
{
    [SerializeField] private Transform _target;

    private void OnEnable()
    {
        if (_target == null) _target = Camera.main.transform;
        StartCoroutine(LookAtTarget());
    }

    private IEnumerator LookAtTarget()
    {
        while (this.gameObject.activeInHierarchy)
        {
            Vector3 dir = _target.position - transform.position;
            //dir.y = 0;
            transform.rotation = Quaternion.LookRotation(dir);
            yield return null;
        }
    }
}
