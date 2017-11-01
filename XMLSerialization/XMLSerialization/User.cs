using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XMLSerialization
{
    public class User
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public User(string n, int a)
        {
            Name = n;
            Age = a;
        }
        public void Display()
        {
            Console.WriteLine("Имя: {0}  Возраст: {1}", this.Name, this.Age);
        }
        public int Payment(int hours, int perhour)
        {
            return hours * perhour;
        }
        public User() { }
    }
}
