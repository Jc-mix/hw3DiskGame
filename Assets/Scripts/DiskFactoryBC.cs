using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.Mygame;

namespace Com.Mygame{
	public class DiskFactory : System.Object {

		private static DiskFactory Instance;//The Factory
		private static List<GameObject> disklist;//Disks
		public GameObject diskTemplate;   //for the prefabs

		public static DiskFactory getInstance(){
			if (Instance == null) {
				Instance = new DiskFactory ();
				disklist = new List<GameObject> ();
			}
			return Instance;
		}

		//get the id of the Disk
		public int getDisk(){
			for (int i = 0; i < disklist.Count; i++) {
				if (!disklist [i].activeInHierarchy) {
					return i;
				}
			}
			disklist.Add (GameObject.Instantiate (diskTemplate) as GameObject);
			return disklist.Count - 1;
		} 

		public GameObject getDiskObject(int num){
			if (num >= 0 && num < disklist.Count) {
				return disklist [num];
			}
			return null;
		}

		//free the disk
		public void free(int num){
			if (num >= 0 && num < disklist.Count) {
				disklist [num].GetComponent<Rigidbody>().velocity = Vector3.zero;
				disklist [num].transform.localScale = diskTemplate.transform.localScale;
				disklist [num].SetActive (false);
			}
		}
	}
}

public class DiskFactoryBC : MonoBehaviour{
	public GameObject disk;
	void Awake(){
		DiskFactory.getInstance ().diskTemplate = disk;
	}
}
