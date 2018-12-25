
#define PIN_HUMIDITY_SENSOR A1 
#define PIN_TEMPERATURE_SENSOR A0  

int chamber = 1000;
float temperature;
float humidity;

void setup(){
   Serial.begin(9600);
}
void loop(){
  //Sensor temperature
  int sensorTemperature = analogRead(PIN_TEMPERATURE_SENSOR);
  temperature = (350 - sensorTemperature) / 6.5;

  //Sensor humidity
  int sensorHumidity = analogRead(PIN_HUMIDITY_SENSOR);
  humidity = (1024 - sensorHumidity) / 8.55;
   
  if(humidity > 100){
    humidity = 100;
  }
   
  if(humidity < 0){
    humidity = 0;
  }
  
  //Output data
  outData(chamber, temperature, humidity);
  
  delay(15000);
}

void outData(int chamber, float temperature, float humidity){
  Serial.print("{IdFridge:");
  Serial.print(chamber);
  Serial.print(", Temperature:");
  Serial.print(temperature);
  Serial.print(", Humidity:");
  Serial.print(humidity);
  Serial.println("}");
}
