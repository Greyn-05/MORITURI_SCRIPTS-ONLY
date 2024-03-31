using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectController : MonoBehaviour
{
    [Header("Effect")]
    [SerializeField]
    private ParticleSystem parrySparkPrefab;

    [SerializeField]
    private ParticleSystem parryShockWavePrefab;

    [SerializeField]
    private ParticleSystem guardSparkPrefab;

    private ParticleSystem parrySpark;
    private ParticleSystem parryShockWave;
    private ParticleSystem guardSpark;

    [SerializeField]
    private Transform[] randomEffectPosition;
    void Start()
    {
        parrySpark = Instantiate(parrySparkPrefab, gameObject.transform);
        parrySpark.transform.position = randomEffectPosition[0].position;
        guardSpark = Instantiate(guardSparkPrefab, gameObject.transform);
        guardSpark.transform.position = randomEffectPosition[0].position;
        parryShockWave = Instantiate(parryShockWavePrefab, gameObject.transform);
        parryShockWave.transform.position = randomEffectPosition[0].position;
    }

    public void PlayParrySpark()
    {
        int T = Random.Range(0, randomEffectPosition.Length);
        parrySpark.transform.position = randomEffectPosition[T].position;
        parryShockWave.transform.position = randomEffectPosition[T].position;
        parryShockWave.Play();
        parrySpark.Play();
    }

    public void PlayGuardSpark()
    {
        guardSpark.transform.position = randomEffectPosition[Random.Range(0, randomEffectPosition.Length)].position;
        guardSpark.Play();
    }


}
