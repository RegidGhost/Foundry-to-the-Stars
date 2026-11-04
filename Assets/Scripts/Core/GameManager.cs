using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public static event Action<float> OnTick;
    public const float TickInterval = 0.05f;

    private float _timer;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    private void Start()
    {
        // Запускаем автосохранение
        StartCoroutine(AutoSaveRoutine());
    }

    private void Update()
    {
        _timer += Time.deltaTime;
        while (_timer >= TickInterval)
        {
            _timer -= TickInterval;
            OnTick?.Invoke(TickInterval);
        }
    }

    private IEnumerator AutoSaveRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(300f); // Ждём 5 минут (300 секунд)
            SaveSystem.Save();
        }
    }
}