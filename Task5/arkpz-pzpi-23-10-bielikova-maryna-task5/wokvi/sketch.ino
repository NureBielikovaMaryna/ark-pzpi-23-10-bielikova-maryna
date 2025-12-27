#include <WiFi.h>
#include <HTTPClient.h>
#include <WiFiClientSecure.h>
#include <time.h>

const char* ssid = "Wokwi-GUEST"; 
const char* password = "";

// путь
const char* serverUrl = "https://aviana-nonimputable-mitigatedly.ngrok-free.dev/api/Sensor/update";
const int sensorPin = 12;

void setup() {
  Serial.begin(115200);
  pinMode(sensorPin, INPUT_PULLUP);// Налаштування піна датчика
   
   // Підключення до мережі
  Serial.print("Connecting to WiFi");
  WiFi.begin(ssid, password);
  while (WiFi.status() != WL_CONNECTED) {
    delay(500);
    Serial.print(".");
  }
  Serial.println("\nWiFi Connected!");

  // Налаштування часового поясу (Київ)
  configTime(7200, 3600, "pool.ntp.org"); 
}

void loop() {
  if (digitalRead(sensorPin) == LOW) {
    Serial.println("Train detected at station!");
    sendData();
    delay(10000); 
  }
}

void sendData() {
  if (WiFi.status() == WL_CONNECTED) {
    WiFiClientSecure *client = new WiFiClientSecure;
    if(client) {
      client->setInsecure();

      HTTPClient http;
      
      // Формування мітки часу
      struct tm timeinfo;
      if(!getLocalTime(&timeinfo)){
        Serial.println("Failed to obtain time, using default 22:45");
      }
      char timeString[6];
      strftime(timeString, 6, "%H:%M", &timeinfo);

      // Формування запиту
      // Об'єднання ID поїзда, ID станції та реального часу прибуття
      String fullUrl = String(serverUrl) + "?trainId=1&stationId=2&actualTime=" + timeString;
      
      Serial.println("Requesting URL: " + fullUrl);
      
      if (http.begin(*client, fullUrl)) {
        // защита нгрок
        http.addHeader("ngrok-skip-browser-warning", "69420");
        http.addHeader("User-Agent", "ESP32-Sensor-Client");

        int httpResponseCode = http.POST(""); 

        // Відправка та обробка відповіді
        if (httpResponseCode > 0) {
          Serial.print("HTTP Response code: ");
          Serial.println(httpResponseCode); 
          Serial.println("Server Response: " + http.getString());
        } else {
          Serial.print("Error code: ");
          Serial.println(http.errorToString(httpResponseCode).c_str());
        }
        http.end();
      }
      delete client;
    }
  }
}