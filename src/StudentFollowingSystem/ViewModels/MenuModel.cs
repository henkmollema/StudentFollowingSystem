namespace StudentFollowingSystem.ViewModels
{
    public class MenuModel
    {
        public bool IsStudent { get; set; }

        public bool IsCounseler { get; set; }

        public string Username { get; set; }

        public bool IsAuthenticated
        {
            get
            {
                return IsStudent || IsCounseler;
            }
        }
    }
}