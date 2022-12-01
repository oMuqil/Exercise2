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
            
            var firstResult = ordList
                            .GroupBy(l => l.PrdName)
                            .Select(cl => new FirstResult
                            {
                                rPrdName = cl.First().PrdName,
                                AvgQuantity = (float)cl.Sum(c => c.PrdQuantity) / ordList.Count,
                            }).ToList();

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = false
            };
            using (var writer = new StreamWriter("C:\\ExerciseCSV\\0_" + fName))
            using (var csv = new CsvWriter(writer, config))
            {
                csv.WriteRecords(firstResult);
            }
            Console.WriteLine("\n C:\\ExerciseCSV\\0_" + fName + " is created successfully..!");
        }

        static void WriteSecondFile(List<Order> ordList, string fName)
        {
          
            var secondResultList = ordList
                            .GroupBy(l => l.PrdName)
                            .Select(cl => new SecondResult
                            {
                                PrdName = cl.First().PrdName,
                                MostPopularBrand = cl.GroupBy(x => x.PrdBrand)
                                                    .OrderByDescending(x => x.Count())
                                                    .First().Key,
                            }).ToList();
           

           
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

