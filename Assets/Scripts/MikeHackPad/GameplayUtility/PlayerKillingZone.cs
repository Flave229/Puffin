using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.MikeHackPad.GameplayUtility
{
	public class PlayerKillingZone : MonoBehaviour
	{
		public void Start()
		{

		}

		public void Update()
		{

		}

		private void OnDrawGizmos()
		{
			Gizmos.color = new Color(0.5f, 0.0f, 0.0f, 0.75f);
			Gizmos.DrawWireCube(transform.position, transform.localScale);
		}

		private void OnDrawGizmosSelected()
		{
			Gizmos.color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
			Gizmos.DrawCube(transform.position, transform.localScale);
		}

		private void OnTriggerEnter(Collider other)
		{
			if (other.tag == "Player")
			{
				SceneManager.LoadScene(SceneManager.GetActiveScene().name);
			}
		}
	}
}