using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public static class GlobalEventManager
{
    public static class OnEnemyDeath{
        static Action<float> listenerList;

        public static void AddListener(Action<float> listener){
            listenerList += listener;
        }

        public static void RemoveListener(Action<float> listener){
            listenerList -= listener;
        }

        public static void Fire(float strengthAmount){
            listenerList?.Invoke(strengthAmount);
        }
    }
    
    public static class OnExtraDeath{
        static Action listenerList;

        public static void AddListener(Action listener){
            listenerList += listener;
        }

        public static void RemoveListener(Action listener){
            listenerList -= listener;
        }

        public static void Fire(){
            listenerList?.Invoke();
        }
    }
}
