using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleRotation : MonoBehaviour
{
    private Vector3 _vector3;
    public bool rotate = true;
    public float time = 5f;
    // Start is called before the first frame update
    void Start()
    {
        if (rotate)
            _vector3.Set(0, 0, 360f);
        else
            _vector3.Set(0, 0, -360f);
        transform.DOLocalRotate(_vector3, time, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
