using System.Collections;
using ChaosHouse.AISettings;
using UnityEngine;
using UnityEngine.AI;

namespace ChaosHouse
{
    public abstract class SleepRoyaleBotLogic<T>: AILogic where T : iBotSetting
    {
        public bool IsSleep { get; protected set; }
        public abstract void Disable();
        protected abstract void Init();        
        public abstract void Move();
        public abstract void OnTriggerEnter();
        public abstract void SetPath();
        public abstract void Sleep();
        public abstract void SpeedDown();
        public abstract void SpeedUp();
        public abstract void Stop();
        public abstract void Update();

        protected Vector3 GetRandomPoint(Vector3 center, float maxDistance)
        {
            Vector3 randomPos = Random.insideUnitSphere * maxDistance + center;
            NavMeshHit hit;
            if (!NavMesh.SamplePosition(randomPos, out hit, maxDistance, NavMesh.AllAreas))
                return GetRandomPoint(center, maxDistance);

            return hit.position;
        }

        public abstract void Falling();
    }
}
