using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using MVC_wepapp_2.Models;

namespace MVC_wepapp_2.Controllers
{
    public class StudentsController : Controller
    {
        IConfiguration configuration;
        public SqlConnection connection;
        public StudentsController(IConfiguration configuration) 
        {
            this.configuration = configuration;
            this.connection = new SqlConnection(configuration.GetConnectionString("practiceDB"));

        }
        
        public List<Students> GetStudents()
        {
            List<Students> students = new List<Students>();
            connection.Open();
            SqlCommand command = new SqlCommand("fetchStudents",connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            SqlDataReader reader = command.ExecuteReader(); 
            while(reader.Read())
            {
                Students student = new Students();
                student.Id = (int)reader["Id"];
                student.Name = (string)reader["Name"];
                student.DOB = (DateTime)reader["date_of_birth"];
                student.Department = (string)reader["department"];
                students.Add(student);
            }

            reader.Close();
            connection.Close();

            return students;
        }
       
        // GET: StudentsController
        public ActionResult Index()
        {

            return View(GetStudents());
        }

        // GET: StudentsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
        void insertStudent(Students student)
        {
            connection.Open();
            SqlCommand command = new SqlCommand("addStudent", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@studentid", student.Id);
            command.Parameters.AddWithValue("@studentname", student.Name);
            command.Parameters.AddWithValue("@studentdob", student.DOB);
            command.Parameters.AddWithValue("@studentdept", student.Department);

            command.ExecuteNonQuery();
            connection.Close();
        }
        // GET: StudentsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: StudentsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Students student)
        {
            try
            {
                insertStudent(student);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        Students GetStudent(int id)
        {
            connection.Open();
            SqlCommand command = new SqlCommand("getStudent", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@studentid", id);
            SqlDataReader reader = command.ExecuteReader();
            Students student = new Students();
            while (reader.Read())
            {
                
                student.Id = (int)reader["Id"];
                student.Name = (string)reader["Name"];
                student.DOB = (DateTime)reader["date_of_birth"];
                student.Department = (string)reader["department"];
               
            }

            reader.Close();
            connection.Close();

            return student;
        }
        
       
        // GET: StudentsController/Edit/5
        public ActionResult Edit(int id)
        {

            return View(GetStudent(id));
        }
        void updateStudents(Students student)
        {
            connection.Open();
            SqlCommand command = new SqlCommand("updateStudent", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@studentid", student.Id);
            command.Parameters.AddWithValue("@studentname", student.Name);
            command.Parameters.AddWithValue("@studentdob", student.DOB);
            command.Parameters.AddWithValue("@studentdept", student.Department);

            command.ExecuteNonQuery();
            connection.Close();

        }
        // POST: StudentsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Students student)
        {
            try
            {
                updateStudents(student);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: StudentsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: StudentsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
