﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PoolSound : MonoBehaviour {

    [SerializeField]
    PoolManager.PoolType type;
    AudioSource mAudioSource = null;
    float mClipLength = 0.0f;
    
	void Awake () {
        mAudioSource = GetComponent<AudioSource>();
        mClipLength = mAudioSource.clip.length;
    }

    public void Play()
    {
        mAudioSource.Play();
        StartCoroutine(StartMethod(mClipLength));
    }

    private IEnumerator StartMethod(float clipLength)
    {
        yield return new WaitForSeconds(clipLength);

        PoolManager.Instance.ReturnPoolElement(type, this.gameObject);

    }

}