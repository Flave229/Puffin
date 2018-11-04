using UnityEngine;

namespace Assets.Scripts.JoshHackPad
{
	public class PlayerController2D : MonoBehaviour
	{
		[SerializeField] private Rigidbody2D _playerBody;
		[SerializeField] private Transform _playerRectTransform;

		[SerializeField] private float _movementSpeed = 30.0f;
		[SerializeField] private float _jumpForce = 10.0f;
		[SerializeField] private float _playerBottom = 5.0f;

		private bool _isGrounded;

		// Use this for initialization
		void Start () {
		
		}

		private void FixedUpdate()
		{
			float horizontalMovement = Input.GetAxis("Horizontal");
			float verticalMovement = Input.GetAxis("Vertical");

			_playerBody.velocity = new Vector2(horizontalMovement * _movementSpeed, _playerBody.velocity.y);
		}

		// Update is called once per frame
		void Update ()
		{
			/*var playerCentre = _playerRectTransform.position;
			Debug.DrawLine(_playerRectTransform.position, new Vector3(playerCentre.x, playerCentre.y - _playerBottom));*/

			if (Input.GetKeyDown(KeyCode.Space))
			{
				RaycastHit2D[] hits = Physics2D.RaycastAll(_playerRectTransform.position, Vector2.down, _playerBottom);
				foreach (var raycastHit2D in hits)
				{
					if (raycastHit2D.collider != _playerBody.GetComponent<Collider2D>())
					{
						//if (hit.collider != _playerBody.GetComponent<Collider2D>())
						_playerBody.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
					}
				}

				
			}
		}
	}
}
