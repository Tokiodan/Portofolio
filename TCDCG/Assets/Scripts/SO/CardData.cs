using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class CardData : ScriptableObject
{
    public string health;
    public string attack;
    public string waterCost;
    public string meatCost;
    public Sprite artwork;
    public string description;
}
