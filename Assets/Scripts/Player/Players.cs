using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Player", menuName = "ScriptableObject/player")]
public class Players : ScriptableObject
{
    [SerializeField] public int health;
    [SerializeField] public int basicDamage;
    [SerializeField] public int spechialDamage;
    [SerializeField] public int ultDamage;

    [SerializeField] public Image Icon;

    [SerializeField] public string name;

}
