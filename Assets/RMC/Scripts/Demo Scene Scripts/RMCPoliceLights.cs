using UnityEngine;
using System.Collections;

public class RMCPoliceLights : MonoBehaviour {

	public GameObject dummyIllumin;
	private Shader illumin;
	private RMCRealisticMotorcycleController motorScript;

	public GameObject headLight;
	public GameObject brakeLight;
	public GameObject[] lights;
	public GameObject[] rightSignals;
	public GameObject[] leftSignals;
	public GameObject[] miscLights;

	private bool headLightsOn = false;
	public bool sirenOn = false;

	private Shader defaultShader;

	private Light[] lightSources;
	private Light[] rightSignalsLightSources;
	private Light[] leftSignalsLightSources;
	private Light[] miscLightSources;
	private Light headLightLightSource;


	void Start () {
	
		if(dummyIllumin)
			illumin = dummyIllumin.GetComponent<MeshRenderer>().material.shader; 
		else
			illumin = Shader.Find ("Self-Illumin/Specular");

		defaultShader = Shader.Find ("Bumped Specular");
		motorScript = GetComponent<RMCRealisticMotorcycleController>();

		lightSources = new Light[lights.Length];
		rightSignalsLightSources = new Light[rightSignals.Length];
		leftSignalsLightSources = new Light[leftSignals.Length];
		miscLightSources = new Light[miscLights.Length];
		headLightLightSource = new Light();
		headLightLightSource = headLight.GetComponentInChildren<Light>();

		for(int i = 0; i < lights.Length; i++){
			lightSources[i] = lights[i].GetComponentInChildren<Light>();
		}
		for(int i = 0; i < rightSignals.Length; i++){
			rightSignalsLightSources[i] = rightSignals[i].GetComponentInChildren<Light>();
		}
		for(int i = 0; i < leftSignals.Length; i++){
			leftSignalsLightSources[i] = leftSignals[i].GetComponentInChildren<Light>();
		}
		for(int i = 0; i < miscLights.Length; i++){
			miscLightSources[i] = miscLights[i].GetComponentInChildren<Light>();
		}

		StartCoroutine(FlashLights());
		StartCoroutine(SignalLights());
		StartCoroutine(MiscLights());

	}

	IEnumerator FlashLights () {

		yield return new WaitForSeconds(1);

		if(sirenOn){

			for(int i = 0; i < lights.Length; i++){

				lights[i].GetComponent<MeshRenderer>().material.shader = illumin;
				lightSources[i].GetComponent<Light>().enabled = true;
				yield return new WaitForSeconds(.05f);
				lights[i].GetComponent<MeshRenderer>().material.shader = defaultShader;
				lightSources[i].GetComponent<Light>().enabled = false;
				yield return new WaitForSeconds(.05f);
				lights[i].GetComponent<MeshRenderer>().material.shader = illumin;
				lightSources[i].GetComponent<Light>().enabled = true;
				yield return new WaitForSeconds(.05f);
				lights[i].GetComponent<MeshRenderer>().material.shader = defaultShader;
				lightSources[i].GetComponent<Light>().enabled = false;
				yield return new WaitForSeconds(.05f);
				lights[i].GetComponent<MeshRenderer>().material.shader = illumin;
				lightSources[i].GetComponent<Light>().enabled = true;
				yield return new WaitForSeconds(.05f);
				lights[i].GetComponent<MeshRenderer>().material.shader = defaultShader;
				lightSources[i].GetComponent<Light>().enabled = false;
				yield return new WaitForSeconds(.05f);
				lights[i].GetComponent<MeshRenderer>().material.shader = illumin;
				lightSources[i].GetComponent<Light>().enabled = true;
				yield return new WaitForSeconds(.05f);
				lights[i].GetComponent<MeshRenderer>().material.shader = defaultShader;
				lightSources[i].GetComponent<Light>().enabled = false;
				yield return new WaitForSeconds(.05f);

			}

		}

		StartCoroutine(FlashLights());
		 
	}

	IEnumerator SignalLights () {

		yield return new WaitForEndOfFrame();

		if(sirenOn){
		
			for(int i = 0; i < rightSignals.Length; i++){
				
				rightSignals[i].GetComponent<MeshRenderer>().material.shader = illumin;
				rightSignalsLightSources[i].GetComponent<Light>().enabled = true;
				yield return new WaitForSeconds(.05f);
				rightSignals[i].GetComponent<MeshRenderer>().material.shader = defaultShader;
				rightSignalsLightSources[i].GetComponent<Light>().enabled = false;
				yield return new WaitForSeconds(.05f);
				rightSignals[i].GetComponent<MeshRenderer>().material.shader = illumin;
				rightSignalsLightSources[i].GetComponent<Light>().enabled = true;
				yield return new WaitForSeconds(.05f);
				rightSignals[i].GetComponent<MeshRenderer>().material.shader = defaultShader;
				rightSignalsLightSources[i].GetComponent<Light>().enabled = false;
				yield return new WaitForSeconds(.05f);
				rightSignals[i].GetComponent<MeshRenderer>().material.shader = illumin;
				rightSignalsLightSources[i].GetComponent<Light>().enabled = true;
				yield return new WaitForSeconds(.05f);
				rightSignals[i].GetComponent<MeshRenderer>().material.shader = defaultShader;
				rightSignalsLightSources[i].GetComponent<Light>().enabled = false;
				yield return new WaitForSeconds(.05f);
				rightSignals[i].GetComponent<MeshRenderer>().material.shader = illumin;
				rightSignalsLightSources[i].GetComponent<Light>().enabled = true;
				yield return new WaitForSeconds(.05f);
				rightSignals[i].GetComponent<MeshRenderer>().material.shader = defaultShader;
				rightSignalsLightSources[i].GetComponent<Light>().enabled = false;
				yield return new WaitForSeconds(.05f);
			}

			for(int i = 0; i < leftSignals.Length; i++){
				
				leftSignals[i].GetComponent<MeshRenderer>().material.shader = illumin;
				leftSignalsLightSources[i].GetComponent<Light>().enabled = true;
				yield return new WaitForSeconds(.05f);
				leftSignals[i].GetComponent<MeshRenderer>().material.shader = defaultShader;
				leftSignalsLightSources[i].GetComponent<Light>().enabled = false;
				yield return new WaitForSeconds(.05f);
				leftSignals[i].GetComponent<MeshRenderer>().material.shader = illumin;
				leftSignalsLightSources[i].GetComponent<Light>().enabled = true;
				yield return new WaitForSeconds(.05f);
				leftSignals[i].GetComponent<MeshRenderer>().material.shader = defaultShader;
				leftSignalsLightSources[i].GetComponent<Light>().enabled = false;
				yield return new WaitForSeconds(.05f);
				leftSignals[i].GetComponent<MeshRenderer>().material.shader = illumin;
				leftSignalsLightSources[i].GetComponent<Light>().enabled = true;
				yield return new WaitForSeconds(.05f);
				leftSignals[i].GetComponent<MeshRenderer>().material.shader = defaultShader;
				leftSignalsLightSources[i].GetComponent<Light>().enabled = false;
				yield return new WaitForSeconds(.05f);
				leftSignals[i].GetComponent<MeshRenderer>().material.shader = illumin;
				leftSignalsLightSources[i].GetComponent<Light>().enabled = true;
				yield return new WaitForSeconds(.05f);
				leftSignals[i].GetComponent<MeshRenderer>().material.shader = defaultShader;
				leftSignalsLightSources[i].GetComponent<Light>().enabled = false;
				yield return new WaitForSeconds(.05f);
			}

		}

		StartCoroutine(SignalLights());

	}

	IEnumerator MiscLights(){

		yield return new WaitForEndOfFrame();

		if(sirenOn){

			for(int i = 0; i < miscLights.Length; i++){ 
				
				miscLights[i].GetComponent<MeshRenderer>().material.shader = illumin;
				miscLightSources[i].GetComponent<Light>().enabled = true;
				yield return new WaitForSeconds(.05f);
				miscLights[i].GetComponent<MeshRenderer>().material.shader = defaultShader;
				miscLightSources[i].GetComponent<Light>().enabled = false;
				yield return new WaitForSeconds(.05f);
				miscLights[i].GetComponent<MeshRenderer>().material.shader = illumin;
				miscLightSources[i].GetComponent<Light>().enabled = true;
				yield return new WaitForSeconds(.05f);
				miscLights[i].GetComponent<MeshRenderer>().material.shader = defaultShader;
				miscLightSources[i].GetComponent<Light>().enabled = false;
				yield return new WaitForSeconds(.05f);
				miscLights[i].GetComponent<MeshRenderer>().material.shader = illumin;
				miscLightSources[i].GetComponent<Light>().enabled = true;
				yield return new WaitForSeconds(.05f);
				miscLights[i].GetComponent<MeshRenderer>().material.shader = defaultShader;
				miscLightSources[i].GetComponent<Light>().enabled = false;
				yield return new WaitForSeconds(.05f);
				miscLights[i].GetComponent<MeshRenderer>().material.shader = illumin;
				miscLightSources[i].GetComponent<Light>().enabled = true;
				yield return new WaitForSeconds(.05f);
				miscLights[i].GetComponent<MeshRenderer>().material.shader = defaultShader;
				miscLightSources[i].GetComponent<Light>().enabled = false;
				yield return new WaitForSeconds(.05f);
				
			}

		}

		StartCoroutine(MiscLights());

	}

	void Update(){

		if(motorScript.brakingNow){

			brakeLight.GetComponent<MeshRenderer>().material.shader = illumin;
			brakeLight.GetComponentInChildren<Light>().intensity = Mathf.Lerp (brakeLight.GetComponentInChildren<Light>().intensity, 1, Time.deltaTime * 10);

		}else{

			brakeLight.GetComponent<MeshRenderer>().material.shader = defaultShader;
			brakeLight.GetComponentInChildren<Light>().intensity = Mathf.Lerp (brakeLight.GetComponentInChildren<Light>().intensity, 0, Time.deltaTime * 10);

		}

			if(Input.GetKeyDown(KeyCode.L)){
				if(!headLightsOn){
					headLight.GetComponent<MeshRenderer>().material.shader = illumin;
					headLightLightSource.GetComponent<Light>().enabled = true;
					headLightsOn = true;
				}else{
					headLight.GetComponent<MeshRenderer>().material.shader = defaultShader;
					headLightLightSource.GetComponent<Light>().enabled = false;
					headLightsOn = false;
				}
			}

		if(Input.GetKeyDown(KeyCode.G)){
			if(!sirenOn){
				sirenOn = true;
			}else{
				sirenOn = false;
			}

		}

	}
	
}
