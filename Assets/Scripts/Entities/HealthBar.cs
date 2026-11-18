// Author: Nour
//Summary:
/* Creates an enemy health bar to show enemies' current health (uses Unity's slider function to do so)
   Healthbar also appears on top of enemies and follows them around*/


// import namespaces 
//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //creates a variable to store slider, since the healthbar + slider were created using Unity's UI system


namespace Entities.HealthBar {

    // create HealthBar class
    public class HealthBar : MonoBehaviour {
        
        //create a struct
        public Slider slider;
        
        //Uses the SetParent method to set the parent of the transform (aka the player)
        public Transform playerController;
        
        void Start() {
            
            //Sets "playerController" as the new parent of the healthbar
            transform.SetParent(playerController);
            
            //worldPositionStays set to false, so healthbar keeps its local orientation 
            transform.SetParent(playerController, false);
            
            //So the healthbar is placed 2 units above the player's head
            transform.localPosition = new Vector3(0, 2f, 0);
        }
        
        public void SetMaxHealth(int health) {
            slider.maxValue = health; //slider starts at max health
            slider.value = health; 
            
        }
        
        //Create function for setting the amount of health on the HealthBar
        public void SetHealth(int health) {
            slider.value = health;  //health value should be defined in the enemy's script
            
        }


    }
}
