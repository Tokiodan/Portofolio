using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardDisplay : MonoBehaviour
{
    public CardData cardData;

    public TMP_Text cardHPText;
    public TMP_Text cardAttackText;
    public TMP_Text waterCostText;
    public TMP_Text meatCostText;
    public Image cardArtwork;

    public Vector2 textPositionOffset = new Vector2(10, 10); 

    public void SetCard(CardData newCard)
    {
        cardData = newCard;
        UpdateCardUI();
    }

    void UpdateCardUI()
    {
        if (cardData != null)
        {
            cardHPText.text = cardData.health.ToString();
            cardAttackText.text = cardData.attack.ToString();
            waterCostText.text = cardData.waterCost.ToString();
            meatCostText.text = cardData.meatCost.ToString();
            cardArtwork.sprite = cardData.artwork;

            
            SetTextFixedPosition(cardHPText);
            SetTextFixedPosition(cardAttackText);
            SetTextFixedPosition(waterCostText);
            SetTextFixedPosition(meatCostText);
            SetImageFixedSize(cardArtwork);
        }
    }

    void SetTextFixedPosition(TMP_Text textComponent)
    {
        
        RectTransform rt = textComponent.GetComponent<RectTransform>();
        rt.localPosition = textPositionOffset; 
        rt.localScale = Vector3.one;
    }

    void SetImageFixedSize(Image imageComponent)
    {
        RectTransform rt = imageComponent.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(150, 200); 
    }
}
