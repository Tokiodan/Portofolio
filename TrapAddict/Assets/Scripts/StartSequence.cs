using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StartSequence : MonoBehaviour
{
    public Image tempBackground;      
    public Image tokiodanPresents;       
    public Image gameBackground;        
    public Image gameTitle;              
    public GameObject buttons;           

    
    public float fadeDuration = 1f;      
    public float delayBeforeTitle = 2f;  
    public float delayBeforeButtons = 1f; 
    public float tempBackgroundHoldTime = 1f; 

    private void Start()
    {
        StartCoroutine(PlayIntroSequence());
    }

    private IEnumerator PlayIntroSequence()
    {

        tokiodanPresents.color = new Color(1, 1, 1, 0); 
        gameTitle.color = new Color(1, 1, 1, 0);       
        tempBackground.color = new Color(tempBackground.color.r, tempBackground.color.g, tempBackground.color.b, 1); 
        gameBackground.gameObject.SetActive(false);    
        buttons.SetActive(false);                     

       
        yield return new WaitForSeconds(tempBackgroundHoldTime);

       
        yield return StartCoroutine(FadeIn(tokiodanPresents));
        yield return new WaitForSeconds(1f); 

        
        yield return StartCoroutine(FadeOut(tempBackground));
        yield return StartCoroutine(FadeOut(tokiodanPresents));

       
        gameBackground.gameObject.SetActive(true);
        yield return StartCoroutine(FadeIn(gameBackground));

    
        yield return new WaitForSeconds(delayBeforeTitle);
        yield return StartCoroutine(FadeIn(gameTitle));

       
        yield return new WaitForSeconds(delayBeforeButtons);
        buttons.SetActive(true);
        yield return StartCoroutine(FadeInButtons());
    }

    private IEnumerator FadeIn(Graphic graphic)
    {
        float elapsedTime = 0;
        Color originalColor = graphic.color;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            graphic.color = new Color(originalColor.r, originalColor.g, originalColor.b, Mathf.Clamp01(elapsedTime / fadeDuration));
            yield return null;
        }
    }

    private IEnumerator FadeOut(Graphic graphic)
    {
        float elapsedTime = 0;
        Color originalColor = graphic.color;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            graphic.color = new Color(originalColor.r, originalColor.g, originalColor.b, Mathf.Clamp01(1 - (elapsedTime / fadeDuration)));
            yield return null;
        }
    }

    private IEnumerator FadeInButtons()
    {
        foreach (Transform button in buttons.transform)
        {
            var graphic = button.GetComponent<Graphic>();
            if (graphic != null)
            {
                graphic.color = new Color(graphic.color.r, graphic.color.g, graphic.color.b, 0);
                yield return StartCoroutine(FadeIn(graphic));
            }
        }
    }
}
