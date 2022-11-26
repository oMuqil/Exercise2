using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;


namespace Exercise2
{
   
    internal class Program
    {

        static void Main(string[] args)
        {
            try
            {
                List<Order> orderlist = new List<Order>();

                Console.WriteLine("Enter the file name:");
                string fileName = Console.ReadLine();

                orderlist = ReadFile(fileName); //read csv file

                WriteFirstFile(orderlist, fileName); //create first csv

                WriteSecondFile(orderlist, fileName); //create second csv

               
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error:" + ex.Message);

            }
            finally {
                Console.WriteLine("\n Press any key to close");
                Console.ReadKey();
            }
        }

        static List<Order> ReadFile(string fname)
        {
            List<Order> ordlist = new List<Order>();
            bool fileExist = File.Exists("C:\\ExerciseCSV\\" + fname);
            if (fileExist)
            {
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HasHeaderRecord = false,
                };
                using (var reader = new StreamReader("C:\\ExerciseCSV\\" + fname))
                using (var csv = new CsvReader(reader, config))
                {
                    
                    while (csv.Read())
                    {
                        var record = csv.GetRecord<Order>();
                         ordlist.Add(new Order()
                        {
                            OrdId = record.OrdId,
                            OrdArea = record.OrdArea,
                            PrdName = record.PrdName,
                            PrdQuantity = record.PrdQuantity,
                            PrdBrand = record.PrdBrand
                        });

                    }
                }
                
            }
            else
            {
                Console.WriteLine("File does not exist.");
                Console.WriteLine("Press any key to close"); Console.ReadKey();

                Environment.Exit(0);
            }
            return ordlist;
        }

        static void WriteFirstFile(List<Order> ordList, string fName)
        {
            List<FirstResult> firstResultList = new List<FirstResult>();
            List<Order> firstResult = ordList
                            .GroupBy(l => l.PrdName)
                            .Select(cl => new Order
                            {
                                PrdName = cl.First().PrdName,
                                PrdQuantity = cl.Sum(c => c.PrdQuantity),
                            }).ToList();

            foreach (Order ord in firstResult)
            {
                firstResultList.Add(new FirstResult()
                {
                    rPrdName = ord.PrdName,
                    AvgQuantity = (float)ord.PrdQuantity / ordList.Count,
                });
            }

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = false
            };
            using (var writer = new StreamWriter("C:\\ExerciseCSV\\0_" + fName))
            using (var csv = new CsvWriter(writer, config))
            {
                csv.WriteRecords(firstResultList);
            }
            Console.WriteLine("\n C:\\ExerciseCSV\\0_" + fName + " is created successfully..!");
        }

        static void WriteSecondFile(List<Order> ordList, string fName)
        {
            List<SecondResult> secondResultList = new List<SecondResult>();
            var mostPopularBrand = "";
            var groupedResult = from s in ordList
                                group s by s.PrdName;
            foreach (var nameGroup in groupedResult)
            {
                foreach (Order s in nameGroup) // Each group has inner collection
                {
                    mostPopularBrand = "";
                    mostPopularBrand = nameGroup.GroupBy(x => x.PrdBrand)
                                                    .OrderByDescending(x => x.Count())
                                                    .First().Key;

                    secondResultList.Add(new SecondResult()
                    {
                        PrdName = nameGroup.Key,
                        MostPopularBrand = mostPopularBrand
                    });
                    break;
                }
            }
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = false,
            };
            using (var writer = new StreamWriter("C:\\ExerciseCSV\\1_" + fName))
            using (var csv = new CsvWriter(writer, config))
            {
                csv.WriteRecords(secondResultList);
            }

            Console.WriteLine("\n C:\\ExerciseCSV\\1_" + fName + " is created successfully..!");
        }
    }
    }

