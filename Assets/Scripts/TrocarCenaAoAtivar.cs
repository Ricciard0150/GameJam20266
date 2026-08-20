using UnityEngine;
using UnityEngine.SceneManagement;

public class TrocarCenaAoAtivar : MonoBehaviour
{
    [SerializeField] private string nomeDaCena;

    private void OnEnable()
    {
        SceneManager.LoadScene(nomeDaCena);
    }
}