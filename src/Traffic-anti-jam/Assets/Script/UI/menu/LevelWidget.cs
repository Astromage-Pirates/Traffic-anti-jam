using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelWidget : MonoBehaviour
{
	[SerializeField] List<Toggle> stars;
	public void Init(int stars,bool unlocked)
	{
		for(int i = 0; i < stars; i++)
		{
			this.stars[i].isOn = true;
		}
		for (int i = stars; i < this.stars.Count; i++)
		{
			this.stars[i].isOn = false;
		}
	}
}
