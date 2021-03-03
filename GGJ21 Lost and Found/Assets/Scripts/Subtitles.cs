using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Subtitles : MonoBehaviour
{
    public string text;
	public Text textBox;

	public bool dead;

	private void OnTriggerEnter(Collider other)
	{
		// If player enter into gameobject's collider, start displaying subtitles
		if (other.gameObject.name == "Player") StartCoroutine(ShowSubtitle());
	}

	void Update()
	{
		if(dead)
		{
			textBox.text = "You died. Press R to restart.";
		}
	}

	IEnumerator ShowSubtitle()
	{
		// Show subititle once and then disable gameobject
		textBox.text = text;
		yield return new WaitForSeconds(5);
		textBox.text = null;
		gameObject.SetActive(false);
	}
}
