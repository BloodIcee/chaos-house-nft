
using UnityEngine;
using UnityEngine.AI;

namespace ChaosHouse
{
    public interface AILogic
    {
        bool IsSleep { get; }
        void Update();
        void Disable();
        void Sleep();
        void Stop();        
        void OnTriggerEnter();
        void SpeedUp();
        void SpeedDown();
        void Move();
        void SetPath();
        void Falling();


    }
}
