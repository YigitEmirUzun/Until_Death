using Spine.Unity;
using UnityEngine;

public class NPCDialogue : MonoBehaviour
{
    public Dialogue dialogue;
    public SkeletonAnimation spineAnim;
    public bool talked = false;

    private void Start()
    {
        spineAnim.AnimationState.SetAnimation(0, "Idle", true);
    }

    public void TriggerDialogue()
    {
        if (!talked)
        {
            spineAnim.timeScale = 0.6f;
            FindAnyObjectByType<DialogueManager>().StartDialogue(dialogue);
            spineAnim.AnimationState.SetAnimation(0, "Talking", true);
            talked = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (FindAnyObjectByType<PlayerController>().interact)
            {
                TriggerDialogue();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            FindAnyObjectByType<DialogueManager>().DisplayNextSentence();
            spineAnim.AnimationState.SetAnimation(0, "Idle", true);
            spineAnim.timeScale = 0.1f;
        }
    }
}
