using UnityEngine;

public class SceneFader : MonoBehaviour
{
    public float FadeDuration=1f;

    private int _fadeAmount = Shader.PropertyToID("_Value");
    private int __useShutter = Shader.PropertyToID("_UseShutter");
}
