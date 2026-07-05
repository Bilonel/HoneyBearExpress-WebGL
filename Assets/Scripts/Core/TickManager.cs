using UnityEngine;
using System;

namespace HoneyBearExpress.Core
{
    public class TickManager : MonoBehaviour
    {
        [SerializeField, Min(1)] private int ticksPerSecond = 10;
        
        private float _tickInterval;
        private float _tickTimer;
        private bool _isRunning;
        
        public event Action<long> OnTick;
        
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
        
        private void Awake()
        {
            TicksPerSecond = ticksPerSecond;
        }
        
        private void Update()
        {
            if (!_isRunning) return;
            
            _tickTimer += Time.deltaTime;
            
            while (_tickTimer >= _tickInterval)
            {
                _tickTimer -= _tickInterval;
                CurrentTick++;
                OnTick?.Invoke(CurrentTick);
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
