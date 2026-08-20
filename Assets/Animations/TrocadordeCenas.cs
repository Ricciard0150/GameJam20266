using UnityEngine;
using UnityEngine.SceneManagement;

public class TrocadorDeCenas : MonoBehaviour
{
    public void IrParaCena(string nomeDaCena)
    {
        SceneManager.LoadScene(nomeDaCena);
    }

    // Aqui é pra caso você queira ter um botão de sair
    public void SairDoJogo()
    {
        Debug.Log("But kitou..");

#if UNITY_EDITOR
        // Se estiver testando dentro da Unity, para o Play Mode
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // Se for o jogo compilado, fecha o jogo
        Application.Quit();
#endif
    }
}