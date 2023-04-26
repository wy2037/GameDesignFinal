using UnityEngine;

public class BlinkingSprite : MonoBehaviour
{
    public float blinkInterval = 0.5f;

    private SpriteRenderer spriteRenderer;
    private float timePassed = 0f;
    private bool isVisible = true;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        timePassed += Time.deltaTime;

        if (timePassed >= blinkInterval)
        {
            isVisible = !isVisible;
            spriteRenderer.enabled = isVisible;
            timePassed = 0f;
        }
    }
}
