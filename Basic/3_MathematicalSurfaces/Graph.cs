using UnityEngine;

public class Graph : MonoBehaviour
{

    [SerializeField]
    Transform pointPrefab;

    [SerializeField, Range(10,100)]
    int resolution = 10;

    [SerializeField]
    FunctionLibrary.FunctionName function;

    Transform[] points;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {   
        points = new Transform[resolution * resolution];

        float step = 2f / resolution;
        var scale= Vector3.one * step;


        Transform point = pointPrefab;
        
        for (int i = 0; i < points.Length; i++){
            
            if (i != 0){
                point = Instantiate(pointPrefab);
            }
            points[i] = point;
            point.localScale = scale;
            point.SetParent(transform, false);
        }        
    } 

    // Update is called once per frame
    void Update()
    {
        FunctionLibrary.Function f = FunctionLibrary.GetFunction(function);

        float time = Time.time;
        float step = 2f / resolution;
		float v = 0.5f * step - 1f;
        for (int i = 0, x = 0, z = 0; i < points.Length; i++, x++) {
			if (x == resolution) {
				x = 0;
				z += 1;
                v = (z + 0.5f) * step - 1f;
			}
			float u = (x + 0.5f) * step - 1f;
			points[i].localPosition = f(u, v, time);
        }   
    }
}
