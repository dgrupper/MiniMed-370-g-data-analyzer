using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace MiniMed_370_g_data_analyzer
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create an instance of StreamReader to read from a file.
            // Create an instance of pumpRecord to read one row of insulin pump record data set
            // The using statement also closes the StreamReader.
            // Read and display lines from the file until the end of file

            var recordList = new List<PumpEntry>();// Each record is a row in dataset
            using (StreamReader pumpRecord = new StreamReader("MiniMed_Data.csv"))
            {
                string record;
                pumpRecord.ReadLine();//burn the header
                while ((record = pumpRecord.ReadLine()) != null) //loop to assign each row in csv file to an instance of class PumpEntry
                {
                    var arrayOfFieldsInThisRow = record.Split(',');
                    var pumpData = new PumpEntry();
                    pumpData.Index = arrayOfFieldsInThisRow[0];
                    pumpData.Date = arrayOfFieldsInThisRow[1];
                    pumpData.Time = arrayOfFieldsInThisRow[2];
                    pumpData.BG = Convert.ToInt32(arrayOfFieldsInThisRow[3]);
                    pumpData.ISIG = Convert.ToDouble(arrayOfFieldsInThisRow[4]);

                    recordList.Add(pumpData);
                }
            }

            int takeAction = 4;
            while (takeAction != 0)  // Loop till user enters 0 for quit
            {
                Console.WriteLine("");
                Console.WriteLine("This is the list of dates in the data file.");
                Console.WriteLine("");
                var dateList = new List<string>();
                var indexList = new List<string>();
                string currentRecordIndex = "0";
                string currentRecordDate = "7/9/2019";
                int counter = -1;
                int nextIndex = 0;

                foreach (var record in recordList)
                {
                    indexList.Add(record.Index);
                    if (record.Date == currentRecordDate)
                    {
                        counter += 1;
                    }
                    else
                    {
                        Console.WriteLine("Date: " + currentRecordDate + " Indexes " + currentRecordIndex + " through " + (Convert.ToInt32(currentRecordIndex) + counter));
                        currentRecordIndex = record.Index;
                        currentRecordDate = record.Date;
                        counter = 0;
                    }
                    if (dateList.Contains(record.Date) == false)
                    {
                        dateList.Add(record.Date);
                    }
                }
                nextIndex = (Convert.ToInt32(currentRecordIndex) + counter + 1);
                Console.WriteLine("Date: " + currentRecordDate + " Indexes " + currentRecordIndex + " through " + (Convert.ToInt32(currentRecordIndex) + counter));

                Console.WriteLine("");
                Console.WriteLine("Enter 0 to Quit");
                Console.WriteLine("Enter 1 to Create a new entry");
                Console.WriteLine("Enter 2 to Read an existing entry");
                Console.WriteLine("Enter 3 to update an existing entry");
                Console.WriteLine("");

                bool success = Int32.TryParse(Console.ReadLine(), out takeAction);

                if (!success || !((takeAction == 0) || (takeAction == 1) || (takeAction == 2) || (takeAction == 3)))  // test for valid input
                {
                    Console.WriteLine("Come on Brian! You had one job! Invalid input, please try again.");
                    continue;
                }

                if (takeAction == 1) // Create new entry
                {
                    Console.WriteLine("");
                    Console.WriteLine("A new entry has 5 fields: Index, Date, time, BG, ISIG.");
                    var newEntry = new PumpEntry();

                    bool successIndex = false;
                    int newIndex = 0;
                    while (!successIndex)  // test for valid input
                    {
                        Console.WriteLine("Please enter the index: " + nextIndex + " for your new entry.");
                        successIndex = Int32.TryParse(Console.ReadLine(), out newIndex);
                        if (!successIndex)
                        {
                            Console.WriteLine("Invalid input, please try again.");
                            continue;
                        }
                        if (newIndex != nextIndex)
                        {
                            Console.WriteLine("Sorry, you must use index " + nextIndex + ".");
                            successIndex = false;
                            continue;
                        }
                    }
                    newEntry.Index = Convert.ToString(newIndex);

                    bool successDate = false;
                    DateTime newDate = DateTime.Now;
                    while (!successDate)  // test for valid input
                    {
                        Console.WriteLine("Please enter the date m/dd/yyyy");
                        successDate = DateTime.TryParse(Console.ReadLine(), out newDate);
                        if (!successDate)
                        {
                            Console.WriteLine("Invalid input, please try again.");
                            continue;
                        }
                    }
                    newEntry.Date = Convert.ToString(newDate.ToShortDateString());

                    bool successTime = false;
                    DateTime newTime = DateTime.Now;
                    while (!successTime)  // test for valid input
                    {
                        Console.WriteLine("Please enter the time h:mm:ss");
                        successTime = DateTime.TryParse(Console.ReadLine(), out newTime);
                        if (!successTime)
                        {
                            Console.WriteLine("Invalid input, please try again.");
                            continue;
                        }
                    }
                    newEntry.Time = Convert.ToString(newTime.ToShortTimeString());

                    bool successBG = false;
                    int newBG = 0;
                    while (!successBG)  // test for valid input
                    {
                        Console.WriteLine("Please enter the BG as an integer");
                        successBG = Int32.TryParse(Console.ReadLine(), out newBG);
                        if (!successBG)
                        {
                            Console.WriteLine("Invalid input, please try again.");
                            continue;
                        }
                    }
                    newEntry.BG = newBG;

                    bool successISIG = false;
                    var newISIG = 0.0;
                    while (!successISIG)  // test for valid input
                    {
                        Console.WriteLine("Please enter the ISIG as an double");
                        successISIG = double.TryParse(Console.ReadLine(), out newISIG);
                        if (!successISIG)
                        {
                            Console.WriteLine("Invalid input, please try again.");
                            continue;
                        }
                    }
                    newEntry.ISIG = newISIG;
                    recordList.Add(newEntry);
                    continue;
                }

                if (takeAction == 2) //Read an existing entry
                {
                    bool chooseIndex = false;
                    string readIndexString = "";
                    int readIndex = 0;
                    while (!chooseIndex)  // test for valid input
                    {
                        Console.WriteLine("");
                        Console.WriteLine("Please enter the index of the entry you want to read.");
                        chooseIndex = Int32.TryParse(Console.ReadLine(), out readIndex);
                        if (!chooseIndex)
                        {
                            Console.WriteLine("Invalid input, please try again.");
                            continue;
                        }
                    }
                    readIndexString = Convert.ToString(readIndex);

                    foreach (var record in recordList)
                    {
                        if (record.Index == readIndexString)
                        {
                            Console.WriteLine("");
                            Console.WriteLine("At Index: " + record.Index);
                            Console.WriteLine("Date: " + record.Date);
                            Console.WriteLine("Time: " + record.Time);
                            Console.WriteLine("BG: " + record.BG);
                            Console.WriteLine("ISIG: " + record.ISIG);
                            break;
                        }
                    }
                }

                if (takeAction == 3) // update an entry
                {
                    bool chooseIndex = false;
                    string readIndexString = "";
                    int readIndex = 0;
                    while (!chooseIndex)  // test for valid input
                    {
                        Console.WriteLine("");
                        Console.WriteLine("Please enter the index of the entry you want to update.");
                        chooseIndex = Int32.TryParse(Console.ReadLine(), out readIndex);
                        if (!chooseIndex)
                        {
                            Console.WriteLine("Invalid input, please try again.");
                            continue;
                        }
                        if (indexList.Contains(Convert.ToString(readIndex)) == false)
                        {
                            Console.WriteLine("");
                            Console.WriteLine("There is no entry with that index. Please choose an index between 0 and " + (nextIndex - 1) + ".");
                            chooseIndex = false;
                            continue;
                        }
                    }
                    readIndexString = Convert.ToString(readIndex);

                    Console.WriteLine("");
                    Console.WriteLine("Do you want to update the Date for this entry? Y/N");
                    string DateYesNo = Console.ReadLine();

                    if (DateYesNo.ToUpper() == "Y")
                    {
                        bool changeDate = false;
                        DateTime updateDate = DateTime.Now;
                        while (!changeDate)  // test for valid input
                        {
                            Console.WriteLine("Please update the date m/dd/yyyy");
                            changeDate = DateTime.TryParse(Console.ReadLine(), out updateDate);
                            if (!changeDate)
                            {
                                Console.WriteLine("Invalid input, please try again.");
                                continue;
                            }
                        }
                        var updateDateString = Convert.ToString(updateDate.ToShortDateString());
                        foreach (var record in recordList)
                        {
                            if (record.Index == readIndexString)
                            {
                                record.Date = updateDateString;
                                break;
                            }
                        }
                    }

                    Console.WriteLine("");
                    Console.WriteLine("Do you want to update the Time for this entry? Y/N");
                    string TimeYesNo = Console.ReadLine();

                    if (TimeYesNo.ToUpper() == "Y")
                    {
                        bool changeTime = false;
                        DateTime updateTime = DateTime.Now;
                        while (!changeTime)  // test for valid input
                        {
                            Console.WriteLine("Please update the time h:mm:ss");
                            changeTime = DateTime.TryParse(Console.ReadLine(), out updateTime);
                            if (!changeTime)
                            {
                                Console.WriteLine("Invalid input, please try again.");
                                continue;
                            }
                        }
                        var updateTimeString = Convert.ToString(updateTime.ToShortTimeString());
                        foreach (var record in recordList)
                        {
                            if (record.Index == readIndexString)
                            {
                                record.Time = updateTimeString;
                                break;
                            }
                        }
                    }

                    Console.WriteLine("");
                    Console.WriteLine("Do you want to update the BG for this entry? Y/N");
                    string BGYesNo = Console.ReadLine();

                    if (BGYesNo.ToUpper() == "Y")
                    {
                        bool changeBG = false;
                        int updateBG = 0;
                        while (!changeBG)  // test for valid input
                        {
                            Console.WriteLine("Please enter the BG as an integer");
                            changeBG = Int32.TryParse(Console.ReadLine(), out updateBG);
                            if (!changeBG)
                            {
                                Console.WriteLine("Invalid input, please try again.");
                                continue;
                            }
                        }
                        foreach (var record in recordList)
                        {
                            if (record.Index == readIndexString)
                            {
                                record.BG = updateBG;
                                break;
                            }
                        }

                        Console.WriteLine("");
                        Console.WriteLine("Do you want to update the ISIG for this entry? Y/N");
                        string ISIGYesNo = Console.ReadLine();

                        if (ISIGYesNo.ToUpper() == "Y")
                        {
                            Console.WriteLine("Please enter the ISIG as a double");
                            bool changeISIG = false;
                            var updateISIG = 0.0;
                            while (!changeISIG)  // test for valid input
                            {
                                Console.WriteLine("Please enter the ISIG as an integer");
                                changeISIG = Double.TryParse(Console.ReadLine(), out updateISIG);
                                if (!changeISIG)
                                {
                                    Console.WriteLine("Invalid input, please try again.");
                                    continue;
                                }
                            }
                            foreach (var record in recordList)
                            {
                                if (record.Index == readIndexString)
                                {
                                    record.ISIG = updateISIG;
                                    break;
                                }
                            }
                        }
                    }
                }
                    var serializer = new JsonSerializer();
                    using (var writer = new StreamWriter("updated_MiniMed_Data"))
                    using (var jsonWriter = new JsonTextWriter(writer))
                    {
                        serializer.Serialize(jsonWriter, recordList);
                    }
                }

            }
        }
    }