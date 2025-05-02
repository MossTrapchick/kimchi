using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HelthMnager : MonoBehaviour
{
    private int healt;
    private int curentHeakth;

    [SerializeField] Players player;

    [SerializeField] Slider mainSlider;



    private void Start()
    {
        healt = player.health;
        curentHeakth = healt;
        mainSlider.maxValue = player.health;
        mainSlider.value = player.health;
    }

    public void testDamage()
    {
        addDamage(5);
    }

    private void addDamage(int damage)
    {
        curentHeakth -= damage;
        mainSlider.value = curentHeakth;
    
    }
}
