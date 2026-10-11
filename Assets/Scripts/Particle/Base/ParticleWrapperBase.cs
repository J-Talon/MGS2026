using Particle.Interfaces;
using UnityEngine;


/*
 * @Author Talon J
 */
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
       
        
        /**
         * Do not modify or define your own OnParticleSystemStopped function. Instead override the
         * OnStop function in a subclass.
         * You should have good reason to want to modify this. 
         */
        private void OnParticleSystemStopped() {
            ParticleManager.GetInstance().ReturnObject(this);
            this.OnStop();
        }
        
        /**
         * UNDER NO CIRCUMSTANCE should you call this function by itself.
         * Instead do ParticleManager.GetInstance().ReturnObject(particle) to stop the particle from playing
         * Doing this without returning the particle will cause it to not be properly tracked by the manager.
         * 
         */
        public void StopPlaying()
        {
            gameObject.SetActive(false);
            particleEmitter.Stop();
            gameObject.transform.SetParent(null); //if for some reason people attach them to objects

        }

        /**
         * UNDER NO CIRCUMSTANCE should you call this function by itself if you want it to be automatically handled
         * by the particle manager
         * Create a new particle instead.
         */
        public void StartPlaying()
        {
            transform.SetParent(null);  //if for some reason people attach them to objects
            gameObject.SetActive(true);
            if (particleEmitter.isEmitting) return;
            particleEmitter.Play();
            this.OnPlay();
        }


        public string GetAssetPath() {
            return _particleType;
        }

        public ParticleSystem GetParticleSystem() {
            return this.particleEmitter;
        }



        public void Initialize(string particleType)
        {
            if (_initialized) return;
            
            _particleType = particleType;
            _initialized = true;
        }
        
        
    }
}