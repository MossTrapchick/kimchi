using UnityEngine;
using UnityEngine.Events;

public class CameraMovementTracker : MonoBehaviour
{
	private float _oldXPosition;
	[HideInInspector] public UnityEvent<float> onCameraMovedOnX;

	private void Start() => _oldXPosition = transform.position.x;

	private void Update()
	{
		if (Mathf.Approximately(transform.position.x, _oldXPosition)) return;
		
		if (onCameraMovedOnX != null)
		{
			var delta = _oldXPosition - transform.position.x;
			onCameraMovedOnX?.Invoke(delta);
		}
		_oldXPosition = transform.position.x;
	}
}