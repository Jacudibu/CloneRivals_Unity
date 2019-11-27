using System.Linq;
using InputConfiguration;
using UnityEditor;
using UnityEngine;

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

        private readonly ParticleSystem.MinMaxCurve _boostLifeTime;
        private readonly ParticleSystem.MinMaxCurve _boostSize;
        
        private readonly ParticleSystem.MinMaxCurve _slowLifeTime;
        private readonly ParticleSystem.MinMaxCurve _slowSize;

        public EngineFlameParticleSystem(ParticleSystem particleSystem, float boostFactor, float slowFactor)
        {
            _particleSystemMainModule = particleSystem.main;
            _defaultLifeTime = _particleSystemMainModule.startLifetime;
            _boostLifeTime = new ParticleSystem.MinMaxCurve(_defaultLifeTime.constantMin * boostFactor, _defaultLifeTime.constantMax * boostFactor);
            _slowLifeTime = new ParticleSystem.MinMaxCurve(_defaultLifeTime.constantMin * slowFactor, _defaultLifeTime.constantMax * slowFactor);

            _defaultSize = _particleSystemMainModule.startSize;
            _boostSize = new ParticleSystem.MinMaxCurve(_defaultSize.constantMin * boostFactor, _defaultSize.constantMax * boostFactor);
            _slowSize = new ParticleSystem.MinMaxCurve(_defaultSize.constantMin * slowFactor, _defaultSize.constantMax * slowFactor);
        }

        public void SetDefault()
        {
            _particleSystemMainModule.startLifetime = _defaultLifeTime;
            _particleSystemMainModule.startSize = _defaultSize;
        }

        public void SetBoost()
        {
            _particleSystemMainModule.startLifetime = _boostLifeTime;
            _particleSystemMainModule.startSize = _boostSize;
        }
        
        public void SetSlow()
        {
            _particleSystemMainModule.startLifetime = _slowLifeTime;
            _particleSystemMainModule.startSize = _slowSize;
        }
    }

    private void OnValidate()
    {
        if (EditorApplication.isPlaying || EditorApplication.isPaused)
        {
            Start();
        }
    }

    void Start()
    {
        _engineFlameParticleSystems = GetComponentsInChildren<ParticleSystem>()
            .Select(x => new EngineFlameParticleSystem(x, boostFactor, slowFactor))
            .ToArray();
    }

    void Update()
    {
        if (Input.GetKey(KeyBindings.Boost))
        {
            foreach (var engineFlameParticleSystem in _engineFlameParticleSystems)
            {
                engineFlameParticleSystem.SetBoost();
            }
        } 
        else if (Input.GetKey(KeyBindings.Brake))
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
