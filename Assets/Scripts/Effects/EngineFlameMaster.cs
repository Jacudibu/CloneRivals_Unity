using System.Linq;
using Settings.InputConfiguration;
using UnityEditor;
using UnityEngine;

namespace Effects
{
    public class EngineFlameMaster : MonoBehaviour
    {
        private EngineFlameParticleSystem[] _engineFlameParticleSystems;

        public float boostFactor = 1.5f;
        public float slowFactor = 0.5f;
    
        private struct EngineFlameParticleSystem
        {
            private ParticleSystem.MainModule _particleSystemMainModule;
        
            private readonly ParticleSystem.MinMaxCurve _defaultLifeTime;
            private readonly ParticleSystem.MinMaxCurve _defaultSize;
            private readonly ParticleSystem.MinMaxCurve _defaultSpeed;

            private readonly ParticleSystem.MinMaxCurve _boostLifeTime;
            private readonly ParticleSystem.MinMaxCurve _boostSize;
            private readonly ParticleSystem.MinMaxCurve _boostSpeed;

            private readonly ParticleSystem.MinMaxCurve _slowLifeTime;
            private readonly ParticleSystem.MinMaxCurve _slowSize;
            private readonly ParticleSystem.MinMaxCurve _slowSpeed;

            public EngineFlameParticleSystem(ParticleSystem particleSystem, float boostFactor, float slowFactor)
            {
                _particleSystemMainModule = particleSystem.main;
                _defaultLifeTime = _particleSystemMainModule.startLifetime;
                _boostLifeTime = new ParticleSystem.MinMaxCurve(_defaultLifeTime.constantMin * boostFactor, _defaultLifeTime.constantMax * boostFactor);
                _slowLifeTime = new ParticleSystem.MinMaxCurve(_defaultLifeTime.constantMin * slowFactor, _defaultLifeTime.constantMax * slowFactor);

                _defaultSize = _particleSystemMainModule.startSize;
                _boostSize = new ParticleSystem.MinMaxCurve(_defaultSize.constantMin * boostFactor, _defaultSize.constantMax * boostFactor);
                _slowSize = new ParticleSystem.MinMaxCurve(_defaultSize.constantMin * slowFactor, _defaultSize.constantMax * slowFactor);

                _defaultSpeed = _particleSystemMainModule.startSpeed;
                _boostSpeed = new ParticleSystem.MinMaxCurve(_defaultSpeed.constantMin * boostFactor, _defaultSpeed.constantMax * boostFactor);
                _slowSpeed = new ParticleSystem.MinMaxCurve(_defaultSpeed.constantMin * slowFactor, _defaultSpeed.constantMax * slowFactor);
            }

            public void SetDefault()
            {
                _particleSystemMainModule.startLifetime = _defaultLifeTime;
                _particleSystemMainModule.startSize = _defaultSize;
                _particleSystemMainModule.startSpeed = _defaultSpeed;
            }

            public void SetBoost()
            {
                _particleSystemMainModule.startLifetime = _boostLifeTime;
                _particleSystemMainModule.startSize = _boostSize;
                _particleSystemMainModule.startSpeed = _boostSpeed;
            }
        
            public void SetSlow()
            {
                _particleSystemMainModule.startLifetime = _slowLifeTime;
                _particleSystemMainModule.startSize = _slowSize;
                _particleSystemMainModule.startSpeed = _defaultSpeed; //slowspeed just doesn't look great.
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (EditorApplication.isPlaying || EditorApplication.isPaused)
            {
                Start();
            }
        }
#endif

        void Start()
        {
            _engineFlameParticleSystems = GetComponentsInChildren<ParticleSystem>()
                .Select(x => new EngineFlameParticleSystem(x, boostFactor, slowFactor))
                .ToArray();
        }

        void Update()
        {
            if (KeyBindings.Boost.IsPressed())
            {
                foreach (var engineFlameParticleSystem in _engineFlameParticleSystems)
                {
                    engineFlameParticleSystem.SetBoost();
                }
            } 
            else if (KeyBindings.Brake.IsPressed())
            {
                foreach (var engineFlameParticleSystem in _engineFlameParticleSystems)
                {
                    engineFlameParticleSystem.SetSlow();
                }
            }
            else
            {
                foreach (var engineFlameParticleSystem in _engineFlameParticleSystems)
                {
                    engineFlameParticleSystem.SetDefault();
                }
            }
        }
    }
}
