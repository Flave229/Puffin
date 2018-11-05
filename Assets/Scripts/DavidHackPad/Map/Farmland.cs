using UnityEngine;
using Assets.Scripts.DavidHackPad.AI.Desires;

namespace Assets.Scripts.DavidHackPad.Map
{
	[RequireComponent(typeof(Food))]
	public class Farmland : MonoBehaviour
	{
		private Food _foodComponent;
		private Vector3 _startingScale;

		void Awake()
		{
			_foodComponent = GetComponent<Food>();
		}

		void Start()
		{
			_startingScale = transform.localScale;
		}

		void Update()
		{
			float scaleMultiplier = _foodComponent.GetRemainingCalories() / _foodComponent.GetStartingCalories();

			if (scaleMultiplier < 0.1f)
			{
				scaleMultiplier = 0.1f;
			}

			transform.localScale = new Vector3(transform.localScale.x, _startingScale.y * scaleMultiplier, transform.localScale.z);
		}
	}
}