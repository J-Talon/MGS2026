using Particle.Interfaces;
using UnityEngine;

namespace Particle.Base
{
    public abstract class ParticleWrapperBase : MonoBehaviour, IParticleWrapper
    {
        protected ParticleSystem particleEmitter;
        
        private string _particleType; //this is used as identification for the particle to denote type
        private bool _initialized;


        protected abstract void OnStop();

        protected abstract void OnPlay();



        private void Awake() {
            particleEmitter = GetComponent<ParticleSystem>();
            DontDestroyOnLoad(gameObject);
        }
       
        
        //Event function
        private void OnParticleSystemStopped() {
            ParticleManager.GetInstance().ReturnObject(this);
            this.OnStop();
        }

        public void StopPlaying()
        {
            gameObject.SetActive(false);
            particleEmitter.Stop();
            
        }

        public void StartPlaying()
        {
            if (particleEmitter.isEmitting) return;
            particleEmitter.Play();
            this.OnPlay();
        }


        public string GetAssetPath() {
            return _particleType;
        }


        public void Initialize(string particleType)
        {
            if (_initialized) return;
            
            _particleType = particleType;
            _initialized = true;
        }
        
        
    }
}