'''
reference: https://github.com/Uberi/speech_recognition/blob/010382b80267f0f7794169fccc8e875ee7da7c19/speech_recognition/__init__.py#L632
To runsuccessfully you need to run
pip install SpeechRecognition
brew install portaudio then pip install pyaudio
'''

import time
import speech_recognition as sr


def recognize_speech_from_mic(recognizer, microphone):
    if not isinstance(recognizer, sr.Recognizer):
        raise TypeError("`recognizer` must be `Recognizer` instance")

    if not isinstance(microphone, sr.Microphone):
        raise TypeError("`microphone` must be `Microphone` instance")

    with microphone as source:
        recognizer.adjust_for_ambient_noise(source)
        audio = recognizer.listen(source, phrase_time_limit = 2)

    response = {
        "success": True,
        "error": None,
        "transcription": None
    }

    try:
        response["transcription"] = recognizer.recognize_google(audio)
    except sr.RequestError:
        response["success"] = False
        response["error"] = "API unavailable"
    except sr.UnknownValueError:
        response["error"] = "Unable to recognize speech"
    return response


if __name__ == "__main__":
    WORDS = ["stop"]
    INFINITY = 9999999999999999999999
    PROMPT_LIMIT = 15

    recognizer = sr.Recognizer()
    microphone = sr.Microphone()

 
    word = str(WORDS[0])
    print('Speak Command')
    time.sleep(3)


    for i in range(INFINITY):
        for j in range(PROMPT_LIMIT):
            print('{} times command!'.format(i+1))
            content = recognize_speech_from_mic(recognizer, microphone)
            if content["transcription"]:
                break
            if not content["success"]:
                break
            print("I didn't catch that. What did you say?\n")

        if content["error"]:
            print("ERROR: {}".format(content["error"]))
            break

        print("{}".format(content["transcription"]))
        iteration_more = i < INFINITY - 1

    
        stop_is_correct = content["transcription"].lower() == word.lower()
        if stop_is_correct:
            print("script stop")
            break
        elif iteration_more:
            print("Speaking another command")