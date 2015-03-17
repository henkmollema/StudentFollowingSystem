using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using StudentFollowingSystem.Models;

namespace StudentFollowingSystem.Data.Repositories
{
    public class AppointmentRepository : RepositoryBase<Appointment>
    {
        /// <summary>
        /// Gets all the appointments for te specified <paramref name="counselerId"/> 
        /// within the specified date range.
        /// </summary>
        /// <param name="counselerId">The counseler id.</param>
        /// <param name="toDate">The starting date.</param>
        /// <param name="nowDate">The end date.</param>
        /// <returns>A collection of appointments.</returns>
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
