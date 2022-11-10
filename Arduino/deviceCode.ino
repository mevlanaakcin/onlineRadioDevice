#include <Arduino.h>
#ifdef ESP32
#include <WiFi.h>
#else
#include <ESP8266WiFi.h>
#endif
#include"EEPROM.h"
#include<String.h>
#include "AudioFileSourceICYStream.h"
#include "AudioFileSourceBuffer.h"
#include "AudioGeneratorMP3.h"
#include "AudioOutputI2S.h"
#include "WPS.h"

#define button 5
#define serialButon 4
#define led 12
#define linkled 14
WPS wps(button, led);

char ssid[50];
char password[50];
char URL[100] ;
String WiFidata;
String eepromSsid;
String eepromPass;
String eepromLink;
boolean serial = false;

String okunan = "";
AudioGeneratorMP3 *mp3;
AudioFileSourceICYStream *file;
AudioFileSourceBuffer *buff;
AudioOutputI2S *out;

// Called when a metadata event occurs (i.e. an ID3 tag, an ICY block, etc.
void MDCallback(void *cbData, const char *type, bool isUnicode, const char *string)
{
  const char *ptr = reinterpret_cast<const char *>(cbData);
  (void) isUnicode; // Punt this ball for now
  // Note that the type and string may be in PROGMEM, so copy them to RAM for printf
  char s1[32], s2[64];
  strncpy_P(s1, type, sizeof(s1));
  s1[sizeof(s1) - 1] = 0;
  strncpy_P(s2, string, sizeof(s2));
  s2[sizeof(s2) - 1] = 0;
  //Serial.printf("METADATA(%s) '%s' = '%s'\n", ptr, s1, s2);
  //Serial.flush();
}

// Called when there's a warning or error (like a buffer underflow or decode hiccup)
void StatusCallback(void *cbData, int code, const char *string)
{
  const char *ptr = reinterpret_cast<const char *>(cbData);
  // Note that the string may be in PROGMEM, so copy it to RAM for printf
  char s1[64];
  strncpy_P(s1, string, sizeof(s1));
  s1[sizeof(s1) - 1] = 0;
  //Serial.printf("STATUS(%s) '%d' = '%s'\n", ptr, code, s1);
  //Serial.flush();
}

String readwifidata() {
  String wifiinfo = "";
  for (int i = 0; i < 128; i++) {
    char oku = EEPROM.read(i);
    wifiinfo += oku;
  }
  EEPROM.commit();
  return wifiinfo;
}
String readnamedata() {
  String namedata = "";
  for (int i = 128; i < 256; i++) {
    char d = EEPROM.read(i);

    namedata += d;
  }
  EEPROM.commit();
  return namedata;

}
String readlinkdata() {
  String linkdata = "";
  for (int i = 256; i < 512; i++) {
    char d = EEPROM.read(i);

    linkdata += d;
  }
  EEPROM.commit();
  return linkdata;
}


void clearwifi() {
  for (int i = 0; i < 128; i++) {
    EEPROM.write(i, 0);
  }
  EEPROM.commit();
}
void clearname() {
  for (int i = 128; i < 256; i++) {
    EEPROM.write(i, 0);
  }
  EEPROM.commit();

}
void clearlink() {
  for (int i = 256; i < 512; i++) {
    EEPROM.write(i, 0);
  }
  EEPROM.commit();
}

void setup() {
  Serial.begin(115200);
  EEPROM.begin(512);

  pinMode(serialButon, INPUT);
  pinMode(linkled, OUTPUT);
  digitalWrite(linkled, LOW);

  int butonDeger = digitalRead(serialButon);
  delay(80);
  if (butonDeger == HIGH) {
    serial = true;
  } else {
    serial = false;
  }

  while (serial) { // serial=true iletişim var ise while döngüsüne gir masaüstü ile iletişim kuruluyor

    if (Serial.available() > 0) {
      okunan = Serial.readStringUntil('\n');

      if (okunan[0] == '#') {
        for (int i = 0; i < okunan.length(); i++) {
          EEPROM.write(i, okunan[i]);
        }
        okunan = "";
      }
      else if (okunan[0] == '%') {
        String c = readwifidata();
        int t = c.length();
        int uzunluk = t + okunan.length();
        int j = 0;
        for (t; t < uzunluk; t++) {
          EEPROM.write(t, okunan[j]); 
          j++;
        }
        okunan = "";

      }
      else if (okunan[0] == '*') {
        String a = readwifidata();
        String b = readnamedata();
        int s = a.length() + b.length();
        int uzunluk = s + okunan.length();
        int j = 0;
        for (s; s < uzunluk; s++) {
          EEPROM.write(s, okunan[j]);
          j++;
        }
        okunan = "";
      }

      else  if (okunan[0] == '1') {
        Serial.println(readwifidata());
      }
      else if (okunan[0] == '2') {
        Serial.println(readnamedata());
      }
      else  if (okunan[0] == '3') {
        Serial.println(readlinkdata());
      }
      else if (okunan[0] == '4') {
        clearwifi();
      }
      else if (okunan[0] == '5') {
        clearname();
      }
      else if (okunan[0] == '6') {
        clearlink();
      }
      else if (okunan[0] == 'W') {
        serial = false;
      }
      else {
        okunan = "";
      }

    }
  }
  //**************************************************************************************

  WiFidata = readwifidata();
  if (WiFidata[0] != '#') { // bu ife girerse WPS bağlantı yapacaktır
   //Serial.println("WPS bağlantı gerçekleşicektir");
   // Serial.println(readnamedata());
    WiFi.mode(WIFI_STA);
    WiFi.begin("", "");
    wps.handleClient();
  }
  else {
    //Serial.println("SSID ve PASSWORD ile bağlantı gerçekleşicektir");
    int cont = 0;
    for (int i = 0; i < WiFidata.length(); i++) {
      if (WiFidata[i] == '#' ) {
        cont = 1;
      }
      if (WiFidata[i] == ':') {
        cont = 2;
      }
      if (WiFidata[i] != '#' && cont == 1) {
        eepromSsid += WiFidata[i];
      }
      if (WiFidata[i] != ':' && cont == 2) {
        eepromPass += WiFidata[i];
      }

    }
    // String char dönüşümü yaptık
    eepromSsid.toCharArray(ssid, 50);
    eepromPass.toCharArray(password, 50);


    WiFi.disconnect();
    WiFi.softAPdisconnect(true);
    WiFi.mode(WIFI_STA);
    //Serial.print("SSID: "); Serial.println(ssid);
    //Serial.print("PASSWORD: "); Serial.println(password);
    WiFi.begin(ssid, password);

    // Try forever
    while (WiFi.status() != WL_CONNECTED) {
      //Serial.println("...Connecting to WiFi");
      digitalWrite(led, 1);
      delay(500);
      digitalWrite(led, 0);
      delay(500);
    }

  }

  //eepromLink de linkin başındaki * a bakıyoruz
  eepromLink = readlinkdata();
  String a;
  for (int i = 1; i < eepromLink.length(); i++) {
    a += eepromLink[i];
  }
  a.toCharArray(URL, 100);

  //Ekrana çıktılar alıyorum

  //Serial.print("LINK: "); Serial.println(URL);


  audioLogger = &Serial;
  file = new AudioFileSourceICYStream(URL);
  file->RegisterMetadataCB(MDCallback, (void*)"ICY");
  buff = new AudioFileSourceBuffer(file, 8192);
  buff->RegisterStatusCB(StatusCallback, (void*)"buffer");
  out = new AudioOutputI2S();
  mp3 = new AudioGeneratorMP3();
  mp3->RegisterStatusCB(StatusCallback, (void*)"mp3");
  mp3->begin(buff, out);


}
void(* resetFunc) (void) = 0; //declare reset function @ address 0

void loop() {



  static int lastms = 0;

  if (mp3->isRunning()) { //müzik çalıyor ise
    if (millis() - lastms > 1000) {
      lastms = millis();
      //Serial.printf("Running for %d ms...\n", lastms);
      //Serial.flush();
    }
    if (!mp3->loop()) mp3->stop();
  }
  else { // müzik çalmıyor ise
    //Serial.printf("MP3 done\n");
    delay(1000);

    if (WiFi.status() != WL_CONNECTED) {
      wps.handleClient();
      audioLogger = &Serial;
      file = new AudioFileSourceICYStream(URL);
      file->RegisterMetadataCB(MDCallback, (void*)"ICY");
      buff = new AudioFileSourceBuffer(file, 8192);
      buff->RegisterStatusCB(StatusCallback, (void*)"buffer");
      out = new AudioOutputI2S();
      mp3 = new AudioGeneratorMP3();
      mp3->RegisterStatusCB(StatusCallback, (void*)"mp3");
      mp3->begin(buff, out);
    }
    if (URL[0] != 'h') {
      digitalWrite(linkled, HIGH);
    }

    else {
      digitalWrite(linkled, LOW);
      resetFunc();
    }
  }
}
