
// Uncomment 29th line in ICM_20948_C.h  to enable DMP support.

#include <WiFi.h>
#include <PubSubClient.h>
#include "ICM_20948.h" // Click here to get the library: http://librarymanager/All#SparkFun_ICM_20948_IMU

/************************  START IMU and PEDAl VARIABLES ************************/

#define SERIAL_PORT Serial
#define WIRE_PORT Wire // Your desired Wire port.
#define AD0_VAL 1
#define Throttle_Sensor A0 

ICM_20948_I2C myICM; // Otherwise create an ICM_20948_I2C object

// declare globe variables
double pitch = 0.0;
int throttle_extract = 0;
int throttle = 0;
float throttle_float = 0.0;
/************************  END IMU and PEDAl VARIABLES ************************/

/************************  START MQTT VARIABLES ************************/
// WiFi
const char *ssid = "Alex4nd3r"; // Enter your WiFi name lemur
const char *password = "3104867361";  // Enter WiFi password lemur9473

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
  pinMode(Throttle_Sensor, INPUT);
  Serial.begin(115200);

    // Connecting to a WiFi network
    WiFi.begin(ssid, password);
    while (WiFi.status() != WL_CONNECTED) {
        delay(500);
        Serial.println("Connecting to WiFi..");
    }
    Serial.println("Connected to the WiFi network");
    //connecting to a mqtt broker
    client.setServer(mqtt_broker, mqtt_port);
    client.setCallback(callback);
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

    success &= (myICM.setDMPODRrate(DMP_ODR_Reg_Quat6, 0) == ICM_20948_Stat_Ok); // Set to the maximum

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

  
}

void callback(char *topic, byte *payload, unsigned int length) {
  Serial.print("Message arrived in topic: ");
  Serial.println(topic);
  Serial.print("Message:");
  for (int i = 0; i < length; i++) {
    Serial.print((char) payload[i]);
  }
  Serial.println();
  Serial.println("-----------------------");
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

            
            throttle_extract = analogRead(Throttle_Sensor); // read the analog Throttle input
            if ((throttle_extract >=1720) && (throttle_extract<=7000))

            {
              throttle = map(throttle_extract, 1720, 7000, 0, 50);
              throttle_float = (float)throttle/100;
            }
            else if ( (throttle_extract>7000) && (throttle_extract<=8191))
            {
              throttle = map(throttle_extract, 7000, 8191, 50, 100);
              throttle_float = (float)throttle/100; 
            }
           
            if (! isnan(pitch)) {
            SERIAL_PORT.print(F(" Steering Angle:"));
            SERIAL_PORT.println(pitch, 1);
            Serial.println(throttle_float);
     
            char drive_data[9];
            snprintf(drive_data, 9, "%d,%.2f", int(pitch) , throttle_float); 
            client.publish(topic, drive_data);
            client.loop();

            delay(300); // 0.3s is smooth enough
            }
        }
    }

    if (myICM.status != ICM_20948_Stat_FIFOMoreDataAvail) // If more data is available then we should read it right away - and not delay
    {
    delay(10);
    }
}