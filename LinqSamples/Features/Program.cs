using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Features
{
    class Program
    {
       

        

        static void Main(string[] args)
        {
            Func<int, int> square = x => x * x;
            Func<int, int, int> add = (x, y) =>
            {
               int temp = x + y;
               return temp;
            };
            Action<int> write = x => Console.WriteLine(x);

            write(square(add(3, 5)));

           var developers = new Employee[]
            {
                new Employee {Id = 1, Name = "Scott"},
                new Employee {Id = 2, Name = "Chris" } 
            };

            var sales = new List<Employee>()
            {
                new Employee
                {
                    Id = 3, Name = "Alex"
                }
            };
            var query = developers.Where(e => e.Name.Length == 5)
                                  .OrderBy(e => e.Name)
                                  .Select(e => e);

            var query2 = from developer in developers
                where developer.Name.Length == 5
                orderby developer.Name
                select developer.Name;


            foreach (var employee in query2)
            {
                Console.WriteLine(employee);  
            }
        }

        private static bool NameStartsWithS(Employee employee)
        {
            return employee.Name.StartsWith("S");
        }
    }
}
