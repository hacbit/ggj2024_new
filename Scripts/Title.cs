using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Title : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        TitilShake();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TitilShake()
    {
        transform.DOMove(transform.position + new Vector3(0, -3, 1), 1f).SetEase(Ease.InOutSine).SetLoops(2, LoopType.Yoyo);

        transform.DOShakeRotation(4f, new Vector3(1, 1, 1), 10, 180, false).SetLoops(-1, LoopType.Incremental);

        transform.DOShakePosition(4f, new Vector3(1, 1, 1), 10, 180, false).SetLoops(-1, LoopType.Incremental);
    }
}
