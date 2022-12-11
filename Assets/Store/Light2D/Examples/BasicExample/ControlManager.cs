using UnityEngine;
using System.Collections;

public class ControlManager : MonoBehaviour {

	public Light2D light2D;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void setIntensity(float intensity)
	{
		if(light2D!=null)
			light2D.Intensity=intensity;
	}
	public void setRange(float range)
	{
		if(light2D!=null)
			light2D.Range = range;
	}
	public void setAngle(float angle)
	{
		if(light2D!=null)
			light2D.TotalCoverageAngle = angle;
	}
	public void SetWhite()
	{
		if(light2D!=null)
			light2D.setColorWithoutAlpha(Color.white);
	}
	public void SetRed()
	{
		if(light2D!=null)
			light2D.setColorWithoutAlpha(Color.red);
	}
	public void SetGreen()
	{
		if(light2D!=null)
			light2D.setColorWithoutAlpha(Color.green);
	}public void SetBlue()
	{
		if(light2D!=null)
			light2D.setColorWithoutAlpha(Color.blue);
	}

}
