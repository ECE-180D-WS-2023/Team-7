import cv2 as cv
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



def face_detect_func(img):
    gray_img = cv.cvtColor(img, cv.COLOR_BGR2GRAY)
    face_detect = cv.CascadeClassifier('.\\haarcascade_frontalface_default.xml')
    face = face_detect.detectMultiScale(gray_img)
    if len(face) == 0:
        return -1
    else:
        for x, y, w, h in face:
            cv.rectangle(img, (x, y), (x + w, y + h), color=(0, 0, 255), thickness=1)
        return x


if __name__ == '__main__':

    parser = argparse.ArgumentParser()
    parser.add_argument("-t", "--topic", type=str, required=True)
    args = parser.parse_args()
    topic = args.topic

    vid = cv.VideoCapture(0)
    client = mqtt.Client()
    client.on_connect = on_connect
    client.on_disconnect = on_disconnect
    client.connect_async('mqtt.eclipseprojects.io')
    client.loop_start()

    def signal_handler(sig, frame):
        vid.release()
        cv.destroyAllWindows()
        client.loop_stop()
        client.disconnect()

    signal.signal(signal.SIGINT, signal_handler)

    last_mode = 'Attack Mode'

    while True:
        ret, frame = vid.read()
        x_coord = face_detect_func(frame)
        if x_coord == -1:
            new_mode = last_mode
        elif int(x_coord) > 250:
            new_mode = 'Defense Mode'
        else:
            new_mode = 'Attack Mode'
    
        client.publish(topic, new_mode, qos=1)

        last_mode = new_mode
        
        time.sleep(1)
    