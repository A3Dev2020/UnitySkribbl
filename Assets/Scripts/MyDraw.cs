using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyDraw : MonoBehaviour
{
    public GameObject Trait;
    public float Threshold;
    public float InterpolMult = 10f;
    private float _threshold = 400;
    private bool Hold;
    private bool Started;
    private Vector3 LastPosition;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Hold = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            Hold = false;
            Started = false;
        }

        if (Hold && Threshold <= _threshold)
        {
            int count = 0;
            Vector3 v = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            int interpolationSteps = 1;

            if (Started)
            {
                float x = Mathf.Abs(LastPosition.x - v.x);
                float y = Mathf.Abs(LastPosition.y - v.y);

                interpolationSteps = (int)((x + y) * InterpolMult);

                Debug.Log(interpolationSteps);
            }


            while (Started && count < interpolationSteps)
            {
                float step = (count / (float)interpolationSteps);
                Vector3 pos = Vector3.Lerp(LastPosition, v, step);

                pos[2] = -0.1f;
                InstantiateTrait(pos);
                _threshold = 0;

                ++count;
            }

            Started = true;

            _threshold = 0;
            LastPosition = v;
        }

        _threshold += Time.deltaTime;
    }

    private void InstantiateTrait(Vector3 position)
    {
        GameObject obj = Instantiate(Trait);

        obj.transform.position = position;
    }
}
