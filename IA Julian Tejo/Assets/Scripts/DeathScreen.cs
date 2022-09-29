using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathScreen : MonoBehaviour
{
    [SerializeField] private Isabelle _isabelle;
    [SerializeField] private GameObject _deathScreen;

    private void Awake()
    {
        _deathScreen.SetActive(false);
    }

    private void Update()
    {
        if (_isabelle.IsDead)
        {
            _deathScreen.SetActive(true);
        }
    }
}
