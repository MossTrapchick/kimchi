using UnityEngine;

[CreateAssetMenu(fileName = "New character", menuName = "Character")]
public class Character : ScriptableObject
{
    [SerializeField] public string Name;
    [SerializeField] public GameObject prefab;
    [SerializeField] public int health;
    [SerializeField] public int basicDamage;
    [SerializeField] public int spechialDamage;
    [SerializeField] public int ultDamage;
    [SerializeField] public Sprite Icon;
    [SerializeField] public float stunTime;
}
