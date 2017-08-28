using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DISA.Models
{
    public class Customer
    {
        string fullName;
        int phoneNumber;
        public Customer(string fullName, int phoneNumber)
        {
            this.FullName = fullName;
            this.PhoneNumber = phoneNumber;
        }

        public int PhoneNumber { get => phoneNumber; set => phoneNumber = value; }
        public string FullName { get => fullName; set => fullName = value; }
    }
}
