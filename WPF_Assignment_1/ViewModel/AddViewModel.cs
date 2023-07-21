using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace WPF_Assignment_1.ViewModel
{
    public class AddViewModel : BaseViewModel
    {
        private string _inputName;
        private string _inputAddress;
        private string _inputGender;
        private DateTime _inputDOB = DateTime.Now;
        public string InputName { get { return _inputName; } set { _inputName = value; OnPropertyChanged("InputName"); } }
        public string InputAddress { get { return _inputAddress; } set { _inputAddress = value; OnPropertyChanged("InputAddress"); } }
        public string InputGender { get { return _inputGender; } set { _inputGender = value; OnPropertyChanged("InputGender"); } }
        public DateTime InputDOB { get { return _inputDOB; } set { _inputDOB = value; OnPropertyChanged("InputDOB"); } }

        private Student _student;
        public Student Student
        {
            get
            {
                return _student;
            }
            set
            {
                if (_student != value)
                {
                    _student = value;
                    OnPropertyChanged(nameof(Student));
                    Messenger.Default.Send(new ViewModelMessage(_student), ViewModelMessafeToken.Token);
                }
            }
        }

        public ICommand SaveAdd { get; set; }

        public AddViewModel()
        {
            SaveAdd = new RelayCommand<MainViewModel>((p) => true, (p) =>
            {
                Student = new Student
                {
                    Id = new Guid(),
                    Name = InputName,
                    Address = InputAddress,
                    DOB = InputDOB,
                    Gender = InputGender,
                };
                MessageBox.Show("Thêm Thành Công");
                InputAddress = InputName = InputGender = "";
                InputDOB = DateTime.Now;
            });
        }

        
    }
}
