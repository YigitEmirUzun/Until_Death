using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScene : MonoBehaviour
{
    public string nextScene;
    public GameObject darkScreen;
    public GameObject player;
    public SpriteRenderer sr;
    Color color; 
    bool isSceneEnd = false;
    public AudioSource bgMusic;

    private void Awake()
    {
        player = GameObject.Find("Axolotl");
        sr = darkScreen.GetComponent<SpriteRenderer>();
        color = sr.color;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isSceneEnd = true;
            player.GetComponent<PlayerController>().enabled = false;
        }
    }

    private void Update()
    {
        if (isSceneEnd)
        {
            if (color.a < 1f)
            {
                color.a += 0.3f * Time.deltaTime;
                sr.color = color;
                bgMusic.volume -= 0.1f;
            }
            else
            {
                StartCoroutine(NextSceneRoutine());
            }
        }
    }

    IEnumerator NextSceneRoutine()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(nextScene);
    }


}
