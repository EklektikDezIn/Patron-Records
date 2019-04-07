#include <NewPing.h>
#include <Wire.h>
#include <SoftwareSerial.h>
#include <string.h>

int doorwidth = 200; //cm

NewPing sonar(12, 11, doorwidth);
SoftwareSerial mySerial(2, 3);

void setup() {
  pinMode(9, OUTPUT);
  mySerial.begin (115200);
  mySerial.println("Module Active");
}

void loop() {
    int dist = sonar.ping_cm();
      mySerial.println(dist);
}

