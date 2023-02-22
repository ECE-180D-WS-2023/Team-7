import cv2
import mediapipe as mp
import time
import signal
import paho.mqtt.client as mqtt
import argparse

def on_connect(client, userdata, flags, rc):
  print("Connection returned result: " + str(rc))

def on_disconnect(client, userdata, rc):
  if rc != 0:
    print('Unexpected Disconnect')
  else:
    print('Expected Disconnect')


if __name__ == '__main__':

    parser = argparse.ArgumentParser()
    parser.add_argument("-t", "--topic", type=str, required=True)
    args = parser.parse_args()
    topic = args.topic

    cap = cv2.VideoCapture(0)

    client = mqtt.Client()
    client.on_connect = on_connect
    client.on_disconnect = on_disconnect
    client.connect_async('mqtt.eclipseprojects.io')
    client.loop_start()

    def signal_handler(sig, frame):
        cap.release()
        cv2.destroyAllWindows()
        client.loop_stop()
        client.disconnect()

    signal.signal(signal.SIGINT, signal_handler)

    mp_face_detection = mp.solutions.face_detection
    mp_drawing = mp.solutions.drawing_utils

    # For webcam input:
    with mp_face_detection.FaceDetection(model_selection=0, min_detection_confidence=0.5) as face_detection:
        while cap.isOpened():
            success, image = cap.read()
            if not success:
                print("Ignoring empty camera frame.")
                # If loading a video, use 'break' instead of 'continue'.
                continue

            # To improve performance, optionally mark the image as not writeable to
            # pass by reference.
            image.flags.writeable = False
            image = cv2.cvtColor(image, cv2.COLOR_BGR2RGB)
            results = face_detection.process(image)

            # Draw the face detection annotations on the image.
            image.flags.writeable = True
            image = cv2.cvtColor(image, cv2.COLOR_RGB2BGR)
            if results.detections:
                # Left > 0.5
                # Right < 0.5
                # Coord of nose is used
                if (float(results.detections[0].location_data.relative_keypoints[2].x) < 0.5):
                    client.publish(topic, "Defense Mode", qos=1)
                else:
                    client.publish(topic, "Attack Mode", qos=1)

                time.sleep(1)

                