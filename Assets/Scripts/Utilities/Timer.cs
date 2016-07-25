using UnityEngine;
using System.Collections;
using System;


public class Timer
{

	private float startTime;
	private float stopTime;
	private bool running = false;


	public void Start()
	{
		this.startTime = Time.time;
		this.running = true;
	}


	public void Stop()
	{
		this.stopTime = Time.time;
		this.running = false;
	}


	// elaspsed time in milliseconds
	public float ElapsedTime()
	{
		float interval;

		if (running)
			interval = Time.time - startTime;
		else
			interval = stopTime - startTime;

		return interval;
	}
}