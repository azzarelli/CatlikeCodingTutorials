using UnityEngine;

public class Graph : MonoBehaviour
{

    [SerializeField]
    Transform pointPrefab;

    [SerializeField, Range(10,100)]
    int resolution = 10;

    Transform[] points;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {   
        points = new Transform[resolution];

        Transform point = pointPrefab;

        float step = 2f / resolution;
        var scale= Vector3.one * step;
        var position = Vector3.zero;
        for (int i = 0; i < points.Length; i++){
            if (i != 0){
                point = Instantiate(pointPrefab);
            }

            points[i] = point;

            position.x = (i+0.5f) * step - 1f;
            // In the static graph we can leave this here (during Start/Awake)
            // position.y = position.x*position.x;

            point.localPosition = position;
            point.localScale = scale;

            point.SetParent(transform, false);
        }        
    } 

    // Update is called once per frame
    void Update()
    {
        float time = Time.time;
        for (int i = 0; i < points.Length; i++){
            Transform point = points[i];
            Vector3 position = point.localPosition;
			position.y = Mathf.Sin(Mathf.PI * (position.x + time));
            point.localPosition = position;


        }   
    }
}
