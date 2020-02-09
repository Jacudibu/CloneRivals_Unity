using System.Linq;
using GearConfigurator;
using Settings.InputConfiguration;
using UnityEditor;
using UnityEngine;
using WebSocketSharp;

namespace Effects
{
    public class EngineFlameMaster : MonoBehaviour
    {
        private EngineFlameParticleSystem[] _engineFlameParticleSystems;

        public float boostFactor = 1.5f;
        public float slowFactor = 0.5f;
    
        private struct EngineFlameParticleSystem
        {
            private readonly ParticleSystem _particleSystem;
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
                _particleSystem = particleSystem;
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

            public void SetColors(Color innerFlame, Color outerFlame)
            {
                var colorOverLifetime = _particleSystem.colorOverLifetime;
                var gradient = new Gradient();
                var usedColor = _particleSystem.name.ToLower().Contains("inner") ? innerFlame : outerFlame;
                
                gradient.SetKeys(new GradientColorKey[]
                    {
                        new GradientColorKey(usedColor, 0f), 
                        new GradientColorKey(usedColor * 0.8f, 0.35f), 
                    },
                    colorOverLifetime.color.gradient.alphaKeys
                );
                colorOverLifetime.color = gradient;
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

        public void ApplyEngineFlameConfiguration(EngineFlameConfiguration configuration)
        {
            foreach (var flameParticles in _engineFlameParticleSystems)
            {
                flameParticles.SetColors(configuration.innerFlameColor, configuration.outerFlameColor);
            }
        }
    }
}
