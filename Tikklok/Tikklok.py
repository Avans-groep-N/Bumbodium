import pyodbc
import sys
import RPi.GPIO as GPIO
from time import sleep
from mfrc522 import SimpleMFRC522

# Maak een instantie van de MFRC522-klasse aan
mfrc522 = SimpleMFRC522()

try:
    while True:
        print("Place RFID-Card.")
        id, text = mfrc522.read()
        print("Card read; UserID: " + text)
        query = "SELECT * FROM [BumbodiumDB].[dbo].[Presence] WHERE EmployeeId = '{}' ORDER BY [PresenceId] DESC OFFSET 0 ROWS FETCH NEXT 1 ROWS ONLY".format(text)
           
        #Maak een verbinding met de MSSQL-database
        conn = pyodbc.connect('DRIVER={ODBC Driver 18 for SQL Server};'
                              'SERVER=tcp:bumbodium.database.windows.net,1433;'
                              'DATABASE=bumbodiumDB;'
                              'UID=BumbodiumAdmin;'
                              'PWD=NietBumboAdmin!;'
                              'ENCRYPT=yes;'
                              'TrustServerCertificate=no;'
                              'Connection Timeout=30')

        # Maak een cursor aan
        cursor = conn.cursor()

        # Voer de query uit
        cursor.execute(query)

        num_rows = cursor.rowcount

        if num_rows != 0:

            # Haal de resultaten op
            results = cursor.fetchall()

            # Doorloop de resultaten
            for row in results:
                # Uitklokken
                if row[3] == None:
                    update_query = "UPDATE Presence SET ClockOutDateTime = CURRENT_TIMESTAMP WHERE PresenceId = '{}'".format(row[0])
                    cursor.execute(update_query)
                    conn.commit()
                    print("Succesfully clocked-out!")
                # Inklokken
                else:
                    query = "INSERT INTO Presence (EmployeeId, ClockInDateTime, IsSick) VALUES ('{}', CURRENT_TIMESTAMP, 0)".format(text)
                    cursor.execute(query)
                    conn.commit()
                    print("Succesfully clocked-in!")

        else:
            query = "SELECT * FROM [BumbodiumDB].[dbo].[Employee] WHERE EmployeeID = '{}'".format(text)

            # Voer de query uit
            cursor.execute(query)

            num_rows = cursor.rowcount

            if num_rows != 0:
                query = "INSERT INTO Presence (EmployeeId, ClockInDateTime, IsSick) VALUES ('{}', CURRENT_TIMESTAMP, 0)".format(text)
                cursor.execute(query)
                conn.commit()
                print("Succesfully clocked-in!")
            else:
                print("User not known in database.")

        # Sluit de cursor en de verbinding
        cursor.close()
        conn.close()
        sleep(3)

except KeyboardInterrupt:
    GPIO.cleanup()
    raise