using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Valve.VR.InteractionSystem;

public class PlayerController : MonoBehaviour
{
    private float _defaultHeight = 1.85f;

    public float DefaultHeight
    {
        get {
            return _defaultHeight;
        }
    }

    private float _playerHeight = 0.00f;
    private Vector3 _initialScale;
    private Vector3 _initialScale_leftHand;
    private Vector3 _initialScale_rightHand;

    public GameObject player;

    private Vector3 initialPosition;

    UnityEvent OnPlayerDeath = new UnityEvent();

    // Start is called before the first frame update
    void Start() {
        _initialScale = player.transform.localScale;

        if(player == null) {
            player = this.gameObject;
        }

        _playerHeight = DefaultHeight;
        _initialScale_leftHand = Player.instance.leftHand.transform.localScale;
        _initialScale_rightHand = Player.instance.rightHand.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdatePlayermodelHeight(float newHeight) {
        float scale = (DefaultHeight / newHeight);
        scale.ToString("0.00");

        player.transform.localScale = _initialScale * scale;

        _playerHeight = newHeight;

        float offset = DefaultHeight > _playerHeight ? DefaultHeight - _playerHeight : 0.0f;

        gameObject.transform.position += new Vector3(0.0f, offset, 0.0f);

        //Inverse scaling for hands
        Player.instance.leftHand.gameObject.transform.localScale = _initialScale_leftHand / scale;
        Player.instance.rightHand.gameObject.transform.localScale = _initialScale_rightHand / scale;
    }
}
