using UnityEngine;
using static UnityEngine.ParticleSystem;

public class AttackTralController : MonoBehaviour
{
    [SerializeField] GameObject particle;
    public void OnAnimationComplete()
    {

        particle.SetActive(false);

    }


    public void OnAnimationProgress(float progress)
    {

        if (progress >= 0.2f) 
        {
            particle.SetActive(true);
        }else if (progress <= 0.7f)
        {
            particle.SetActive(false);
        }
    }
}
