using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.MikeHackPad
{
	public enum ECharacterSprite
	{
		IS_STANDING,
		IS_WALKING,
		IS_RUNNING,
		IS_JUMPING,
		IS_FALLING,
	}

	public class CharacterSpriteRenderer : MonoBehaviour
	{
		[SerializeField]
		private SpriteRenderer _standingSprite;
		[SerializeField]
		private SpriteRenderer _walkingSprite;
		[SerializeField]
		private SpriteRenderer _runningSprite;
		[SerializeField]
		private SpriteRenderer _jumpingSprite;
		[SerializeField]
		private SpriteRenderer _fallingSprite;

		public ECharacterSprite _currentSprite;
		public bool _isXFlipped;

		// Use this for initialization
		void Start()
		{
			_currentSprite = ECharacterSprite.IS_STANDING;
		}

		// Update is called once per frame
		void LateUpdate()
		{
			_standingSprite.enabled = _currentSprite == ECharacterSprite.IS_STANDING;
			_walkingSprite.enabled = _currentSprite == ECharacterSprite.IS_WALKING;
			_runningSprite.enabled = _currentSprite == ECharacterSprite.IS_RUNNING;
			_jumpingSprite.enabled = _currentSprite == ECharacterSprite.IS_JUMPING;
			_fallingSprite.enabled = _currentSprite == ECharacterSprite.IS_FALLING;

			_standingSprite.flipX = _isXFlipped;
			_walkingSprite.flipX = _isXFlipped;
			_runningSprite.flipX = _isXFlipped;
			_jumpingSprite.flipX = _isXFlipped;
			_fallingSprite.flipX = _isXFlipped;
		}
	}
}
