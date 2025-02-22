using UnityEngine;
using static UnityEngine.Mathf;
public static class FunctionLibrary
{

    // Instantiate the Function type which inherits the selected function
    public delegate Vector3 Function (float u, float v, float t);
    
    public enum FunctionName {Wave, MultiWave, Ripple, Sphere, Torus};

    static Function[] functions = {Wave, MultiWave, Ripple, Sphere, Torus};

    // Instantiation the GetFunction which provides the function for the above to inherit w.r.t input index
    public static Function GetFunction(FunctionName name){
        return functions[(int) name];
    }


    public static Vector3 Wave (float u, float v, float t) {
		Vector3 p;
		p.x = u;
		p.y = Sin(PI * (u + v + t));
		p.z = v;
		return p;
	}

    public static Vector3 MultiWave (float u, float v, float t) {
		Vector3 p;
		p.x = u;
		p.y = Sin(PI * (u + 0.5f * t));
		p.y += 0.5f * Sin(2f * PI * (v + t));
		p.y += Sin(PI * (u + v + 0.25f * t));
		p.y *= 1f / 2.5f;
		p.z = v;
		return p;
	}

	public static Vector3 Ripple (float u, float v, float t) {
		float d = Sqrt(u * u + v * v);
		Vector3 p;
		p.x = u;
		p.y = Sin(PI * (4f * d - t));
		p.y /= 1f + 10f * d;
		p.z = v;
		return p;
	}

    public static Vector3 Sphere (float u, float v, float t) {
        float r = 0.5f + 0.5f * Sin(PI * t*u*v);
		float s = r * Cos(0.5f * PI * v);
		Vector3 p;
		p.x = s * Sin(PI * u);
		p.y = r * Sin(0.5f * PI * v);
		p.z = s * Cos(PI * u);
        return p;
	}

    public static Vector3 Torus (float u, float v, float t) {
		float r1 = 0.1f + 0.2f * Sin(PI* (6f * u + 0.5f * t));
		float r2 = 0.1f + 0.2f * Cos(PI* (8f * u + 4f * v + 2f * t));
		float s = r1 + r2 * Cos(PI * v);
		Vector3 p;
		p.x = s * Sin(PI * u);
		p.y = r2 * Sin(PI * v);
		p.z = s * Cos(PI * u);
		return p;
	}
}
