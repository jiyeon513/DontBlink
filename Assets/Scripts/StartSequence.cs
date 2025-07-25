using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class StartSequence : MonoBehaviour
{
    public Animator dollAnimator;

    public void StartGame()
    {
        Debug.Log("StartZoom 트리거 실행됨!");
        dollAnimator.SetTrigger("StartZoom");

        StartCoroutine(WaitAndLoadScene());
    }

    private IEnumerator WaitAndLoadScene()
    {
        yield return new WaitForSeconds(0.5f);

        SceneManager.LoadScene("GameScene"); 
    }
}
