using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public static class GlobalEventManager
{
    public static class OnEnemyDeath{
        static Action<Enemy, float> listenerList;

        public static void AddListener(Action<Enemy, float> listener){
            listenerList += listener;
        }

        public static void RemoveListener(Action<Enemy, float> listener){
            listenerList -= listener;
        }

        public static void Fire(Enemy enemy, float strengthAmount){
            listenerList?.Invoke(enemy, strengthAmount);
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
