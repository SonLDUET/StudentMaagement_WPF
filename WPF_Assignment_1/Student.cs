using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Assignment_1
{
    public class Student
    {
        private DateTime _dob;
        public Guid Id { get;set; }
        public string Name { get; set; }
        public DateTime DOB { 
            get 
            { 
                return _dob; 
            }
            set 
            { 
                _dob = value; 
            } 
        }
        public string Gender { get; set; }
        public string Address { get; set; }

    }
}
