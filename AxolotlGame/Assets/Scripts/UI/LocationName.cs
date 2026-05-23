using System.Collections;
using TMPro;
using UnityEngine;

public class LocationName : MonoBehaviour
{
    public float startDelay = 1;
    public TextMeshProUGUI textMesh;
    public string Location;

    private void Awake()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        Color color = textMesh.color;
        color.a = 0f;
        textMesh.color = color;
        textMesh.text = Location;
    }

    void Start()
    {
        StartCoroutine(FadeText());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator FadeText()
    {
        yield return new WaitForSeconds(startDelay);
        
        Color color = textMesh.color;

        float timer = 0;

        while (timer < 3)
        {
            timer += Time.deltaTime;

            color.a = Mathf.Lerp(
                0,
                1,
                timer / 3
            );

            textMesh.color = color;

            yield return null;
        }

        yield return new WaitForSeconds(2);
        timer = 0;

        while (timer < 3)
        {
            timer += Time.deltaTime;

            color.a = Mathf.Lerp(
                1,
                0,
                timer / 3
            );

            textMesh.color = color;

            yield return null;
        }
    }

}
