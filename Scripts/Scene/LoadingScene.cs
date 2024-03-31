using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class LoadingScene : BaseScene
{
    [SerializeField] private TextMeshProUGUI uiText;
    private static string _nextScene;

    [SerializeField]
    private Image _loadingBar;


    /*
     
    슬라이더
        [SerializeField] 
        Slider _loadingBar;
        _loadingBar.value = op.progress;
     
     */

    protected void Start()
    {
        //  base.Start(); TODO

        uiText.text = Define.loadingComment[Random.Range(0, Define.loadingComment.Length)];
        StartCoroutine(LoadSceneProcess());

    }


    public static void LoadScene(string sceneName)   //  LoadingScene.LoadScene("MainScene"); 으로 실행
    {
        // TODO 현재씬 리소스 언로드, 다음씬 리소스 로드

        _nextScene = sceneName;
        SceneManager.LoadScene(Define.SceneName.Loading);
    }


    private IEnumerator LoadSceneProcess()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(_nextScene);
        op.allowSceneActivation = false;
        float timer = 0f;

        while (!op.isDone)
        {
            yield return null;

            if (op.progress < 0.2f)
            {
                _loadingBar.fillAmount = op.progress;
            }
            else
            {
                timer += Time.unscaledDeltaTime;
                _loadingBar.fillAmount = Mathf.Lerp(0.2f, 1f, timer); // 로딩 너무 빨리 되면 페이크 로딩을 넣어준다.

                if (_loadingBar.fillAmount >= 1f)
                {
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }
}

