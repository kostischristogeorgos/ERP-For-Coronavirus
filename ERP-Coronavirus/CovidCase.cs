using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thema3
{
    class CovidCase
    {
        string Name, Email, Phone, Gender,Age, Subsymptoms, Address, Date;
        // Getters and Setters for each atrribute of the CovidCase.
        public CovidCase(string name, string email, string phone, string gender,string age, string subsymptoms, string address, string date)
        {
            this.Name = name;
            this.Email = email;
            this.Phone = phone;
            this.Gender = gender;
            this.Age = age;
            this.Subsymptoms = subsymptoms;
            this.Address = address;
            this.Date = date;
        }
        public string GetName()
        {
            return this.Name;
        }
        public string GetEmail()
        {
            return this.Email;
        }
        public string GetPhone()
        {
            return this.Phone;
        }
        public string GetGender()
        {
            return this.Gender;
        }
        public string GetAge()
        {
            return this.Age;
        }
        public string GetSubsymptoms()
        {
            return this.Subsymptoms;
        }
        public string GetAddress()
        {
            return this.Address;
        }
        public string GetDate()
        {
            return this.Date;
        }
    }
}
