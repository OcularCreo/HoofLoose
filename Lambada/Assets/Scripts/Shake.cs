using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Shake : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private bool alwaysShake;
    [SerializeField] private float shakeSpeed = 1.25f;

    private Vector3 starPos;

    // Start is called before the first frame update
    void Start()
    {
        starPos = transform.position;     
    }

    // Update is called once per frame
    void Update()
    {
        int sequenceIdx = alwaysShake ? 1 : gameManager.combo / 8;
        
        if(sequenceIdx > 0)
        {
            transform.position = starPos + (Random.insideUnitSphere * Mathf.Clamp(shakeSpeed * sequenceIdx, shakeSpeed, 5f));
        } else
        {
            transform.position = starPos;
        }
    }

}
