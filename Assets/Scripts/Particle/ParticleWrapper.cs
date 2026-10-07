using System;
using UnityEngine;

namespace Particle
{
    public abstract class ParticleWrapper : MonoBehaviour
    {
        protected ParticleSystem particleEmitter;


        private string _particleType; //this is used as identification for the particle to denote type
        private bool _initialized;



        private void Awake() {
            particleEmitter = GetComponent<ParticleSystem>();
            DontDestroyOnLoad(gameObject);
        }
       
       
       
        
        private void OnParticleSystemStopped()
        {
            
        }
        
        
        
        //================


        public string GetAssetPath()
        {
            return _particleType;
        }

        public bool IsInitialized()
        {
            return _initialized;
        }


        public void Initialize(string particleType)
        {
            if (_initialized) return;
            
            _particleType = particleType;
            _initialized = true;
        }
        
        
    }
}