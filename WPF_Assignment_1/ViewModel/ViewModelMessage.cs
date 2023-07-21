using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Assignment_1.ViewModel
{
    class ViewModelMessage
    {
        public Student student { get; set; }   
        public ViewModelMessage(Student student)
        {
            this.student = student;
        }
    }
}
