using Dommel;
using StudentFollowingSystem.Models;

namespace StudentFollowingSystem.Data.Repositories
{
    public class CounselingRepository : RepositoryBase<Counseling>
    {
        /// <summary>
        /// Get a counseling by the appointment id joined with the appointment.
        /// </summary>
        /// <param name="id">The id of an appointment.</param>
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
