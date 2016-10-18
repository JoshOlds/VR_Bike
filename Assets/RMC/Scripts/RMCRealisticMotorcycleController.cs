using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof(Rigidbody))]

public class RMCRealisticMotorcycleController : MonoBehaviour {

	private Rigidbody rigid;

	public WheelCollider FrontWheelCollider;
	public WheelCollider RearWheelCollider;
	public Transform FrontWheelTransform;
	public Transform RearWheelTransform;
	public Transform Fender;
	public Transform SteeringHandlebar; 
	public Transform COM;
    public bool UsingController;

	//Gearbox
	public bool changingGear = false;
	public float gearShiftRate = 10.0f;
	[HideInInspector]
	public float[] gearSpeed;
	public int currentGear;
	public int totalGears = 6;
	private int _totalGears
	{
		get
		{
			return totalGears - 1;
		}
	}

	//Bike Body Lean
	public GameObject chassis;
	public float chassisVerticalLean = 4.0f;
	public float chassisHorizontalLean = 4.0f;
	private float horizontalLean = 0.0f;
	private float verticalLean = 0.0f;

	//Configurations
	[HideInInspector]
	public AnimationCurve[] engineTorqueCurve;
	public float EngineTorque  = 1500f;
	public float MaxEngineRPM  = 6000f;
	public float MinEngineRPM  = 1000f;
	public float SteerAngle = 40f;
	[HideInInspector]
	public float Speed;
	public float highSpeedSteerAngle = 5f;
	public float highSpeedSteerAngleAtSpeed = 80f;
	public float maxSpeed = 180f;
	public float Brake = 2500f;

	private float EngineRPM = 0f;
	private float motorInput = 0f;
	private float defsteerAngle = 0f;
	private float RotationValue1 = 0f;
	private float RotationValue2 = 0f;
	[HideInInspector]
	public bool brakingNow = false;
	[HideInInspector]
	public float steerInput = 0f;
	[HideInInspector]
	public bool crashed = false;
	private bool reversing = false; 

	//Audio
	private AudioSource engineStartAudio;
	public AudioClip engineStartClip;
	private AudioSource engineAudio;
	public AudioClip engineClip;
	private AudioSource skidAudio;
	public AudioClip skidClip;
	private AudioSource crashAudio;
	public AudioClip[] crashClips;
	private AudioSource gearShiftingSound;
	public AudioClip[] gearShiftingClips;

	//Particles
	public GameObject WheelSlipPrefab;
	private List <GameObject> WheelParticles = new List<GameObject>();
	public ParticleEmitter[] normalExhaustGas;
	public ParticleEmitter[] heavyExhaustGas;


	void Start (){

		SoundsInitialize();
		if(WheelSlipPrefab)
			SmokeInit();

		//Rigidbody
		rigid = GetComponent<Rigidbody>();
		rigid.constraints = RigidbodyConstraints.FreezeRotationZ;
		rigid.centerOfMass = new Vector3(COM.localPosition.x * transform.localScale.x , COM.localPosition.y * transform.localScale.y , COM.localPosition.z * transform.localScale.z);
		rigid.maxAngularVelocity = 2f;

		defsteerAngle = SteerAngle;

	}

	public AudioSource CreateAudioSource(string audioName, float minDistance, float volume, AudioClip audioClip, bool loop, bool playNow, bool destroyAfterFinished){
		
		GameObject audioSource = new GameObject(audioName);
		audioSource.transform.position = transform.position;
		audioSource.transform.rotation = transform.rotation;
		audioSource.transform.parent = transform;
		audioSource.AddComponent<AudioSource>();
		audioSource.GetComponent<AudioSource>().minDistance = minDistance;
		audioSource.GetComponent<AudioSource>().volume = volume;
		audioSource.GetComponent<AudioSource>().clip = audioClip;
		audioSource.GetComponent<AudioSource>().loop = loop;
		audioSource.GetComponent<AudioSource>().spatialBlend = 1f;
		
		if(playNow)
			audioSource.GetComponent<AudioSource>().Play();
		
		if(destroyAfterFinished)
			Destroy(audioSource, audioClip.length);
		
		return audioSource.GetComponent<AudioSource>();
		
	}

	public void SoundsInitialize (){
		
		engineAudio = CreateAudioSource("engineSound", 5, 0, engineClip, true, true, false);
		skidAudio = CreateAudioSource("skidSound", 5, 0, skidClip, true, true, false);
		engineStartAudio = CreateAudioSource("engineStartSound", 5, .7f, engineStartClip, false, true, true);
		
	}

	public void SmokeInit (){
		
		string wheelSlipPrefabName = WheelSlipPrefab.name+"(Clone)";
		
		for(int i = 0; i < 2; i++){
			Instantiate(WheelSlipPrefab, transform.position, transform.rotation);
		}
		
		foreach(GameObject go in GameObject.FindObjectsOfType(typeof(GameObject)))
		{
			if(go.name == wheelSlipPrefabName)
				WheelParticles.Add (go);
		} 
		
		WheelParticles[0].transform.position = FrontWheelCollider.transform.position;
		WheelParticles[1].transform.position = RearWheelCollider.transform.position;
		
		WheelParticles[0].transform.parent = FrontWheelCollider.transform;
		WheelParticles[1].transform.parent = RearWheelCollider.transform;
		
	}
	
	void FixedUpdate (){

		Inputs();
		Engine();
		Braking();
		ShiftGears();
		SkidAudio();
		Smoke();

	}

	void Update(){

		WheelAlign();
		Lean();

	}

	void Inputs (){

		Speed = rigid.velocity.magnitude * 3.6f;

		//Freezing rotation by Z axis.
		transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);

        var va = bikeController.Speed;
        var ha = bikeController.Heading;
		//If crashed...
		if(!crashed){
			if(!changingGear)
				motorInput = Input.GetAxis("Vertical");
			else
				motorInput = Mathf.Clamp(Input.GetAxis("Vertical"), -1, 0);
			steerInput = Input.GetAxis("Horizontal");
		}else{
			motorInput = 0;
			steerInput = 0;
		}
        if (UsingController)
        {
            motorInput = va;
            steerInput = ha;
        }

		//Reverse bool
		if(motorInput < 0  && transform.InverseTransformDirection(rigid.velocity).z < 0) 
			reversing = true;
		else
			reversing = false;

	}

	void Engine (){
		
		//Steer Limit.
		SteerAngle = Mathf.Lerp(defsteerAngle, highSpeedSteerAngle, (Speed / highSpeedSteerAngleAtSpeed));
		FrontWheelCollider.steerAngle = SteerAngle * steerInput;

		//Engine RPM.
		EngineRPM = Mathf.Clamp((((Mathf.Abs((FrontWheelCollider.rpm + RearWheelCollider.rpm)) * gearShiftRate) + MinEngineRPM)) / (currentGear + 1), MinEngineRPM, MaxEngineRPM);
		
		//Engine Audio Volume.
		engineAudio.volume = Mathf.Lerp (engineAudio.volume, Mathf.Clamp (motorInput, .35f, .85f), Time.deltaTime*  5);
		engineAudio.pitch = Mathf.Lerp ( engineAudio.pitch, Mathf.Lerp (1f, 2f, (EngineRPM - (MinEngineRPM / 1.5f)) / (MaxEngineRPM + MinEngineRPM)), Time.deltaTime * 5);
		
		if(engineStartAudio)
			engineStartAudio.GetComponent<AudioSource>().volume -= Time.deltaTime / 5f;

		// Applying Motor Torque.
		if(Speed > maxSpeed){
			RearWheelCollider.motorTorque = 0;
		}else if(!reversing && !changingGear){
			RearWheelCollider.motorTorque = EngineTorque  * Mathf.Clamp(motorInput, 0f, 1f) * engineTorqueCurve[currentGear].Evaluate(Speed);
		}
		
		if(reversing){
			if(Speed < 10){
				RearWheelCollider.motorTorque = (EngineTorque  * motorInput) / 5f;
			}else{
				RearWheelCollider.motorTorque = 0;
			}
		}
		
	}

	public void Braking (){
			
		// Deceleration.
		if(Mathf.Abs (motorInput) <= .05f){
			brakingNow = false;
			FrontWheelCollider.brakeTorque = (Brake) / 25f;
			RearWheelCollider.brakeTorque = (Brake) / 25f;
		}else if(motorInput < 0 && !reversing){
			brakingNow = true;
			FrontWheelCollider.brakeTorque = (Brake) * (Mathf.Abs(motorInput) / 5f);
			RearWheelCollider.brakeTorque = (Brake) * (Mathf.Abs(motorInput ));
		}else{
			brakingNow = false;
			FrontWheelCollider.brakeTorque = 0;
			RearWheelCollider.brakeTorque = 0;
		}
		
	}

	void WheelAlign (){

		RaycastHit hit;
		WheelHit CorrespondingGroundHit;
		float extension_F;
		float extension_R;

		Vector3 ColliderCenterPointFL = FrontWheelCollider.transform.TransformPoint( FrontWheelCollider.center );
		FrontWheelCollider.GetGroundHit( out CorrespondingGroundHit );

		if ( Physics.Raycast( ColliderCenterPointFL, -FrontWheelCollider.transform.up, out hit, (FrontWheelCollider.suspensionDistance + FrontWheelCollider.radius) * transform.localScale.y) ) {
			if(hit.transform.gameObject.layer != LayerMask.NameToLayer("Bike")){
				FrontWheelTransform.transform.position = hit.point + (FrontWheelCollider.transform.up * FrontWheelCollider.radius) * transform.localScale.y;
				if(Fender)
					Fender.transform.position = hit.point + (FrontWheelCollider.transform.up * (FrontWheelCollider.radius + FrontWheelCollider.suspensionDistance)) * transform.localScale.y;
				extension_F = (-FrontWheelCollider.transform.InverseTransformPoint(CorrespondingGroundHit.point).y - FrontWheelCollider.radius) / FrontWheelCollider.suspensionDistance;
				Debug.DrawLine(CorrespondingGroundHit.point, CorrespondingGroundHit.point + FrontWheelCollider.transform.up * (CorrespondingGroundHit.force / 8000), extension_F <= 0.0f ? Color.magenta : Color.white);
				Debug.DrawLine(CorrespondingGroundHit.point, CorrespondingGroundHit.point - FrontWheelCollider.transform.forward * CorrespondingGroundHit.forwardSlip, Color.green);
				Debug.DrawLine(CorrespondingGroundHit.point, CorrespondingGroundHit.point - FrontWheelCollider.transform.right * CorrespondingGroundHit.sidewaysSlip, Color.red);
			}
		}else{
			FrontWheelTransform.transform.position = ColliderCenterPointFL - (FrontWheelCollider.transform.up * FrontWheelCollider.suspensionDistance) * transform.localScale.y;
		}
		RotationValue1 += FrontWheelCollider.rpm * ( 6 ) * Time.deltaTime;
		FrontWheelTransform.transform.rotation = FrontWheelCollider.transform.rotation * Quaternion.Euler( RotationValue1, FrontWheelCollider.steerAngle, FrontWheelCollider.transform.rotation.z);

		Vector3 ColliderCenterPointRL = RearWheelCollider.transform.TransformPoint( RearWheelCollider.center );
		RearWheelCollider.GetGroundHit( out CorrespondingGroundHit );

		if ( Physics.Raycast( ColliderCenterPointRL, -RearWheelCollider.transform.up, out hit, (RearWheelCollider.suspensionDistance + RearWheelCollider.radius) * transform.localScale.y) ) {
			if(hit.transform.gameObject.layer != LayerMask.NameToLayer("Bike")){
				RearWheelTransform.transform.position = hit.point + (RearWheelCollider.transform.up * RearWheelCollider.radius) * transform.localScale.y;
				extension_R = (-RearWheelCollider.transform.InverseTransformPoint(CorrespondingGroundHit.point).y - RearWheelCollider.radius) / RearWheelCollider.suspensionDistance;
				Debug.DrawLine(CorrespondingGroundHit.point, CorrespondingGroundHit.point + RearWheelCollider.transform.up * (CorrespondingGroundHit.force / 8000), extension_R <= 0.0f ? Color.magenta : Color.white);
				Debug.DrawLine(CorrespondingGroundHit.point, CorrespondingGroundHit.point - RearWheelCollider.transform.forward * CorrespondingGroundHit.forwardSlip, Color.green);
				Debug.DrawLine(CorrespondingGroundHit.point, CorrespondingGroundHit.point - RearWheelCollider.transform.right * CorrespondingGroundHit.sidewaysSlip, Color.red);
			}
		}else{
			RearWheelTransform.transform.position = ColliderCenterPointRL - (RearWheelCollider.transform.up * RearWheelCollider.suspensionDistance) * transform.localScale.y;
		}
		RotationValue2 += RearWheelCollider.rpm * ( 6 ) * Time.deltaTime;
		RearWheelTransform.transform.rotation = RearWheelCollider.transform.rotation * Quaternion.Euler( RotationValue2, RearWheelCollider.steerAngle, RearWheelCollider.transform.rotation.z);

		//Steering Wheel and Fender transforms
		if(SteeringHandlebar)
			SteeringHandlebar.transform.rotation = FrontWheelCollider.transform.rotation * Quaternion.Euler( 0, FrontWheelCollider.steerAngle, FrontWheelCollider.transform.rotation.z);
		if(Fender)
			Fender.rotation = FrontWheelCollider.transform.rotation * Quaternion.Euler( 0, FrontWheelCollider.steerAngle, FrontWheelCollider.transform.rotation.z);

	}
	
	public void ShiftGears (){
			
		if(currentGear < _totalGears && !changingGear){
			if(EngineRPM > (MaxEngineRPM - 500) && RearWheelCollider.rpm >= 0){
				StartCoroutine("ChangingGear", currentGear + 1);
			}
		}
		
		if(currentGear > 0){
			if(EngineRPM < MinEngineRPM + 500 && !changingGear){
				
				for(int i = 0; i < gearSpeed.Length; i++){
					if(Speed > gearSpeed[i])
						StartCoroutine("ChangingGear", i);
				}
				
			}
		}
		
	}

	IEnumerator ChangingGear(int gear){
		
		changingGear = true;
		
		if(gearShiftingClips.Length >= 1){
			
			gearShiftingSound = CreateAudioSource("gearShiftingAudio", 5f, .3f, gearShiftingClips[UnityEngine.Random.Range(0, gearShiftingClips.Length)], false, true, true);
			
		}
		
		yield return new WaitForSeconds(.5f);
		changingGear = false;
		currentGear = gear;
		
	}

	void Lean (){

		verticalLean = Mathf.Clamp(Mathf.Lerp (verticalLean, transform.InverseTransformDirection(rigid.angularVelocity).x * chassisVerticalLean, Time.deltaTime * 5f), -10.0f, 10.0f);
		
		WheelHit CorrespondingGroundHit;
		FrontWheelCollider.GetGroundHit(out CorrespondingGroundHit);
		
		float normalizedLeanAngle = Mathf.Clamp(CorrespondingGroundHit.sidewaysSlip, -1f, 1f);
		
		if(transform.InverseTransformDirection(rigid.velocity).z > 0f)
			normalizedLeanAngle = -1;
		else
			normalizedLeanAngle = 1;
		
		horizontalLean = Mathf.Clamp(Mathf.Lerp (horizontalLean, (transform.InverseTransformDirection(rigid.angularVelocity).y * normalizedLeanAngle) * chassisHorizontalLean, Time.deltaTime * 3f), -50.0f, 50.0f);
		
		Quaternion target = Quaternion.Euler(verticalLean, chassis.transform.localRotation.y + (rigid.angularVelocity.z), horizontalLean);
		chassis.transform.localRotation = target;
		
		rigid.centerOfMass = new Vector3((COM.localPosition.x) * transform.localScale.x , (COM.localPosition.y) * transform.localScale.y , (COM.localPosition.z) * transform.localScale.z);

	}

	public void Smoke () {
		
		WheelHit CorrespondingGroundHit0;
		WheelHit CorrespondingGroundHit1;
		
		if ( WheelParticles.Count > 0 ) {
			
			FrontWheelCollider.GetGroundHit( out CorrespondingGroundHit0 );
			if(Mathf.Abs(CorrespondingGroundHit0.sidewaysSlip) > .25f || Mathf.Abs(CorrespondingGroundHit0.forwardSlip) > .5f ){
				WheelParticles[0].GetComponent<ParticleEmitter>().emit = true;
			}else{ 
				WheelParticles[0].GetComponent<ParticleEmitter>().emit = false;
			}
			
			RearWheelCollider.GetGroundHit( out CorrespondingGroundHit1 );
			if(Mathf.Abs(CorrespondingGroundHit1.sidewaysSlip) > .25f || Mathf.Abs(CorrespondingGroundHit1.forwardSlip) > .5f ){
				WheelParticles[1].GetComponent<ParticleEmitter>().emit = true;
			}else{ 
				WheelParticles[1].GetComponent<ParticleEmitter>().emit = false;
			}
			
		}
		
		if(normalExhaustGas.Length > 0){

			foreach(ParticleEmitter pe in normalExhaustGas){
				if(Speed < 20)
					pe.emit = true;
				else
					pe.emit = false;
			}

		}
		
		if(heavyExhaustGas.Length > 0){
			
			foreach(ParticleEmitter pe in heavyExhaustGas){
				if(Speed < 20 && motorInput > .5f)
					pe.emit = true;
				else
					pe.emit = false;
			}
			
		}
		
	}

	public void SkidAudio (){
		
		if(!skidAudio)
			return;
		
		WheelHit CorrespondingGroundHitF;
		FrontWheelCollider.GetGroundHit( out CorrespondingGroundHitF );
		
		WheelHit CorrespondingGroundHitR;
		RearWheelCollider.GetGroundHit( out CorrespondingGroundHitR );
		
		if(Mathf.Abs(CorrespondingGroundHitF.sidewaysSlip) > .25f || Mathf.Abs(CorrespondingGroundHitR.forwardSlip) > .5f || Mathf.Abs(CorrespondingGroundHitF.forwardSlip) > .5f){
			if(rigid.velocity.magnitude > 1f)
				skidAudio.volume = Mathf.Abs(CorrespondingGroundHitF.sidewaysSlip) + ((Mathf.Abs(CorrespondingGroundHitF.forwardSlip) + Mathf.Abs(CorrespondingGroundHitR.forwardSlip)) / 4f);
			else
				skidAudio.volume -= Time.deltaTime;
		}else{
			skidAudio.volume -= Time.deltaTime;
		}
		
	}

	void OnCollisionEnter (Collision collision){
		
		if (collision.contacts.Length > 0){
			
			if(collision.relativeVelocity.magnitude > 10 && crashClips.Length > 0){
				
				if (collision.contacts[0].thisCollider.gameObject.transform != transform.parent){
					
					crashAudio = CreateAudioSource("crashSound", 5, 1, crashClips[UnityEngine.Random.Range(0, crashClips.Length)], false, true, true);
					
				}
				
			}
			
		}

		//crashed = true;
		
	}

}