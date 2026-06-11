using UnityEngine;

public class StartScene : MonoBehaviour
{
    bool isStart = true;
    public SpriteRenderer sr;
    Color color;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        color = sr.color;
        isStart = true;
    }
    void Start()
    {
        isStart = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isStart)
        {
            color.a -= 0.2f * Time.deltaTime;
            sr.color = color;

        }

        if(color.a <= 0f)
        {
            isStart = false;
        }
    }
}
