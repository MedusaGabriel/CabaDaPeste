using UnityEngine;
using System.Collections.Generic;

public class DamagePopUpPool : MonoBehaviour
{
    public CanvasPopUp popupPrefab;
    public int poolSize = 10;
    private List<CanvasPopUp> pool = new List<CanvasPopUp>();

    void Awake()
    {
        for (int i = 0; i < poolSize; i++)
        {
            CanvasPopUp popup = Instantiate(popupPrefab, transform);
            popup.gameObject.SetActive(false);
            pool.Add(popup);
        }
    }

    public CanvasPopUp GetPopUp()
    {
        foreach (var popup in pool)
        {
            if (!popup.gameObject.activeInHierarchy)
                return popup;
        }
        CanvasPopUp popupExtra = Instantiate(popupPrefab, transform);
        popupExtra.gameObject.SetActive(false);
        pool.Add(popupExtra);
        return popupExtra;
    }
}