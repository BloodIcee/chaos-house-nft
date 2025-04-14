using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChaosHouse
{
    public class HairBonesView : MonoBehaviour
    {
        [SerializeField] private CharacterView _characterView;
        [SerializeField] private string _hairName;
        
        private List<string> _bonesNames = new List<string>();
        private List<Transform> _bonesTransformList = new List<Transform>();

        public CharacterView CharacterViewValue => _characterView;
        public string HairName => _hairName;
        public List<string> GetAllBonesName()
        {
            _bonesNames = new List<string>();

            AddChildNames(transform);

            return _bonesNames;
        }

        public List<Transform> GetBonesTransforms(List<string> bonesNames)
        {
            _bonesTransformList = new List<Transform>();

            AddChildTransform(transform, bonesNames);

            return _bonesTransformList;
        }

        private void AddChildNames(Transform child)
        {
            foreach (Transform newChild in child)
            {
                _bonesNames.Add(newChild.name);
                if (newChild.childCount > 0)
                {
                    AddChildNames(newChild);
                }
            }
        }

        private void AddChildTransform(Transform child, List<string> bonesNames)
        {
            foreach (Transform newChild in child)
            {
                if (bonesNames.Contains(newChild.name))
                    _bonesTransformList.Add(newChild);

                if (newChild.childCount > 0)
                {
                    AddChildTransform(newChild, bonesNames);
                }
            }
        }
    }
}
