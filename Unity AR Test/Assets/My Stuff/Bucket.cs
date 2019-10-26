using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bucket : MonoBehaviour
{
    ParticleSystem sys;
    AudioSource audio;

    // Start is called before the first frame update
    void Start()
    {
        sys = GetComponentInChildren<ParticleSystem>();
        audio = GetComponentInChildren<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Pop();
        }
    }

    public void Pop()
    {
        sys.Play();
        audio.Play();
    }
}
