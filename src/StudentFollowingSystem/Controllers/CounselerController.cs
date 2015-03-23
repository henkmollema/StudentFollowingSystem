﻿using System;
using System.Linq;
using System.Web.Mvc;
using StudentFollowingSystem.Data.Repositories;
using StudentFollowingSystem.Filters;
using StudentFollowingSystem.Models;
using StudentFollowingSystem.ViewModels;

namespace StudentFollowingSystem.Controllers
{
    [AuthorizeCounseler]
    public class CounselerController : ControllerBase
    {
        private readonly AppointmentRepository _appointmentRepository = new AppointmentRepository();

        public ActionResult Dashboard()
        {
            Counseler counseler = Counseler;
            var appointments = _appointmentRepository.GetAppointmentsByCounseler(counseler.Id, GetFirstSaturday(), DateTime.Now);
            var students = StudentRepository.GetAll();
            foreach (var appointment in appointments)
            {
                appointment.Counseler = counseler;
            }

            var model = new CounselerDashboardModel
                            {
                                Appointments = appointments,
                                Students = students.Where(s => s.Status == Status.Orange ||
                                                               s.Status == Status.Red ||
                                                               DateTime.Now >= s.NextAppointment)
                                                   .OrderByDescending(s => s.Status)
                                                   .ThenBy(s => s.NextAppointment)
                                                   .ToList()
                            };

            return View(model);
        }

        private static DateTime GetFirstSaturday()
        {
            DateTime date = DateTime.Now;
            int days = 0;

            switch (date.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    days = 5;
                    break;
                case DayOfWeek.Tuesday:
                    days = 4;
                    break;
                case DayOfWeek.Wednesday:
                    days = 3;
                    break;
                case DayOfWeek.Thursday:
                    days = 2;
                    break;
                case DayOfWeek.Friday:
                    days = 1;
                    break;
                case DayOfWeek.Saturday:
                    days = 7;
                    break;
                case DayOfWeek.Sunday:
                    days = 6;
                    break;
            }

            return date.AddDays(days);
        }
    }
}
