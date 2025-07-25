using UnityEngine;
using TMPro;
using System.Collections;

public class TitleSequence : MonoBehaviour
{
    public TextMeshProUGUI dontText;
    public TextMeshProUGUI blinkText;

    public float typeSpeed = 0.1f;
    public float delayBeforeBlink = 1.5f;

    void Start()
    {
        StartCoroutine(PlaySequence());
    }

    IEnumerator PlaySequence()
    {
        string fullText = "Don't...";
        dontText.text = "";

        foreach (char c in fullText)
        {
            dontText.text += c;
            yield return new WaitForSeconds(typeSpeed);
        }

        yield return new WaitForSeconds(delayBeforeBlink);

        blinkText.gameObject.SetActive(true);
        Animator anim = blinkText.GetComponent<Animator>();
        if (anim != null)
            anim.SetTrigger("Glitch");
    }
}
