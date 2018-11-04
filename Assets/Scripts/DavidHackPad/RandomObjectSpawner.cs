using UnityEngine;

namespace Assets.Scripts.DavidHackPad
{
	public class RandomObjectSpawner : MonoBehaviour
	{
		[SerializeField]
		private GameObject ObjectToSpawn;
		[SerializeField]
		private float SpawnRateInSeconds;

		private float _timeSinceLastSpawn;
		private float _minXBound;
		private float _maxXBound;
		private float _minZBound;
		private float _maxZBound;

		void Awake()
		{
			_timeSinceLastSpawn = 0;
		}

		void Start()
		{
			_minXBound = transform.position.x - (transform.localScale.x / 2);
			_maxXBound = transform.position.x + (transform.localScale.x / 2);
			_minZBound = transform.position.z - (transform.localScale.z / 2);
			_maxZBound = transform.position.z + (transform.localScale.z / 2);
		}

		void Update()
		{
			_timeSinceLastSpawn += Time.deltaTime;

			if (_timeSinceLastSpawn < SpawnRateInSeconds)
			{
				return;
			}

			_timeSinceLastSpawn = 0;

			System.Random randomGenerator = new System.Random();
			Vector3 randomPosition = new Vector3(randomGenerator.Next((int)_minXBound, (int)_maxXBound), transform.position.y, randomGenerator.Next((int)_minZBound, (int)_maxZBound));

			Instantiate(ObjectToSpawn, randomPosition, Quaternion.identity);
		}
	}
}