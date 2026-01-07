using System;

namespace HRMS.Helper
{
    public static class UserSession
    {
        public static int CurrentUserId { get; set; }

        public static string CurrentUserName { get; set; }

        public static string CurrentUserRole { get; set; }
    }
}
