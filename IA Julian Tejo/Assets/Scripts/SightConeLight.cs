using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SightConeLight : MonoBehaviour
{
    [SerializeField] private Light _light;
    [SerializeField] private Enemy _enemy;
    private void Awake()
    {
        _light.range = _enemy.range;
        _light.spotAngle = _enemy.angle;
    }
    private void Update()
    {
        if (_enemy.IsplayerDetected && !_enemy.isabelle.IsDead)
        {
            _light.color = Color.red;
        }
        else
        {
            _light.color = Color.yellow;
        }
    }
}
