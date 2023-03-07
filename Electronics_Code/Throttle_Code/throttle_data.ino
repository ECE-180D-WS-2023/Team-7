#define Throttle_Sensor A0

int Val1 = 0;

void setup() {
   pinMode(Throttle_Sensor, INPUT);
   Serial.begin(9600);
}
void loop() {
  Val1 = analogRead(Throttle_Sensor); // read the analog Throttle input
  int throttle = map(Val1, 2690, 8191, 0, 100);
  float throttle_float = (float)throttle/100;
  Serial.println(throttle_float);

}