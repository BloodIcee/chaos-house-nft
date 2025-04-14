using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace ChaosHouse
{
    public class ItemSpawner : MonoBehaviour
    {
        [SerializeField] private ItemToFind[] _itemPFBs;
        [SerializeField] private int _minCount;
        [SerializeField] private int _maxCount;
        [SerializeField] private int _radius;

        private List<ItemToFind> _generatedItem = new List<ItemToFind>();
        public int CountItemInStart { get; private set; }
        public int CurrentCountItem => _generatedItem.Count;

        private void OnValidate()
        {
            if (_itemPFBs == null || _itemPFBs.Length == 0) Debug.LogError($"check field: {gameObject.name} -> _itemPFBs");
            if (_minCount > _maxCount || _maxCount < _minCount) Debug.LogError($"check field: {gameObject.name} -> _maxCount ,_minCount ");
        }

        private void Awake()
        {
            Generate();
        }

        private void Generate()
        {
            var r = Random.Range(_minCount, _maxCount+1);
            CountItemInStart = r;
            for (int i = 0; i < r; i++)
            {
                var randomIndex = Random.Range(0, _itemPFBs.Length);
                GameObject generatedObj = Instantiate(_itemPFBs[randomIndex].gameObject);
                var pos = GetRandomPoint(transform.position, _radius);
                pos.y += generatedObj.transform.localScale.y * 0.5f;
                generatedObj.transform.position = pos;
                var item = generatedObj.GetComponent<ItemToFind>();
                item.onFinded = OnTakeItem;
                _generatedItem.Add(item);

            }
        }

        private void OnTakeItem(ItemToFind item)
        {
            _generatedItem.Remove(item);
            if (_generatedItem.Count == 0)
            {
                EventManager.OnEvent(EEventsName.LevelComplite);
            }
        }
        protected Vector3 GetRandomPoint(Vector3 center, float maxDistance)
        {
            Vector3 randomPos = Random.insideUnitSphere * maxDistance + center;
            NavMeshHit hit;
            if (!NavMesh.SamplePosition(randomPos, out hit, maxDistance, NavMesh.AllAreas))
                return GetRandomPoint(center, maxDistance);
            
            return hit.position;
        }

        private Vector3 RandomNavmeshLocation()
        {
            Vector3 randomDirection = Random.insideUnitSphere * _radius;
            randomDirection += transform.position;
            NavMeshHit hit;
            Vector3 finalPosition = Vector3.zero;
            if (NavMesh.SamplePosition(randomDirection, out hit, _radius, 1))
            {
                finalPosition = hit.position;
            }
            return finalPosition;
        }


        public Vector3 GetRandomItemPosition()
        {
            var r = Random.Range(0, _generatedItem.Count);
            return _generatedItem[r].transform.position;
        }
    }
}
