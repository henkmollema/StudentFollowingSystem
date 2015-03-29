using System.Collections.Generic;
using System.Linq;
using Dapper;
using Dommel;
using StudentFollowingSystem.Models;

namespace StudentFollowingSystem.Data.Repositories
{
    public class CounselingRepository : RepositoryBase<Counseling>
    {
        public Counseling GetByAppointmentId(int id)
        {
            using (var con = ConnectionFactory.GetOpenConnection())
            {
                string sql = @"
select * from Counselings c
inner join Appointments a on a.Id = c.AppointmentId
where a.Id = @Id";

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