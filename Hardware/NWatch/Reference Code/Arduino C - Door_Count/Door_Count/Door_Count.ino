#include <NewPing.h>
#include <Wire.h>
#include <SoftwareSerial.h>
#include <string.h>
bool CMode = false;
int doorwidth = 200; //cm
int trigger = 80; //cm
int standp = 2;
int passp = 2;
int p_slow = 100;
int p_fast = 80;
int dout = 2;

NewPing sonar(12, 11, doorwidth+20);
SoftwareSerial mySerial(2, 3);

void setup() {
  pinMode(9, OUTPUT);
  mySerial.begin (115200);
  mySerial.println("Module Active");
}

int stand = 0;
int pings = p_slow;
int ntime = 0;
bool standing = false;
int pass = 0;


void loop() {
  String c = " ";
  if (mySerial.available())
  {  
    c = mySerial.readString();
    mySerial.println(c);
  if (c.startsWith("DC+override"))
  {
    CMode = true;
    mySerial.println("Command Mode active.");
  }
  if (CMode){
    String message = "";
      if (c.startsWith("DC+width")){
        if (c.indexOf("=:") != -1){
          doorwidth = c.substring(c.indexOf("=:")+2).toInt();
        }
        message = "Door width set to " + String(doorwidth) + " //This variable denotes the maximum width of the area over which you are trying to detect objects (cm) default 200";
      }else if (c.startsWith("DC+trigger")){
        if (c.indexOf("=:") != -1){
          trigger = c.substring(c.indexOf("=:")+2).toInt();
        }
        message = "Trigger point set to " + String(trigger) + " //This variable denotes the maximum distance an object can be from the module and still be recognized (cm) default 90";
      }else if (c.startsWith("DC+pslow")){
        if (c.indexOf("=:") != -1){
          p_slow = c.substring(c.indexOf("=:")+2).toInt();
        }
        message = "Slow ping rate set to " + String(p_slow) + " //This variable denotes the standard ping rate for the module (ms) default 100";
      }else if (c.startsWith("DC+pfast")){
        if (c.indexOf("=:") != -1){
          p_fast = c.substring(c.indexOf("=:")+2).toInt();
        }
        message = "Fast ping rate set to " + String(p_fast) + " //This variable denotes the accelerated ping rate for detecting individuals in a group (ms) default 80";
      }else if (c.startsWith("DC+stand")){
        if (c.indexOf("=:") != -1){
          standp = c.substring(c.indexOf("=:")+2).toInt();
        }
        message = "Standing time set to " + String(standp) + " //This variable denotes the number of pings required to guarantee an object is in front of the module (pings) default 2";
       }else if (c.startsWith("DC+pass")){
        if (c.indexOf("=:") != -1){
          passp = c.substring(c.indexOf("=:")+2).toInt();
        }
        message = "Pass time set to " + String(passp) + " //This variable denotes the number of pings required to guarantee an object is no longer in front of the module (pings) default 3";
      }else if (c.startsWith("DC+data")){
        if (c.indexOf("=:") != -1){
          dout = c.substring(c.indexOf("=:")+2).toInt();
        }
        message = "Output preference set to " + String(dout) + " //This variable sets the data output style (1 - All data | 2 - Only positive readings) default 2";
      }else if (c.startsWith("DC+exit")){
        CMode = false;
        message = "Returning to default operation."; 
      }
      mySerial.println(message);
    }  
  }else{
    if (millis()-ntime >=1000){
      digitalWrite(9, LOW);
    }
    delay(pings);
    int dist = sonar.ping_cm();
    if(dout == 1 && !CMode){
      mySerial.println(dist);
    }
    if (!standing){
      if (dist<trigger){
        stand++;
        if (stand>=standp){
          standing = true;
          digitalWrite(9, HIGH);
          ntime = millis();
          pings = p_fast;
          mySerial.println("Patron: stand1, p_fast, 91");
        }
      }else{
        stand = 0;
      }
    }else{
      if (dist<trigger){
        pass=0;
      }else{
        pass++;
        if (pass>=passp){
          standing = false;
          pings = p_slow;
          mySerial.println("stand0,p_slow");
        }
      }
    }
  }
}

