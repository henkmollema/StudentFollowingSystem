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
                return con.Get<Counseling, Appointment, Counseling>(id,
                    (c, a) =>
                    {
                        c.Appointment = a;
                        return c;
                    });
            }
        }
    }
}
