using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public GameObject camThirdPerson;
    public GameObject camIsometrica;

    private bool isThirdPerson = true;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            isThirdPerson = !isThirdPerson;

            camThirdPerson.SetActive(isThirdPerson);
            camIsometrica.SetActive(!isThirdPerson);
        }
    }
}
