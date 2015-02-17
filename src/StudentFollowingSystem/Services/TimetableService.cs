using System;
using System.Collections.Generic;
using System.Net.Http;

namespace StudentFollowingSystem.Services
{
    public class TimetableService
    {
        public string GetTimetableHtml()
        {
            using (var client = new HttpClient())
            {
                var values = new Dictionary<string, string>
                                 {
                                     { "department", "TEE" },
                                     { "week", ConvertToTimestamp(DateTime.Now).ToString("0") },
                                     { "schedule", "Informatica jaar 1- I1B" }
                                 };

                var content = new FormUrlEncodedContent(values);

                var response = client.PostAsync("http://lekkerdoelloos.nl/getRoster.php", content);

                var responseString = response.Result.Content.ReadAsStringAsync();
                return responseString.Result;
            }
        }

        private static double ConvertToTimestamp(DateTime value)
        {
            return (value - new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime()).TotalSeconds;
        }
    }
}
