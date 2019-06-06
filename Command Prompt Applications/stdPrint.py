import csv
import fileinput


for line in fileinput.input():
    dataLine = line[43:]
    if(len(dataLine) > 0):
        if(dataLine.count("Position: ") > 0):
            print(dataLine)
            if(dataLine.strip()):
                with open("randnumbers.csv", "a") as f:
                    writer = csv.writer(f)
                    writer.writerow([dataLine])
