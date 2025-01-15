using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField]
    private Dialog dialog;

    private const string TUTORIAL_URL = "https://example.com/tutorial";

    [SerializeField]
    private SaveController saveController;

    private void Awake()
    {
        saveController.OnInitialized += () => gameObject.SetActive(true);
    }

    private void Start()
    {
        if (dialog != null)
        {
            dialog.OnDialogResult += OnDialogResult;
        }
    }

    private void OnDialogResult(bool result)
    {
        if (result)
        {
            Application.OpenURL(TUTORIAL_URL);
        }
    }

    private void OnDestroy()
    {
        if (dialog != null)
        {
            dialog.OnDialogResult -= OnDialogResult;
        }
    }
}
