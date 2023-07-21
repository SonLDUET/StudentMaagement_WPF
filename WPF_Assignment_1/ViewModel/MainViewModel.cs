using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using System.Windows.Markup;
using System.Windows.Navigation;
using System.Windows.Controls;
using System.Windows.Data;

namespace WPF_Assignment_1.ViewModel
{
    public class MainViewModel : BaseViewModel
    {

        private string JsonFile = "C:\\Users\\Admin\\source\\WPF\\WPF_Assignment_1\\WPF_Assignment_1\\data.json";

        private string _name;
        private string _addr;
        private string _gender;
        private DateTime _start = new DateTime(1970,1,1);
        private DateTime _end = DateTime.Now.Date;
        private ObservableCollection<Student> _listStudent;
        private List<Student> _studentData;
        
        public AddStudentForm StudentForm { get; set; }
      
        public string SearchName{  get { return _name; } set { _name = value; OnPropertyChanged("SearchName"); } }
        public string SearchAddress{ get { return _addr; } set { _addr = value; OnPropertyChanged("SearchAddress"); } }
        public string SearchGender{ get { return _gender; } set { _gender = value; OnPropertyChanged("SearchGender"); } }
        public DateTime StartDate { get { return _start; } set { _start = value; OnPropertyChanged("StartDate"); } }
        public DateTime EndDate { get { return _end; } set { _end = value; OnPropertyChanged("EndDate"); } }

        public ICommand SearchCmd { get; set; }
        public ICommand AddCmd { get; set; }
        public ICommand SaveToFileCmd { get; set; }
        public ICommand ShowAllData { get; set; }
        public ICommand DeleteCmd { get; set; }
        public ICommand ModifyCmd { get; set; }

        public List<Student> StudentData { get { return _studentData; } set { _studentData = value; OnPropertyChanged("StudentData"); } }

        public ObservableCollection<Student> StudentsToDisplay { get { return _listStudent; } set { _listStudent = value; OnPropertyChanged("StudentsToDisplay"); } }

        public MainViewModel()
        {
            StudentData = LoadFromFile();
            Messenger.Default.Register<ViewModelMessage>(this, ViewModelMessafeToken.Token, ReceiveData);

            StudentForm = null;
            StudentsToDisplay = new ObservableCollection<Student>(StudentData);
            SearchCmd = new RelayCommand<MainViewModel>((p) => true, (p) =>
            {
                StudentsToDisplay = Search(_name, SearchAddress, SearchGender, StartDate, EndDate);
            });

            AddCmd = new RelayCommand<MainViewModel>((p) => true, (p) =>
            {
                StudentForm = new AddStudentForm();
                StudentForm.Visibility = Visibility.Visible;
            });
            
            ShowAllData = new RelayCommand<MainViewModel>(p => true, p =>
            {
                StudentsToDisplay = new ObservableCollection<Student>(StudentData);
            });
            DeleteCmd = new RelayCommand<Student>(p => true, p =>
            {
                MessageBoxResult result = MessageBox.Show("Em có chắc không?", "Xác nhận xóas", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    StudentsToDisplay.Remove(p);
                    StudentData.Remove(p);
                }
            });
            ModifyCmd = new RelayCommand<Student>(p => true, p =>
            {
                Student modifiedStudent = StudentData.FirstOrDefault(x => x.Id ==  p.Id);
                Student newStudent = StudentsToDisplay.FirstOrDefault(x => x.Id == p.Id);
                modifiedStudent.Name = p.Name;
                modifiedStudent.Address = p.Address;
                modifiedStudent.DOB = p.DOB;
                modifiedStudent.Gender = p.Gender;
                MessageBox.Show("Sửa dữ liệu thành công");
            });
            SaveToFileCmd = new RelayCommand<MainViewModel>(p => true, p =>
            {
                SaveToFile();
                MessageBox.Show("Lưu thành công dữ liệu vào file !");
            });
        }

        private void ReceiveData (ViewModelMessage message)
        {
            StudentData.Add(message.student);
            MessageBox.Show("Thêm Thành Công");
        }

       public ObservableCollection<Student> Search(string name, string address, string gender, DateTime start, DateTime end)
       {
            if (StudentData.Count == 0)
            {
                return null;
            }
            List<Student> ListStudent_0 = StudentData.ToList();
            List<Student> ListStudent_1 = null;
            if (!string.IsNullOrEmpty(name))
            {
                ListStudent_1 = (from std in StudentData where std.Name.Contains(name) select std).ToList();
            }
            else ListStudent_1 = ListStudent_0;
            List<Student> ListStudent_2 = null;
            if (!string.IsNullOrEmpty(address))
            {
                ListStudent_2 = (from std in ListStudent_1 where std.Address.Contains(address) select std).ToList();
            }
            else ListStudent_2 = ListStudent_1;
            List<Student> ListStudent_3 = null;
            if (!string.IsNullOrEmpty(gender))
            {
                ListStudent_3 = (from std in ListStudent_2 where std.Gender.Contains(gender) select std).ToList();
            }
            else ListStudent_3 = ListStudent_2;
            List<Student> ListStudent_4 = (from std in ListStudent_3 where std.DOB >= start && std.DOB <= end select std).ToList();
            ObservableCollection<Student> res = new ObservableCollection<Student>(ListStudent_4);
            return res;
       }

        public void AddStudent([Required]string Name,[Required] string Addr, [Required]string Gender, [Required] DateTime DOB)
        {
            Student newStudent = new Student {Id= new Guid(), Name=Name, Address = _addr, Gender=_gender, DOB=DOB };
            StudentsToDisplay.Add(newStudent);
            StudentData.Add(newStudent);
        }


        public void SaveToFile()
        {
            File.WriteAllText(JsonFile, "");
            JArray jArrayString = new JArray();
            StudentData.ForEach(x => jArrayString.Add(JObject.FromObject(x)));
            File.WriteAllText(JsonFile, jArrayString.ToString());
        }

        public List<Student> LoadFromFile()
        {
            var json = File.ReadAllText(JsonFile);
            if (string.IsNullOrEmpty(json))
            {
                return new List<Student>();
            }
            JArray jArray = JArray.Parse(json);
            List<Student> students = jArray.ToObject<List<Student>>();
            return students;
        }

    }
}
