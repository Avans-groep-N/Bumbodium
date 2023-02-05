from time import sleep
import sys
from mfrc522 import SimpleMFRC522
smfrc = SimpleMFRC522()

try:
    while True:
        print("Enter the employeeID that you would like to link to a card.")
        enteredId = input()
        print("Hold a tag near the reader.")
        smfrc.write(enteredId)
        print("Card written!")
        sleep(5)
        id, text = smfrc.read()
        print("The following information is now written to the card:\nCardnumber: %s\nEmployeeID: %s" % (id,text))
        sleep(5)

except KeyboardInterrupt:
    GPIO.cleanup()
    raise