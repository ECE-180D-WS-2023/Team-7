#define FORWARD_B 11
#define REVERSE_B 12

void setup() {
  Serial.begin(115200);
  pinMode(FORWARD_B, INPUT_PULLUP);
  pinMode(REVERSE_B, INPUT_PULLUP);
}

int mode = 0;

void loop() {  
  if(digitalRead(FORWARD_B) == 0)
    mode = 1;
  else if(digitalRead(REVERSE_B) == 0)
    mode = -1;
  else
    Serial.println(mode);
}