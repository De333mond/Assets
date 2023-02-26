using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character
{

	public class CameraFollow : MonoBehaviour
	{
		public float FollowSpeed = 2f;
		public Transform Target;
		
		private Transform camTransform;

		public float shakeDuration = 0f;

		public float shakeAmount = 0.1f;
		public float decreaseFactor = 1.0f;
		[SerializeField] private Vector3 _PositionOffset;

		
		Vector3 originalPos;

		void Awake()
		{
			Cursor.visible = false;
			if (camTransform == null)
			{
				camTransform = GetComponent(typeof(Transform)) as Transform;
			}

			camTransform.position = Target.position;
		}

		void OnEnable()
		{
			originalPos = camTransform.localPosition;
		}

		private void Update()
		{
			transform.position = Vector3.Slerp(transform.position, Target.position + _PositionOffset, FollowSpeed * Time.deltaTime);

			if (shakeDuration > 0)
			{
				camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;

				shakeDuration -= Time.deltaTime * decreaseFactor;
			}
		}

		public void ShakeCamera()
		{
			originalPos = camTransform.localPosition;
			shakeDuration = 0.2f;
		}
	}
}