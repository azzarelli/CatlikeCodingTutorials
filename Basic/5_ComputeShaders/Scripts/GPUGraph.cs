using UnityEngine;

public class GPUGraph : MonoBehaviour
{

    const int maxResolution = 1000;

    [SerializeField, Range(10,maxResolution)]
    int resolution = 10;

    [SerializeField]
    FunctionLibrary.FunctionName function;


    [SerializeField, Min(0f)]
	float functionDuration = 1f, transitionDuration = 1f;

    float duration;
    bool transitioning;
	FunctionLibrary.FunctionName transitionFunction;

    // Need to add compute buffer
    ComputeBuffer positionsBuffer;
    // Link to computer shader
    [SerializeField]
    ComputeShader computeShader; 

    static readonly int positionsId = Shader.PropertyToID("_Positions"),
		resolutionId = Shader.PropertyToID("_Resolution"),
		stepId = Shader.PropertyToID("_Step"),
		timeId = Shader.PropertyToID("_Time"),
		transitionProgressId = Shader.PropertyToID("_TransitionProgress");

    
    
    // We need to instruct the GPU what properties our points (already known to the GPU with the ubffer) have
    [SerializeField]
    Material material;
    [SerializeField]
    Mesh mesh;

    // We use onEnable and onDisable instead of Awake/Start so that the code survives a hot reload.
    void OnEnable(){
        // Init compute bubffer - as we need three floats per point, the element size in 3*4
        positionsBuffer = new ComputeBuffer(maxResolution * maxResolution, 3 * 4);
    }
    void onDisable(){
        // We dont need this persayy, but its best practice to release memory asap as we dont want
        // floating garbbage
        positionsBuffer.Release();
        // We add the following statement to allow Unity's garbage collector to reclaim it before the next time it runs
        // during a hot reload
        positionsBuffer = null;

    }
 
    void UpdateFunctionOnGPU () { 

        // Initialize/Update the parameters we wabnt the GPU to know about (defined in the compute script)
		float step = 2f / resolution;
		computeShader.SetInt(resolutionId, resolution);
		computeShader.SetFloat(stepId, step);
		computeShader.SetFloat(timeId, Time.time);

        if (transitioning) {
			computeShader.SetFloat(
				transitionProgressId,
				Mathf.SmoothStep(0f, 1f, duration / transitionDuration)
			);
		}
        
        // This doesnt comput and data bbut need to be invked as it links the buffer to the Kernel.
        // I.e. the bubffer stores and kernel processes
        // The addtional point about the kernel is that we defined various kernets for our compute
        // shader functionality, meaning we can set the buffer index w.r.t the function thats being used
        // My thoughts are that this may means when we transition we dont have to modify the current buffer and instead
        // have a set of dedicated buffers for each function
        var kernelIndex =
			(int)function +
			(int)(transitioning ? transitionFunction : function) *
			FunctionLibrary.FunctionCount;
		computeShader.SetBuffer(kernelIndex, positionsId, positionsBuffer);

        // We need the buffer (current 8x8) to divide into the resoluton (i.e. res/8 = int)
        int groups = Mathf.CeilToInt(resolution / 8f);
        // After setting the buffer we can run the kernel usin Dispatch
        computeShader.Dispatch(kernelIndex, groups, groups, 1); // inputs: Kernel indenx, 3-D thread multipliers
        // here thread multiplies is the number of groups of threads we are processing.
        // As we define resolution as the steps along an axis and our graph is square
        // the groups can be handeled this way. 

        // We need to set the material properties
        material.SetBuffer(positionsId, positionsBuffer);
        material.SetFloat(stepId, step);

        // Procedural drawing follows
        // First cal;uclate boudns (similar to frustrum culling but as we view the whole graph
        // this will be self contained in the square boxdefine by the max res.
        var bounds = new Bounds(Vector3.zero, Vector3.one * (2f + 2f / resolution));
        Graphics.DrawMeshInstancedProcedural(mesh, 0, material, bounds, resolution * resolution);
        //Note; The DrawMeshInstancedIndirect method is useful for when you do not know how many 
        // instances to draw on the CPU side and instead provide that information with a compute
        // shader via a buffer.
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
        
        // Update GPU
        UpdateFunctionOnGPU();

        
    }


    
}
