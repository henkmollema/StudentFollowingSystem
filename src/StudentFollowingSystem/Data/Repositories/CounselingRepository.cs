using System.Linq;
using Dapper;
using StudentFollowingSystem.Models;

namespace StudentFollowingSystem.Data.Repositories
{
    public class CounselingRepository : RepositoryBase<Counseling>
    {
        /// <summary>
        /// Get a counseling by the appointment id joined with the appointment.
        /// </summary>
        /// <param name="id">The id of an appointment.</param>
        public Counseling GetByAppointment(int id)
        {
            using (var con = ConnectionFactory.GetOpenConnection())
            {
                string sql = @"
select * from Counselings c
join Appointments a on a.Id = c.AppointmentId
where c.AppointmentId = @Id";

                return con.Query<Counseling, Appointment, Counseling>(sql,
                    (c, a) =>
                    {
                        c.Appointment = a;
                        return c;
                    },
                    new { id }).FirstOrDefault();
            }
        }
    }
}
