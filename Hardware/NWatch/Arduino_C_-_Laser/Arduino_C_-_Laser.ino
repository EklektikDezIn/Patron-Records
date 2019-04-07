/*################### Arudino C Laser #############################
# Eklektik Design
# Micah Richards
# 02/11/17
#
# Purpose: Relay light information to PC running DoorC 
#
#################################################################*/

/*################### Includes ##################################*/
#include <SoftwareSerial.h>                         //## Bluetooth Communication
#include <Wire.h>                                   //## I2C Communication
//#include <String.h>

/*################### Global Variables ##########################*/
                                                    //## cactus.io/hookups/sensors/light/ldr/hookup-arduino-to-ldr-sensor
int LDR_Pin = A6;                                   //## Assign Light Dependent Resistor to Analog Pin 0
int Laser_Power = 7;
SoftwareSerial mySerial(3, 5);                      //## Allow serial output to Bluetooth Module
//String output;

/*################### Setup #######################################
# Purpose: Initilize components for program
#           Begins bluetooth serial connection
# Inputs:  N/A
#      
# Outputs: Announces activity of module through Bluetooth
#################################################################*/
void setup() {
  mySerial.begin (115200);
  mySerial.println("Module Active");
  pinMode(Laser_Power, OUTPUT);          // sets the digital pin 13 as output
}

/*################### Main Loop ###################################
# Purpose: Get data from LDR and output it through BT Module
#          
# Inputs:  LDR Readings
#      
# Outputs: Integer of light brightness [0-1000]
#################################################################*/
void loop() {
      
      digitalWrite(Laser_Power, HIGH);
      delay(50);
      for (int i = 0;i<100;i++){
        mySerial.print('0');
        mySerial.println(analogRead(LDR_Pin) + .1);
      }
      digitalWrite(Laser_Power, LOW);
      delay (50);
        mySerial.print('0');
        mySerial.println(analogRead(LDR_Pin) + .9);
}

