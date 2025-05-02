using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Player", menuName = "ScriptableObject/player")]
public class Players : ScriptableObject
{
    [SerializeField] int health;
    [SerializeField] int basicDamage;
    [SerializeField] int spechialDamage;
    [SerializeField] int ultDamage;

    [SerializeField] Image Icon;

    [SerializeField] string name;

}
