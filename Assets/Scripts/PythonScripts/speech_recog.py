import speech_recognition as sr
import paho.mqtt.client as mqtt
import argparse
import signal
import time

skill_one_cloud = ["skill one", "skill 1", "skil one", "skil 1", "one", "1", "still one", "stillwell", "still water", "diy", "dear one", "deal one","dear one", "gil wife","gil wine","gill won", "steal one", "steel one", "gill one", "gear one", "scale one", "scale 1","kilwins", "scale w", "pier one", "skill-wise", "dr1" ]
skill_two_cloud = ["skill two", "skill 2", "guity", "skill to", "skilled two", "skilled to", "skilled 2", "two", "to", "2", "skil to", "skil 2", "schooltube", "school 2", "school to", "forget you","gear 2","youtube", "co2", "steel 2", "u2","bo2","due to", "steel to", "still 2", "still two", "still to", "still two", "sio2", "forget to", "tokyo 2", "get you", "dr2"]
skill_three_cloud = ["skill three", "skill 3", "skills 3", "skills three", "skillz three", "skillz 3", "three", "3", "sklz 3", "sweet","suite","give 3", "gear s3", "po3", "scare street", "steele street","still 3", "steel 3", "steal 3", "gill three", "gill street", "gear street", "dusri", "deuce reid", "sd3", "geer street", "deer street","view street", "scary street", "gyro street","ustream","stream 3","stream"]

# MQTT related
def on_connect(client, userdata, flags, rc):
  print("Connection returned result: " + str(rc))

def on_disconnect(client, userdata, rc):
  if rc != 0:
    print('Unexpected Disconnect')
  else:
    print('Expected Disconnect')

# speech recog callback
def callback(recognizer, audio):
    #{'alternative': [{'transcript': 'skills 3', 'confidence': 0.81178844}, {'transcript': 'skills three'}, 
    #{'transcript': 'Skill 3'}, {'transcript': 'skill three'}, {'transcript': 'skills III'}], 'final': True}
    try:
        res_dict = recognizer.recognize_google(audio, show_all=True)
        print(res_dict)
        if isinstance(res_dict, list):
            return
        output = None

        predicted = []
        for transcript_dict in res_dict['alternative']:
            predicted.append(transcript_dict['transcript'])

        for each_predicted in predicted:
            if each_predicted.lower() in skill_one_cloud:
                output = 'skill 1'
                break
            elif each_predicted.lower() in skill_two_cloud:
                output = 'skill 2'
                break
            elif each_predicted.lower() in skill_three_cloud:
                output = 'skill 3'
                break
        
        if output != None:
            print(output)
            client.publish(topic, output, qos=1)
    except sr.UnknownValueError:
        print("Google Speech Recognition could not understand audio")
    except sr.RequestError as e:
        print("Could not request results from Google Speech Recognition service; {0}".format(e))


if __name__ == '__main__':

    # get topic from input
    parser = argparse.ArgumentParser()
    parser.add_argument("-t", "--topic", type=str, required=True)
    args = parser.parse_args()
    global topic
    topic = args.topic

    # initialize mqtt client
    global client
    client = mqtt.Client()
    client.on_connect = on_connect
    client.on_disconnect = on_disconnect
    client.connect_async('mqtt.eclipseprojects.io')
    client.loop_start()

    # register sigint to clear everything
    def signal_handler(sig, frame):
        stop_listening(wait_for_stop=False)
        client.loop_stop()
        client.disconnect()
        exit()
    signal.signal(signal.SIGINT, signal_handler)

    # initialize recognizers
    r = sr.Recognizer()
    m = sr.Microphone()
    with m as source:
        r.adjust_for_ambient_noise(source)  # we only need to calibrate once, before we start listening

    stop_listening = r.listen_in_background(m, callback, phrase_time_limit=2.0)

    # enter loop
    while True:
        time.sleep(10)
        
# SKILL 1
# {'alternative': [{'transcript': 'DIY', 'confidence': 0.80940437}, {'transcript': 'deal one'}, {'transcript': 'dear one'}], 'final': True}
# {'alternative': [{'transcript': 'Gil wife', 'confidence': 0.29084378}, {'transcript': 'Gill won'}, {'transcript': 'DIY'}, {'transcript': 'Gill Wife'}, {'transcript': 'Gil wine'}], 'final': True}
# {'alternative': [{'transcript': 'still one', 'confidence': 0.71840119}, {'transcript': 'steal one'}, {'transcript': 'steel one'}, {'transcript': 'steel 1'}, {'transcript': 'still 1'}], 'final': True}
# skill 1
# {'alternative': [{'transcript': 'Gill won', 'confidence': 0.37392741}, {'transcript': 'gr1'}, {'transcript': 'Gil 1'}, {'transcript': 'Gear 1'}, {'transcript': 'Gil one'}], 'final': True}
# {'alternative': [{'transcript': 'skill-wise', 'confidence': 0.84499335}, {'transcript': 'Kilwins'}, {'transcript': 'scale w'}, {'transcript': 'scale 1'}], 'final': True}
# {'alternative': [{'transcript': 'steal one', 'confidence': 0.61468828}, {'transcript': 'still one'}, {'transcript': 'steel one'}, {'transcript': 'Pier One'}, {'transcript': 'skill one'}], 'final': True}
# skill 1
# {'alternative': [{'transcript': 'deal one', 'confidence': 0.78467011}, {'transcript': 'still one'}, {'transcript': 'deal 1'}, {'transcript': 'dr1'}, {'transcript': 'dear one'}], 'final': True}
# skill 1
# {'alternative': [{'transcript': 'steal one', 'confidence': 0.48739195}, {'transcript': 'still one'}, {'transcript': 'steel one'}, {'transcript': 'steel 1'}, {'transcript': 'Stihl 1'}], 'final': True}

# SKILL 2
# {'alternative': [{'transcript': 'forget you', 'confidence': 0.28066948}, {'transcript': 'get you'}, {'transcript': 'Gear 2'}, {'transcript': 'get to'}, {'transcript': 'YouTube'}], 'final': True}
# {'alternative': [{'transcript': 'Gear 2', 'confidence': 0.57748866}, {'transcript': 'dr2'}, {'transcript': 'CO2'}, {'transcript': 'YouTube'}], 'final': True}
# {'alternative': [{'transcript': 'YouTube', 'confidence': 0.78668207}, {'transcript': 'sicario 2'}, {'transcript': 'spaghetti'}, {'transcript': 'Gear 2'}], 'final': True}
# {'alternative': [{'transcript': 'Gear 2', 'confidence': 0.81261635}, {'transcript': 'dr2'}], 'final': True}
# {'alternative': [{'transcript': 'CO2', 'confidence': 0.98762906}, {'transcript': 'Gear 2'}, {'transcript': 'forget you'}, {'transcript': 'forget to'}, {'transcript': 'Tokyo 2'}], 'final': True}
# {'alternative': [{'transcript': 'CO2', 'confidence': 0.81477416}, {'transcript': 'still too'}, {'transcript': 'go2'}, {'transcript': 'sio2'}, {'transcript': 'still to'}], 'final': True}
# {'alternative': [{'transcript': 'YouTube', 'confidence': 0.96026528}, {'transcript': 'Gear 2'}], 'final': True}
# {'alternative': [{'transcript': 'CO2', 'confidence': 0.36185354}, {'transcript': 'BO2'}, {'transcript': 'U2'}, {'transcript': '302'}, {'transcript': 'due to'}], 'final': True}
# {'alternative': [{'transcript': 'Steel 2', 'confidence': 0.37799221}, {'transcript': 'still too'}, {'transcript': 'CO2'}, {'transcript': 'steel to'}, {'transcript': 'still to'}], 'final': True}

# SKILL 3
# {'alternative': [{'transcript': 'sweet', 'confidence': 0.85867363}, {'transcript': 'Suite'}], 'final': True}
# {'alternative': [{'transcript': 'give three', 'confidence': 0.60665071}, {'transcript': 'po3'}, {'transcript': 'give 3'}, {'transcript': 'gifts 3'}, {'transcript': 'gifs free'}], 'final': True}
# {'alternative': [{'transcript': 'gear S3', 'confidence': 0.67358601}, {'transcript': 'Gear 3'}, {'transcript': 'Gears 3'}, {'transcript': 'PS3'}, {'transcript': 'give three'}], 'final': True}
# {'alternative': [{'transcript': 'po3', 'confidence': 0.50451797}, {'transcript': 'CO3'}, {'transcript': 'PS3'}, {'transcript': 'killstream'}, {'transcript': 'give three'}], 'final': True}
# {'alternative': [{'transcript': 'scare Street', 'confidence': 0.36200419}, {'transcript': 'Skate 3'}, {'transcript': 'scarce tree'}, {'transcript': 'Skiff Street'}, {'transcript': 'scarce read'}], 'final': True}
# {'alternative': [{'transcript': 'Steele Street', 'confidence': 0.32976329}, {'transcript': 'Geer Street'}, {'transcript': 'Deer Street'}, {'transcript': 'Gill Street'}, {'transcript': 'Gear street'}], 'final': True}
# {'alternative': [{'transcript': 'dusri', 'confidence': 0.79593021}, {'transcript': 'View Street'}, {'transcript': 'do Street'}, {'transcript': 'DS3'}, {'transcript': 'Deuce Reid'}], 'final': True}
# {'alternative': [{'transcript': 'Geer Street', 'confidence': 0.66592222}, {'transcript': 'skier Street'}, {'transcript': 'scary Street'}, {'transcript': 'gyro Street'}, {'transcript': 'Steele Street'}], 'final': True}
# {'alternative': [{'transcript': 'still', 'confidence': 0.47670799}, {'transcript': 'Steel'}, {'transcript': 'Stihl'}, {'transcript': 'steal'}, {'transcript': "Steele's"}], 'final': True}
# {'alternative': [{'transcript': '3', 'confidence': 0.87736654}, {'transcript': 'three'}, {'transcript': 'free'}, {'transcript': '3:00'}], 'final': True}
# skill 3
# {'alternative': [{'transcript': 'steel 3', 'confidence': 0.56199098}, {'transcript': 'still three'}, {'transcript': 'still 3'}, {'transcript': 'Stihl 3'}, {'transcript': 'steel three'}], 'final': True}
