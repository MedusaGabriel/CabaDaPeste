using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class HistoriaLoadingController : MonoBehaviour
{
    public string proximaCena = "Level 1";
    public TextMeshProUGUI mensagemPressioneTecla;
    public float tempoMinimoLoading = 2f;

    private bool carregando = true;
    private AsyncOperation asyncOp;

    void Start()
    {
        mensagemPressioneTecla.gameObject.SetActive(false);
        StartCoroutine(CarregarCena());
    }

    IEnumerator CarregarCena()
    {
        asyncOp = SceneManager.LoadSceneAsync(proximaCena);
        asyncOp.allowSceneActivation = false;

        yield return new WaitForSeconds(tempoMinimoLoading);

        while (!asyncOp.isDone && asyncOp.progress < 0.9f)
            yield return null;

        carregando = false;
        mensagemPressioneTecla.gameObject.SetActive(true);
    }

    void Update()
    {
        if (!carregando && Input.anyKeyDown)
        {
            asyncOp.allowSceneActivation = true;
        }
    }
}