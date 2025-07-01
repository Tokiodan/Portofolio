using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public CardData[] playerDeck;
    public GameObject cardPrefab;
    public Transform playerHandArea;


    public Vector2 handCardSize = new Vector2(200, 300); 

    void Start()
    {
        DrawStartingHand();
    }

    void DrawStartingHand()
    {
       
        for (int i = 0; i < 3; i++) 
        {
            SpawnCard(playerDeck[Random.Range(0, playerDeck.Length)], playerHandArea);
        }

    }

    void SpawnCard(CardData card, Transform handArea)
    {
        GameObject newCard = Instantiate(cardPrefab, handArea);
        newCard.GetComponent<CardDisplay>().SetCard(card);

        
        RectTransform rt = newCard.GetComponent<RectTransform>();
        rt.sizeDelta = handCardSize;
    }
}
