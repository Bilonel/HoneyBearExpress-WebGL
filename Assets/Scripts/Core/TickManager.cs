using UnityEngine;
using System;
using System.Collections.Generic;

namespace HoneyBearExpress.Core
{
    public class TickManager : MonoBehaviour
    {
        [SerializeField, Min(1)] private int ticksPerSecond = 10;
        
        private float _tickInterval;
        private float _tickTimer;
        private bool _isRunning;
        
        public int TicksPerSecond
        {
            get => ticksPerSecond;
            set
            {
                ticksPerSecond = Mathf.Max(1, value);
                _tickInterval = 1f / ticksPerSecond;
            }
        }
        
        public bool IsRunning => _isRunning;
        public long CurrentTick { get; private set; }
        public float TickInterval => _tickInterval;
        
        private readonly List<ITickable> _tickables = new List<ITickable>(1024);

        private void Awake()
        {
            TicksPerSecond = ticksPerSecond;
        }
        private void Start()
        {
            StartTicking();
        }

        public void RegisterTickable(ITickable tickable)
        {
            if (!_tickables.Contains(tickable))
                _tickables.Add(tickable);
        }

        public void UnregisterTickable(ITickable tickable)
        {
            _tickables.Remove(tickable);
        }

        private void Update()
        {
            if (!_isRunning) return;
            
            _tickTimer += Time.deltaTime;
            
            while (_tickTimer >= _tickInterval)
            {
                _tickTimer -= _tickInterval;
                CurrentTick++;
                
                // Güvenli ve hızlı iterasyon
                for (int i = 0; i < _tickables.Count; i++)
                {
                    _tickables[i].OnTick(CurrentTick);
                }
            }
        }
        
        public void StartTicking()
        {
            _isRunning = true;
            _tickTimer = 0f;
        }
        
        public void StopTicking()
        {
            _isRunning = false;
        }
        
        public void ResetTicks()
        {
            CurrentTick = 0;
            _tickTimer = 0f;
        }
    }
}
