using Entities.Unit;

namespace Effect.Abstract
{
    
    
    
    //@Author Talon J
    /*
     * we don't wanna have a global system for callbacks since that will mess up events
     * someone will surely try and use the events (which are intended for the callbacks) for something else
     * and I can see that very easily bricking the system, so
     * yes, we're not using events here, BUT we're doing a system very similar to events
     */
    
    public abstract class Effect
    {
        protected double duration;
        protected bool finite;
        protected bool isSpent;
        
        public Effect(double duration, bool finite) {
            this.duration = duration;
            this.finite = finite;
            isSpent = false;
        }

        //return whether the effect is indefinite
        public bool IsFinite() {
            return finite;
        }

        //return duration in seconds
        public double GetDuration() {
            return duration;
        }

        // set duration in seconds
        public void SetDuration(double durationSeconds) {
            this.duration = durationSeconds;
        }

        public virtual bool IsSpent() {
            return isSpent;
        }
        

        // called when the effect is applied onto the thing
        public abstract void OnStart();
        
        //called each tick, if needed
        public virtual void Tick(double deltaTime)
        {
            if (isSpent) return;

            if (!finite) return;
            
            this.duration -= deltaTime;

            if (this.duration <= 0 && finite)
                isSpent = true;
        }

        //called immediately before the effect is removed
        public abstract void OnEnd();

        public abstract EffectType GetEffectType();

    }
}