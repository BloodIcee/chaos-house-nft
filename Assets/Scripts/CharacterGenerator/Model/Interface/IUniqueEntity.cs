using UnityEngine;

namespace ChaosHouse
{
    public interface IUniqueEntity
    {
        string EntityName { get; }
        string GetID();        
    }
}
