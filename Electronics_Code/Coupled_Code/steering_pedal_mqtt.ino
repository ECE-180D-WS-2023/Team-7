

// Uncomment 29th line in ICM_20948_C.h  to enable DMP support.

#include <WiFi.h>
#include <PubSubClient.h>
#include "ICM_20948.h" // Click here to get the library: http://librarymanager/All#SparkFun_ICM_20948_IMU
#include <Wire.h>
#include <SparkFun_Alphanumeric_Display.h>  //Click here to get the library: http://librarymanager/All#SparkFun_Qwiic_Alphanumeric_Display by SparkFun


/************************  START IMU and PEDAl VARIABLES ************************/

#define SERIAL_PORT Serial
#define AD0_VAL 1
#define Throttle_Sensor A0 
#define FORWARD_B 11
#define WIRE_PORT Wire // Your desired Wire port.

HT16K33 display;
ICM_20948_I2C myICM; // Otherwise create an ICM_20948_I2C object

// declare globe variables
double pitch = 0.0;
int throttle_extract = 0;
int throttle = 0;
float throttle_float = 0.0;


// THIS IS SUBJECT TO CHANGE
int MSG_INTERVAL = 30;
int COUNTER = 0;

char MSG[10];

bool DEBUG = false;
/************************  END IMU and PEDAl VARIABLES ************************/

/************************  START MQTT VARIABLES ************************/
// WiFi
const char *ssid = ""; // Enter your WiFi name lemur
const char *password = "";  // Enter WiFi password lemur9473

// MQTT Broker
const char *mqtt_broker = "mqtt.eclipseprojects.io";
const char *topic = "ECE180D/team7/1";
const int mqtt_port = 1883;

WiFiClient espClient;
PubSubClient client(espClient);
/************************  END MQTT VARIABLES ************************/



void setup()
{
    SERIAL_PORT.begin(115200); 
    Serial.begin(115200);
    //Wire.begin();
    pinMode(Throttle_Sensor, INPUT);
    pinMode(FORWARD_B, INPUT_PULLUP);
    // Connecting to a WiFi network
    WiFi.begin(ssid, password);
    while (WiFi.status() != WL_CONNECTED) {
        delay(500);
        Serial.println("Connecting to WiFi..");
    }
    Serial.println("Connected to the WiFi network");
    //connecting to a mqtt broker
    client.setServer(mqtt_broker, mqtt_port);
    while (!client.connected()) {
        String client_id = "esp32-client-";
        client_id += String(WiFi.macAddress());
        Serial.printf("The client %s connects to the public mqtt broker\n", client_id.c_str());
        if (client.connect(client_id.c_str())) { //, mqtt_username, mqtt_password)) {
            Serial.println("mqtt broker connected");
        } else {
            Serial.print("failed with state ");
            Serial.print(client.state());
            delay(2000);
        }
    }

    SERIAL_PORT.println(F("Functionality: Calculate Send Steering Angles Through MQTT"));

    delay(100);

    while (SERIAL_PORT.available()) // Make sure the serial RX buffer is empty
        SERIAL_PORT.read();

    SERIAL_PORT.println(F("Press any key to continue..."));

    while (!SERIAL_PORT.available()) // Wait for the user to press a key (send any serial character)
        ;

    WIRE_PORT.begin();
    WIRE_PORT.setClock(400000);

    bool initialized = false;
    while (!initialized)
    {

        // Initialize the ICM-20948
        // If the DMP is enabled, .begin performs a minimal startup. We need to configure the sample mode etc. manually.
        myICM.begin(WIRE_PORT, AD0_VAL);

        SERIAL_PORT.print(F("Initialization of the sensor returned: "));
        SERIAL_PORT.println(myICM.statusString());

        if (myICM.status != ICM_20948_Stat_Ok)
        {
        SERIAL_PORT.println(F("Trying again..."));
        delay(500);
        }
        else
        {
        initialized = true;
        }
    }

    SERIAL_PORT.println(F("Device connected!"));

    bool success = true; // Use success to show if the DMP configuration was successful

    // Initialize the DMP. initializeDMP is a weak function. You can overwrite it if you want to e.g. to change the sample rate
    success &= (myICM.initializeDMP() == ICM_20948_Stat_Ok);

    // Enable the DMP Game Rotation Vector sensor
    success &= (myICM.enableDMPSensor(INV_ICM20948_SENSOR_GAME_ROTATION_VECTOR) == ICM_20948_Stat_Ok);
    
    // Change sampling rate !!FIFO SHOULD NOT BE FILLED UP
    success &= (myICM.setDMPODRrate(DMP_ODR_Reg_Quat6, 20) == ICM_20948_Stat_Ok); 

    // Enable the FIFO
    success &= (myICM.enableFIFO() == ICM_20948_Stat_Ok);

    // Enable the DMP
    success &= (myICM.enableDMP() == ICM_20948_Stat_Ok);

    // Reset DMP
    success &= (myICM.resetDMP() == ICM_20948_Stat_Ok);

    // Reset FIFO
    success &= (myICM.resetFIFO() == ICM_20948_Stat_Ok);

    // Check success
    if (success)
    {
        SERIAL_PORT.println(F("DMP enabled!"));
    }
    else
    {
        SERIAL_PORT.println(F("Enable DMP failed!"));
        SERIAL_PORT.println(F("Please check that you have uncommented line 29 (#define ICM_20948_USE_DMP) in ICM_20948_C.h..."));
        while (1)
        ; // Do nothing more
    }

    if (display.begin() == false)
    {
      Serial.println("Device did not acknowledge! Freezing.");
      while(1);
    }
    Serial.println("Display acknowledged.");
}


void loop()
{
    icm_20948_DMP_data_t data;
    myICM.readDMPdataFromFIFO(&data);

    if ((myICM.status == ICM_20948_Stat_Ok) || (myICM.status == ICM_20948_Stat_FIFOMoreDataAvail)) // Was valid data available?
    {

        if ((data.header & DMP_header_bitmap_Quat6) > 0) // We have asked for GRV data so we should receive Quat6
        {
            double q1 = ((double)data.Quat6.Data.Q1) / 1073741824.0; // Convert to double. Divide by 2^30
            double q2 = ((double)data.Quat6.Data.Q2) / 1073741824.0; // Convert to double. Divide by 2^30
            double q3 = ((double)data.Quat6.Data.Q3) / 1073741824.0; // Convert to double. Divide by 2^30

            // Convert the quaternions to Euler angles (roll, pitch, yaw)
            // https://en.wikipedia.org/w/index.php?title=Conversion_between_quaternions_and_Euler_angles&section=8#Source_code_2
            double q0 = sqrt(1.0 - ((q1 * q1) + (q2 * q2) + (q3 * q3)));
            double q2sqr = q2 * q2;

            // Pitch (y-axis rotation)
            double t2 = +2.0 * (q0 * q2 - q3 * q1);
            t2 = t2 > 1.0 ? 1.0 : t2;
            t2 = t2 < -1.0 ? -1.0 : t2;           
            pitch = asin(t2) * 180.0 / PI;

            float is_forward = digitalRead(FORWARD_B)?1:-1;

            throttle_extract = analogRead(Throttle_Sensor); // read the analog Throttle input

            if ((throttle_extract >=1720) && (throttle_extract<=7000))
            {
              throttle = map(throttle_extract, 2500, 7000, 0, 50);
              throttle_float = is_forward * (float)throttle/100;
            }
            else if ( (throttle_extract>7000) && (throttle_extract<=9000))
            {
              throttle = map(throttle_extract, 7000, 8000, 50, 100);
              throttle_float = is_forward * min((float)throttle/100, 1.0f); 
            }
            // // test display module
            // int positive_angle = max(pitch,-pitch);
            // int first;
            // int second;
            // if(positive_angle<0 || positive_angle>100){
            //   first=0;
            //   second=0;
            // }
            // else{
            //   second = positive_angle%10;
            //   first = (positive_angle - second)/10;
            // }
            // char first_char = first +'0';
            // char second_char = second + '0';
           
            if (isnan(pitch)){
                pitch = 0;
            }

            if (DEBUG){
                SERIAL_PORT.print(F(" Steering Angle:"));
                SERIAL_PORT.println(pitch, 1);
                SERIAL_PORT.println(throttle_float);
            }

            if (COUNTER % MSG_INTERVAL == 0){
                snprintf(MSG, 10, "%d,%.2f", int(pitch) , throttle_float); 
                client.publish(topic, MSG);
                client.loop();

                COUNTER = 0;

                // Display system
                display.clear();

                if(is_forward == 1.0f){
                  display.printChar('D', 0);
                }
                else{
                  display.printChar('R', 0);
                }

                display.printChar('0'+(int)(throttle/100)%10, 1);
                display.printChar('0'+(int)(throttle/10)%10, 2);
                display.printChar('0'+(int)throttle%10, 3);
                  
                display.updateDisplay();

            }
            else
            {
                COUNTER ++;
            }

            // delay blocks the entire loop, omitted
            // delay(50); 
        }
    }

    if (myICM.status != ICM_20948_Stat_FIFOMoreDataAvail) // If more data is available then we should read it right away - and not delay
    {
    delay(10);
    }
}