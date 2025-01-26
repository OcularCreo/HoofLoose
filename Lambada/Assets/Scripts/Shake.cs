using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Shake : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] bool shake;

    private Vector3 starPos;

    // Start is called before the first frame update
    void Start()
    {
        starPos = transform.position;     
    }

    // Update is called once per frame
    void Update()
    {
        int sequenceIdx = gameManager.combo / 8;
        
        if(sequenceIdx > 1)
        {
            transform.position = starPos + (Random.insideUnitSphere * Mathf.Clamp(1.25f * sequenceIdx, 1.25f, 3f));
        } else
        {
            transform.position = starPos;
        }
    }

    public void startShake()
    {
        shake = true;
    }

    public void stopShake()
    {
        shake = false; 
        transform.position = starPos;
    }
}
