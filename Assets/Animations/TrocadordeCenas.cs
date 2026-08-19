using UnityEngine;
using UnityEngine.SceneManagement;

public class TrocadorDeCenas : MonoBehaviour
{

    public void IrParaCena(string nomeDaCena)
    {
        SceneManager.LoadScene(nomeDaCena);
    }

    
}