using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Xml.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Cars
{
    class Program
    {
        static void Main(string[] args)
        {
            //gives EF permission to drop/create table if model doesnt match schema
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<CarDb>());
            InsertData();
            QueryData();
        }

        private static void QueryData()
        {
           var db = new  CarDb();
            db.Database.Log = Console.WriteLine; // will show sql sent to server

            var query =
                db.Cars.GroupBy(c => c.Manufacturer)
                    .Select(g => new
                    {
                        Name = g.Key,
                        Cars = g.OrderByDescending(c => c.Combined).Take(2)
                    });
            Console.WriteLine(query.Count());
          

            foreach (var group in query)
            {
                Console.WriteLine(group.Name);
                foreach (var car in group.Cars)
                {
                    Console.WriteLine($"\t{car.Name} : {car.Combined}");
                }
            }
        }

        private static void InsertData()
        {
            var cars = ProcessCars("fuel.csv");
            var db = new CarDb();

            if (!db.Cars.Any()) //anything there?
            {
                foreach (var car in cars)
                {
                    db.Cars.Add(car); //note to insert car
                }
                db.SaveChanges(); //actually insert cars
            }
        }

        private static void QueryXml()
        {
            var document = XDocument.Load("fuel.xml");
            var query =
                from element in document.Descendants("Car")
                where element.Attribute("Manufacturer")?.Value == "BMW"
                select element.Attribute("Name").Value;

            foreach (var name in query)
            {
                Console.WriteLine(name);
            }
        }

        private static void CreateXml()
        {
            var records = ProcessCars("fuel.csv");
            var document = new XDocument();
            var cars = new XElement("Cars",
                from record in records
                select new XElement("Car",
                    new XAttribute("Name", record.Name),
                    new XAttribute("Combined", record.Combined),
                    new XAttribute("Manufacturer", record.Manufacturer))
            );

            document.Add(cars);
            document.Save("fuel.xml");
        }

        private static List<Manufacturer> ProcessManufacturers(string path)
        {
            var query =
                File.ReadAllLines(path)
                    .Where(l => l.Length > 1)
                    .Select(l =>
                    {
                        var columns = l.Split(',');
                        return new Manufacturer
                        {
                            Name = columns[0],
                            Headquarters = columns[1],
                            Year = int.Parse(columns[2])
                        };
                    });
            return query.ToList();
        }

        private static List<Car> ProcessCars(string path)
        {
            //return 
            //File.ReadAllLines(path)
            //    .Skip(1)    //Skip first line with column name
            //    .Where(line => line.Length > 1) //select lines that are not empty
            //    .Select(Car.ParseFromCsv)
            //    .ToList();
            var query =
                File.ReadAllLines(path)
                    .Skip(1)
                    .Where(l => l.Length > 1)
                    .ToCar();

            return query.ToList();

        }
    }


    public class CarStatistics
    {
        public CarStatistics()
        {
            Min = int.MaxValue;
            Max = int.MinValue;
        }

        public CarStatistics Accumulate(Car car)
        {
            Count += 1;
            Total += car.Combined;
            Max = Math.Max(this.Max, car.Combined);
            Min = Math.Min(this.Min, car.Combined);
            return this;
        }

        public int Max { get; set; }
        public int Min { get; set; }
        public double Avg { get; set; }
        public int Total { get; set; }
        public int Count { get; set; }

        public CarStatistics Compute()
        {
            Avg = Total / Count;
            return this;
        }
    }

    public static class CarExtensions
    {
        public static IEnumerable<Car> ToCar(this IEnumerable<string> source)
        {
            foreach (var line in source)
            {
                var columns = line.Split(',');

                yield return new Car
                {
                    Year = int.Parse(columns[0]),
                    Manufacturer = columns[1],
                    Name = columns[2],
                    Displacement = double.Parse(columns[3]),
                    Cylinders = int.Parse(columns[4]),
                    City = int.Parse(columns[5]),
                    Highway = int.Parse(columns[6]),
                    Combined = int.Parse(columns[7])
                };
            }
        }
    }
}
