using SQLite;

namespace PrjctCoursesMAUI.Models
{

    public static class CourseStatus
    {
        public static string Obsolete = "Obsolete";
        public static string Finished = "Finished";
        public static string Starting = "Started";
        public static string NotStartingYet = "Not Started";
    }

    public class CourseModel
    {
        [PrimaryKey]
        public string Guid { get; set; }
        public string UserId { get; set; }

        public string Company { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }


        public string Category { get; set; }
        public string Technology { get; set; }
        public string Status { get; set; }


        public string Url { get; set; }
        public string Github { get; set; }        

    }
}
