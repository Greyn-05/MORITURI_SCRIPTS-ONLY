using System.Collections;
using UnityEngine;

public class NPCAnimation : MonoBehaviour
{
    private Animator _animator;
    private string[] triggers = { "Trigger1", "Trigger2", "Trigger3", "Trigger4", "Trigger5", "Trigger6", "Trigger7", "Trigger8", "Trigger9", "Trigger10", "Trigger11", "Trigger12", "Trigger13", "Trigger14" };

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        TriggerRandomAnimation();
    }

    public void OnAnimationEnd()
    {
        TriggerRandomAnimation();
    }

    private void TriggerRandomAnimation()
    {
        foreach (var trigger in triggers)
        {
            _animator.ResetTrigger(trigger);
        }
        
        string randomTrigger = triggers[Random.Range(0, triggers.Length)];
        _animator.SetTrigger(randomTrigger);
    }
}