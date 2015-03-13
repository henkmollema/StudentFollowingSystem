using StudentFollowingSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dapper;

namespace StudentFollowingSystem.Data.Repositories
{
    public class AppointmentRepository : RepositoryBase<Appointment>
    {
        public List<Appointment> GetAppointmentsByCounseler(int counselerId, DateTime toDate, DateTime nowDate)
        {
            using (var con = ConnectionFactory.GetOpenConnection())
            {
                string sql = @"
select * from Appointments a
inner join Students s on a.StudentId = s.Id
where a.Accepted = 1 and a.CounselerId = @counselerId and a.DateTime < @toDate and a.DateTime >= @nowDate
order by a.DateTime";

                return con.Query<Appointment, Student, Appointment>(sql, (a, s) =>
                {
                    a.Student = s;
                    return a;
                },
                new { counselerId = counselerId, toDate = toDate, nowDate = nowDate })
                .ToList();
            }
        }
    }
}