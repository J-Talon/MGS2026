using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Effect.Abstract
{
    //@author Talon J
    //this is kinda like the event system we have but localized
    public class LocalizedEffectManager<T> where T : Effect
    {

        private List<T> _effects; // list of active effects on the entity
        private HashSet<EffectType> _activeTypes;  //set of active effect types for quick contains accesses (dicts are overrated)

        public LocalizedEffectManager()
        {
            _effects = new List<T>();
            _activeTypes = new HashSet<EffectType>();
        }

        //calls the tick method on all of the effects and removes spent effects
        public void Tick(double deltaTime) {
            this.RemoveSpent();
            EffectCallback((effect) =>
            {
                effect.Tick(deltaTime);
            });
            
        }

        //removes effects that are spent (basically their time duration has gone to zero)
        private void RemoveSpent() {
            int count = _effects.Count - 1;
            for (; count >= 0; count--) {
                T effect = _effects[count];
                
                EffectType effectType = effect.GetEffectType(); 
                
                if (effect.IsSpent()) {
                    effect.OnEnd();
                    _effects.RemoveAt(count);
                    _activeTypes.Remove(effectType);
                }
            }
        }
        
        
        /*
         * @Author Talon J
         * so essentially, there's 2 ways of doing things
         * you can either create a gigantic for loop to iterate through all of the effects each time you wanna do things...
         * or do this. And then pass what's essentially a lambda into the function to handle it
         *
         * see: Consumer pattern
         */
        public void EffectCallback(Action<T> action) {
            int count = _effects.Count - 1;
            for (; count >= 0; count--) {
                T effect = _effects[count];
                action(effect);
            }
        }

        
        /* adds an effect to the list of effects
         * returns false: if the effect wasn't added, true if it was
         */
        public bool AddEffect(T effect) {
            if (_activeTypes.Contains(effect.GetEffectType())) 
                return false;
            
            effect.OnStart();
            _activeTypes.Add(effect.GetEffectType());
            _effects.Add(effect);
            return true;
        }

        
        //self explanatory. I think. Well I hope. If you don't know what this does you probably 
        //shouldn't be touching it. 
        //I'm probably writing more than I should here actually.
        //wow so much commenting for 2 lines of code.
        //wait no it's 3 lines.
        //well see actually if you put it on 1 line, it's actually 1 line of code. still valid syntax. Will I yell at myself for it? Yes.
        //should you do it? No. 
        // (I swear if I see any of you do this- )
        //...
        //okay fine what this does is check if the entity has a given effect, it returns true if it does, false if it doesn't.
        public bool HasEffect(EffectType effectType) {
            return _activeTypes.Contains(effectType);
        }
        
        
        //returns a capture of ? extends Effect
        //given an effect type
        //(see this is the benefit of having an enum for both.
        //the bad thing about having an enum for both is then the dooks who don't use it right then can cause nightmarish bugs)
        //Am I gonna change it despite knowing that? :P no. they're the ones debugging it.
        [CanBeNull]
        public T GetEffect(EffectType effectType) {
            if (!HasEffect(effectType))
                return null;
            
            for (int index = 0; index < _effects.Count; index++) {
                T effect = _effects[index];
                if (effect.GetEffectType() == effectType) {
                    return effect;
                }
            }
            return null;
        }
        

        //yeah not the most ideal
        //but it's this or 2 iterations each tick going through the dict
        public bool RemoveEffect(EffectType type) {
            
            if (!_activeTypes.Contains(type))
                return true;
            
            for (int index = 0; index < _effects.Count; index++) {
                T effect = _effects[index];
                if (effect.GetEffectType() == type) {
                    effect.OnEnd();
                    _effects.RemoveAt(index);
                    _activeTypes.Remove(type);
                    return true;
                }
            }
            return false;
        }


    }
}