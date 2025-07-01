using UnityEngine;

public class ButtonChangeLightColor : MonoBehaviour
{
    [Header("Button Settings")]
    [SerializeField] private Light directionalLight; // Reference to the directional light
    [SerializeField] private Color[] targetColors; // Array of colors to cycle through
    [SerializeField] private float colorChangeSpeed = 1f; // Speed at which the light color changes

    private int currentColorIndex = 0;

    void OnMouseDown()
    {
        ChangeLightColor();
    }

    private void ChangeLightColor()
    {
        if (directionalLight != null && targetColors.Length > 0)
        {
            Color targetColor = targetColors[currentColorIndex];
            StartCoroutine(ChangeColorCoroutine(targetColor));
            currentColorIndex = (currentColorIndex + 1) % targetColors.Length;
        }
        else
        {
            Debug.LogWarning("Directional Light is not assigned or no target colors specified.");
        }
    }

    private System.Collections.IEnumerator ChangeColorCoroutine(Color targetColor)
    {
        Color initialColor = directionalLight.color;
        float elapsedTime = 0f;

        while (elapsedTime < colorChangeSpeed)
        {
            directionalLight.color = Color.Lerp(initialColor, targetColor, elapsedTime / colorChangeSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        directionalLight.color = targetColor;
    }
}
