using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;
using Newtonsoft.Json;
using UnityEngine;
using WebSocketSharp;

public class MyDraw : MonoBehaviour
{
    public GameObject Trait;
    public float Threshold;
    public float InterpolMult = 10f;
    private float _threshold = 400;
    private bool Hold;
    private bool Started;
    private Vector3 LastPosition;

    WebSocket ws;
    public Queue<string> Queue = new Queue<string>();

    // Start is called before the first frame update
    void Start()
    {
        ws = new WebSocket("ws://localhost:8080");
        ws.Connect();
        ws.OnMessage += (sender, e) =>
        { 
            Queue.Enqueue(e.Data);
        };
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 v = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            Hold = true;
            ws.SendJson(new StartEndInput
            {
                Start = true,
                X = v.x,
                Y = v.y
            });
        }

        if (Input.GetMouseButtonUp(0))
        {
            Hold = false;
            Started = false;
            ws.SendJson(new StartEndInput
            {
                Start = false
            });
        }

        if (Hold && Threshold <= _threshold)
        {
            ws.SendJson(new TraitInput
            {
                X = v.x,
                Y = v.y
            });
        }

        TreatQueue();

        _threshold += Time.deltaTime;
    }

    private void TreatQueue()
    {
        if (Queue.Any())
        {
            string e = Queue.Dequeue();

            if (e.Contains("Start"))
            {
                StartEndInput i = JsonConvert.DeserializeObject<StartEndInput>(e);

                if (i.Start)
                {
                    LastPosition = new Vector3(i.X, i.Y);
                    Started = true;
                }
                else
                {
                    Started = false;
                }
            }
            else if (Started)
            {
                TraitInput i = JsonConvert.DeserializeObject<TraitInput>(e);
                int count = 0;

                int interpolationSteps = 1;

                float x = Mathf.Abs(LastPosition.x - i.X);
                float y = Mathf.Abs(LastPosition.y - i.Y);

                interpolationSteps = (int)((x + y) * InterpolMult);

                while (count < interpolationSteps)
                {
                    Vector3 v = i.GetVector();
                    float step = (count / (float)interpolationSteps);
                    Vector3 pos = Vector3.Lerp(LastPosition, v, step);

                    pos[2] = -0.1f;
                    InstantiateTrait(pos);
                    _threshold = 0;

                    ++count;
                }

                Started = true;

                _threshold = 0;
                LastPosition = i.GetVector();
            }
        }
    }

    private void InstantiateTrait(Vector3 position)
    {
        GameObject obj = Instantiate(Trait);

        obj.transform.position = position;
    }
}
