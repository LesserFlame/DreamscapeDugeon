using System.Collections;
using System.Collections.Generic;
//using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreen : Singleton<LoadingScreen>
{
    //public GameObject loadingScreen;
    [SerializeField] private SliderTextHandler loadingBar;
    [SerializeField] private Animator animator;
    private float animationLength = 0;
    //private bool isFading = false;
    private int currentSceneID = 0;

    private void Start()
    {
        if (animator != null) animationLength = animator.GetCurrentAnimatorClipInfo(0).Length;
    }
    public void LoadScene(int sceneId)
    {
        //isFading = true;
        
        currentSceneID = sceneId;
        animator.SetBool("IsOpen", true);
        Invoke("LoadScene", animationLength);
    }
    private void LoadScene()
    {
        if (MusicManager.Instance != null) { MusicManager.Instance.FadeSong(animationLength); }
       
        loadingBar.SetValue(0, 1);
        StartCoroutine(LoadSceneAsync(currentSceneID));
    }
    IEnumerator LoadSceneAsync(int sceneId)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);
        //loadingScreen.SetActive(true);

        
        while (!operation.isDone) 
        {
            float progressValue = Mathf.Clamp01(operation.progress / 0.9f);
            //Debug.Log(progressValue);
            loadingBar.OnValueChanged(progressValue, 1);
            yield return null;
        }

        if (operation.isDone)
        {
            //isFading = true;
            animator.SetBool("IsOpen", false);
        }
    }
}
