using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blood : MonoBehaviour
{
    #region Field

    [SerializeField] private Health _health;
    [SerializeField] private GameObject[] _bloodPrefab = new GameObject[2];
    [SerializeField] private GameObject[] _bloodSpawn = new GameObject[5];

    #endregion

    public void BloodEffect()
    {
        int randPrefab = Random.Range(0, _bloodPrefab.Length);
        int randSpawn = Random.Range(0, _bloodSpawn.Length);
        
        _bloodPrefab[randPrefab].SetActive(false);
        _bloodPrefab[randPrefab].transform.SetParent(_bloodSpawn[randSpawn].transform);
        _bloodPrefab[randPrefab].transform.localPosition = Vector3.zero;
        _bloodPrefab[randPrefab].transform.localRotation = Quaternion.identity;
        _bloodPrefab[randPrefab].SetActive(true);
    }

}
