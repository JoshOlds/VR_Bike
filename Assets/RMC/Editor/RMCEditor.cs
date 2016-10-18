using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using CurveExtended;

[CustomEditor(typeof(RMCRealisticMotorcycleController)), CanEditMultipleObjects]
public class RMCEditor : Editor {

	RMCRealisticMotorcycleController motorScript;


	void Awake () {
	
		motorScript = (RMCRealisticMotorcycleController)target;

	}
	

	public override void OnInspectorGUI () {

		DrawDefaultInspector();

		if(GUI.changed)
			EngineCurveInit();

	}

	void EngineCurveInit (){
		
		if(motorScript.totalGears <= 0){
			return;
		}
		
		motorScript.gearSpeed = new float[motorScript.totalGears];
		motorScript.engineTorqueCurve = new AnimationCurve[motorScript.totalGears];
		motorScript.currentGear = 0;
		
		for(int i = 0; i < motorScript.engineTorqueCurve.Length; i ++){
			motorScript.engineTorqueCurve[i] = new AnimationCurve(new Keyframe(0, 1));
		}
		
		for(int i = 0; i < motorScript.totalGears; i ++){
			
			motorScript.gearSpeed[i] = Mathf.Lerp(0, motorScript.maxSpeed, ((float)i/(float)(motorScript.totalGears - 0)));
			
			if(i != 0){
				motorScript.engineTorqueCurve[i].MoveKey(0, new Keyframe(0, .25f));
				motorScript.engineTorqueCurve[i].AddKey(KeyframeUtil.GetNew(Mathf.Lerp(0, motorScript.maxSpeed, ((float)i/(float)(motorScript.totalGears - 0))), 1f, CurveExtended.TangentMode.Smooth));
				motorScript.engineTorqueCurve[i].AddKey(motorScript.maxSpeed, 0);
				motorScript.engineTorqueCurve[i].postWrapMode = WrapMode.Clamp;
				motorScript.engineTorqueCurve[i].UpdateAllLinearTangents();
			}else{
				motorScript.engineTorqueCurve[i].AddKey(KeyframeUtil.GetNew(Mathf.Lerp(25, motorScript.maxSpeed, ((float)i/(float)(motorScript.totalGears - 1))), 1.25f, TangentMode.Linear));
				motorScript.engineTorqueCurve[i].AddKey(KeyframeUtil.GetNew(Mathf.Lerp(25f, motorScript.maxSpeed, ((float)(i+1)/(float)(motorScript.totalGears - 1))), 0, TangentMode.Linear));
				motorScript.engineTorqueCurve[i].AddKey(motorScript.maxSpeed, 0);
				motorScript.engineTorqueCurve[i].UpdateAllLinearTangents();
			}
			
		}
		
	}

}
