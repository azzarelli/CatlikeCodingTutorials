using UnityEngine;

public class Graph1 : MonoBehaviour
{

    [SerializeField]
    Transform pointPrefab;

    [SerializeField, Range(10,100)]
    int resolution = 10;

    [SerializeField]
    FunctionLibrary.FunctionName function;

    Transform[] points;

    [SerializeField, Min(0f)]
	float functionDuration = 1f, transitionDuration = 1f;

    float duration;
    bool transitioning;
	FunctionLibrary.FunctionName transitionFunction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
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
        duration += Time.deltaTime;
        if(transitioning){
            if (duration >= transitionDuration) {
				duration -= transitionDuration;
				transitioning = false;
			}
        }
		else if (duration >= functionDuration) {
			duration -= functionDuration;
            transitioning = true;
			transitionFunction = function;
            function = FunctionLibrary.GetNextFunctionName(function);
		}

        if (transitioning) {
			UpdateFunctionTransition();
		}
		else {
			UpdateFunction();
		}
        
        
    }
    void UpdateFunction (){
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
    void UpdateFunctionTransition (){
        FunctionLibrary.Function 
            from = FunctionLibrary.GetFunction(transitionFunction),
			to = FunctionLibrary.GetFunction(function);
		float progress = duration / transitionDuration;

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
			points[i].localPosition = FunctionLibrary.Morph(
				u, v, time, from, to, progress
			);
        }


    }
}