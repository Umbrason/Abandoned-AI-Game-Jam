using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureWorldRepresentation : MonoBehaviour
{
    [SerializeField] private GameObject buildVFX;
    [SerializeField] private GameObject preview;
    [SerializeField] private GameObject realDeal;

    public void EnablePreview()
    {
        preview.SetActive(true);
    }

    public void DisablePreview()
    {
        preview.SetActive(false);
    }
    public void Show()
    {

        realDeal.SetActive(true);
    }
}
