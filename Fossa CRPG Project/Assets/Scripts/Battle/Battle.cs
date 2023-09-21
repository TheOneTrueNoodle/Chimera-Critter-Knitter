using UnityEngine;

[CreateAssetMenu(fileName = "New Battle", menuName = "Battle")]
public class Battle : ScriptableObject
{
    public string enemyName; //display name
    public GameObject battlePrefab;

    public int enemyHealth;
    public int maxEnemyHealth;

}
