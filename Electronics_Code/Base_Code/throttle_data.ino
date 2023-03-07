#define Throttle_Sensor A0

int throttle_extract = 0;

void setup() {
   pinMode(Throttle_Sensor, INPUT);
   Serial.begin(9600);
}
void loop() {
  throttle_extract = analogRead(Throttle_Sensor); // read the analog Throttle input
  int throttle = map(throttle_extract, 2690, 8191, 0, 100);
  float throttle_float = (float)throttle/100;
  Serial.println(throttle_float);

}