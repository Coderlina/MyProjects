using System;
using UnityEngine;
using System.Collections;

/// <summary>
/// Timer class for starting/stopping time in Game Of Life.
/// </summary>
public class Timer : MonoBehaviour {
    public bool IsTimerOn { set; get; }
    public event Action<bool> OnTimerToggleCallback;

    [Tooltip("The one generation's duration in seconds.")]
    [SerializeField] private float _stepDuration = 1.0f;
    private WaitForSeconds _waitTime;
    private GameOfLife _gameOfLife;


    /// <summary>
    /// Save the timer's delay to a variable
    /// for more efficient garbage collection.
    /// </summary>
    private void Start()
    {
        _gameOfLife = FindObjectOfType<GameOfLife>();
        _waitTime = new WaitForSeconds(_stepDuration);
    }

    /// <summary>
    /// Toggle the advancing of time on/off.
    /// </summary>
    public void ToggleTimer()
    {
        IsTimerOn = !IsTimerOn;
        if (OnTimerToggleCallback != null) {
            OnTimerToggleCallback(IsTimerOn);
        }
        if (IsTimerOn)
        {
            StartCoroutine(AdvanceTime());
        }
        else
        {
            StopCoroutine(AdvanceTime());
        }
    }

    /// <summary>
    /// Co-routine that makes the game move to next cell generation
    /// every seconds determined in _stepDuration variable.
    /// </summary>
    private IEnumerator AdvanceTime()
    {
        while (IsTimerOn)
        {
            _gameOfLife.OnGenerationLifetimeEnded();
            yield return _waitTime;
        }
    }

}
