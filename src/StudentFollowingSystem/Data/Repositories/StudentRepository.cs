using System.Collections.Generic;
using System.Linq;
using Dommel;
using StudentFollowingSystem.Models;

namespace StudentFollowingSystem.Data.Repositories
{
    public class StudentRepository
    {
        public List<Student> GetAll()
        {
            using (var con = ConnectionFactory.GetOpenConnection())
            {
                return con.GetAll<Student>().ToList();
            }
        }

        public Student GetById(int id)
        {
            using (var con = ConnectionFactory.GetOpenConnection())
            {
                return con.Get<Student>(id);
            }
        }

        public Student GetByEmail(string email)
        {
            using (var con = ConnectionFactory.GetOpenConnection())
            {
                return con.Select<Student>(s => s.Email == email).FirstOrDefault();
            }
        }

        public int Add(Student student)
        {
            using (var con = ConnectionFactory.GetOpenConnection())
            {
                return con.Insert(student);
            }
        }

        public bool Update(Student student)
        {
            using (var con = ConnectionFactory.GetOpenConnection())
            {
                return con.Update(student);
            }
        }

        public void Delete(Student student)
        {
            using (var con = ConnectionFactory.GetOpenConnection())
            {
                con.Delete(student);
            }
        }
    }
}
