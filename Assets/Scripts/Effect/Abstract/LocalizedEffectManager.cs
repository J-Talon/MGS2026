using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Effect.Abstract
{
    public class LocalizedEffectManager<T> where T : EntityEffect
    {
        
        private List<T> _effects;
        private HashSet<EffectType> _activeTypes;

        public LocalizedEffectManager() {
            _effects = new List<T>();
            _activeTypes = new HashSet<EffectType>();
        }

        public void Tick(double deltaTime) {
            EffectCallback((effect) => effect.Tick(deltaTime));
        }

        public void EffectCallback(Action<T> action) {
            int count = _effects.Count;
            for (; count >= 0; count--) {
                T effect = _effects[count];
                action(effect);
            }
        }

        public bool AddEffect(T effect)
        {
            return false;
        }
        
        
        
    }
}