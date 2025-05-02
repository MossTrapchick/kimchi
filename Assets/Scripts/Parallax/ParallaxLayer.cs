using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    [Tooltip("0 = stays at the same position, 1 = follows camera completely")] public float parallaxFactor; 
    private CameraMovementTracker _parallaxCamera;
    private void Awake()
    {
        _parallaxCamera = Camera.main?.GetComponent<CameraMovementTracker>();
    }
    
    private void Start()
    {
        _parallaxCamera?.onCameraMovedOnX.AddListener(Move); 
    }
    
    public void Move(float delta)
    {
        Vector2 newPos = transform.localPosition;
        newPos.x -= delta * parallaxFactor; 
        transform.localPosition = newPos;
    }
}