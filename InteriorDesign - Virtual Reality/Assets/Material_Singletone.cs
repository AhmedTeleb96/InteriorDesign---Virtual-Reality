using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Material_Singletone : MonoBehaviour {

	static Material_Singletone mInstance;
	 Material selectedMaterial;

	public static Material_Singletone Instance
	{
		get
		{
			if (mInstance == null) mInstance = new Material_Singletone();
			return mInstance;
		}
	}

	public  Material SelectMaterial
	{

		get
			{
			return selectedMaterial;
			}

		set
		{
			selectedMaterial = value;
		}
	}
}
