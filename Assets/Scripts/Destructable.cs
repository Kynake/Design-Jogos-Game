﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructable : MonoBehaviour
{

    public float damagePerSecond;
    public float cooldownPerSecond;
    public Vector2 startingVelocity;
    public float startingRotation;
    public LayerMask thrustersLayer;
    public LayerMask collideScoreLayers;

    private Rigidbody2D _rigidbody;
    private SpriteRenderer _sprite;
    private bool _isBeingDestroyed = false;
    private float _destroyedAmount = 0; // from 0 to 1

    private float _baseRed;
    private const float _maxRed = 1;

    private static GameObject gameControllerObject = null;
    private static GameController gameController = null;

    private void Awake()
    {
        _sprite = GetComponent<SpriteRenderer>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        _baseRed = _sprite.color.r;

        // Get gameController reference to update game stats
        if(gameControllerObject == null) {
            gameControllerObject = GameObject.Find("Canvas");
        }

        if(gameControllerObject != null && gameController == null) {
            gameController = gameControllerObject.GetComponent<GameController>();
        }
    }

    protected virtual void OnEnable()
    {
        if(startingRotation != 0) {
            _rigidbody.AddTorque(startingRotation * _rigidbody.mass);
        }
        if(startingVelocity != Vector2.zero) {
            _rigidbody.AddForce(startingVelocity * _rigidbody.mass, ForceMode2D.Impulse);
        }
    }

    private void Update()
    {
        // if(_destroyedAmount == 1) {
        //     _isBeingDestroyed = false;
        // } else if(_destroyedAmount == 0) {
        //     _isBeingDestroyed = true;
        // }


        if(_isBeingDestroyed) {
            _destroyedAmount += damagePerSecond * Time.deltaTime;
        } else {
            _destroyedAmount -= damagePerSecond * Time.deltaTime;
        }

        _destroyedAmount = Mathf.Clamp01(_destroyedAmount);
        var newColor = _sprite.color;
        newColor.g = 1 - _destroyedAmount;
        newColor.b = 1 - _destroyedAmount;
        _sprite.color = newColor;

        if(_destroyedAmount == 1) {
            gameObject.SetActive(false);
            increaseDestructionStat();
        }
    }

    private void OnTriggerStay2D(Collider2D collider) => _isBeingDestroyed = (collider.gameObject.layer.toLayerMask() & thrustersLayer) != 0;
    private void OnTriggerExit2D(Collider2D collider) => _isBeingDestroyed = (collider.gameObject.layer.toLayerMask() & thrustersLayer) == 0;

    protected virtual void OnCollisionEnter2D(Collision2D collision) {
        var layer = collision.gameObject.layer.toLayerMask();
        if((layer & collideScoreLayers) != 0) {
            GameController.debrisCollisions++;
        }
    }

    protected virtual void increaseDestructionStat() {
        GameController.debrisDestroyed++;
    }
}
