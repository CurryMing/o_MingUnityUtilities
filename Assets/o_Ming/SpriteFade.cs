using System.Collections;
using UnityEngine;

public class SpriteFade : MonoBehaviour
{
    public float spriteFadeTime = 0.01f;

    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (gameObject.activeInHierarchy)
        {
            StartCoroutine(FadeSprite());
        }
    }

    private IEnumerator FadeSprite()
    {
        while (gameObject.activeInHierarchy)
        {
            sr.color = Color.Lerp(sr.color, Color.clear, spriteFadeTime * Time.deltaTime);
            if(sr.color == Color.clear)
            {
                gameObject.SetActive(false);
            }
            yield return null;
        }
        
    }
}
