using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DISA.Models
{
    public class Customer
    {
        string firstName;
        string lastName;
        int phoneNumber;
        public Customer(string firstName, string lastname, int phoneNumber)
        {
            this.FirstName = firstName;
            this.LastName = lastname;
            this.PhoneNumber = phoneNumber;
        }
        public string FirstName { get => firstName; set => firstName = value; }
        public string LastName { get => lastName; set => lastName = value; }
        public int PhoneNumber { get => phoneNumber; set => phoneNumber = value; }
    }
}
