import cv2 as cv
import time
import signal
import keyboard as kbd # importing keyboard module



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

    curr_mode = 'Attack'

    vid = cv.VideoCapture(0)

    def signal_handler(sig, frame):
        vid.release()
        cv.destroyAllWindows()

    signal.signal(signal.SIGINT, signal_handler)

    while True:
        ret, frame = vid.read()
        x_coord = face_detect_func(frame)
        if int(x_coord) > 250:
            new_mode = 'Defense'
        else:
            new_mode = 'Attack'
    
        if new_mode != curr_mode:
            kbd.press_and_release('s')
            curr_mode = new_mode

        time.sleep(1)
    