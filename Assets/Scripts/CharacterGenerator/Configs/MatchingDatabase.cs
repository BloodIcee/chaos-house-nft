using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

namespace ChaosHouse
{
    [CreateAssetMenu(fileName = "MatchingDatabase", menuName = "Configs/MatchingDatabase")]
    public class MatchingDatabase : ScriptableObject
    {        
        [field: SerializeField] public List<IHaveMatching> matchings { get; private set; }
    
        public T GetMatchingConfig<T>()where T :IHaveMatching
        {
            foreach (var matching in matchings)
            {
                if (matching is T) return (T)matching;
            }

            return null;
        }


        public T GetMatchingConfig<T>(eCharacterGender gender) where T : IHaveMatching
        {
            foreach (var matching in matchings)
            {
                if (matching is T && matching.Gender == gender) return (T)matching;
            }

            return null;
        }

     
    }
}
