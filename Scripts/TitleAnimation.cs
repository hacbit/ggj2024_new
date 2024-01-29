using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        TitleMove();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TitleMove()
    {
        transform.DOMove(transform.position + new Vector3(0, -3, 1), 1f).SetEase(Ease.InOutSine).SetLoops(2, LoopType.Yoyo);
        
    }
}
