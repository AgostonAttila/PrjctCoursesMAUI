using PrjctCoursesMAUI.Models;
using PrjctCoursesMAUI.Services;


namespace PrjctCoursesMAUI.States
{
    public class AppState
    {           

        public CourseModel? SelectedCourse { get; private set; }
        public List<CourseModel> CourseList { get; private set; } 
        public String? Username { get; private set; }

        public event Action OnChange;

        public void SetCourse(CourseModel courseModel)
        {
            SelectedCourse = courseModel;
            NotifyStateChanged();
        }

        public void SetCourseList(List<CourseModel> courses)
        {
            CourseList = courses;
            NotifyStateChanged();
        }

        public void AddCourse(CourseModel course)
        {
            CourseList.Add(course);
            NotifyStateChanged();
        }
        public void UpdateCourse(CourseModel course)
        {
            if (CourseList?.Count == 0) return;

            CourseModel updatedCourse = CourseList.FirstOrDefault(p =>p.Guid == course.Guid);

            if (updatedCourse != null)
             updatedCourse = course;
            
            NotifyStateChanged();
        }

        public void DeleteCourse(CourseModel course)
        {
            CourseList.Remove(course);
            NotifyStateChanged();
        }

        public void SetUsername(string name)
        {
            Username = name;
            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}

