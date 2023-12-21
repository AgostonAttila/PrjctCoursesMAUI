using PrjctCoursesMAUI.Models;
using SQLite;
using System.Collections.Generic;
using static SQLite.SQLite3;

namespace PrjctCoursesMAUI.Services
{
    public interface ICourseService
    {
        Task<List<CourseModel>> GetAllCourse();
        Task<CourseModel> GetCourseByID(string guid);
        Task<int> AddCourse(CourseModel courseModel);
        Task<int> AddCourses(List<CourseModel> courses);
        Task<int> UpdateCourse(CourseModel courseModel);
        Task<int> DeleteCourse(string guid);
        Task<int> DeleteAllCourse();
        Task<List<CourseModel>> UploadCourseFromCSV(string path);

    }

    public class CourseService : ICourseService
    {

        private SQLiteAsyncConnection _dbConnection;
        string _dbPath;

        public CourseService(string dbPath = "")
        {
            _dbPath = dbPath;
            SetUpDb();
        }

        private async void SetUpDb()
        {
            try
            {
                if (_dbConnection == null)
                {
                    if (string.IsNullOrEmpty(_dbPath))
                        _dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "course_test_v1.db3");
                    _dbConnection = new SQLiteAsyncConnection(_dbPath);
                    await _dbConnection.CreateTableAsync<CourseModel>();
                }
            }
            catch (Exception e)
            {

            }
        }

        public async Task<int> AddCourse(CourseModel courseModel)
        {
            courseModel.Guid = Guid.NewGuid().ToString();
            return await _dbConnection.InsertAsync(courseModel);
        }

        public async Task<int> AddCourses(List<CourseModel> courseModelList)
        {
            try
            {
                foreach (CourseModel course in courseModelList)                
                    course.Guid = Guid.NewGuid().ToString();                  
                
                await _dbConnection.InsertAllAsync(courseModelList);
            }
            catch (Exception e)
            {

                throw;
            }

            return courseModelList.Count();
        }

        public async Task<int> DeleteCourse(string guid)
        {
            try
            {
                CourseModel pwd = await _dbConnection.Table<CourseModel>().Where(p => p.Guid == guid).FirstOrDefaultAsync();
                return await _dbConnection.DeleteAsync(pwd);
            }
            catch (Exception e)
            {

                throw;
            }

        }

        public async Task<int> DeleteAllCourse()
        {
            try
            {
                return await _dbConnection.DeleteAllAsync<CourseModel>();
            }
            catch (Exception e)
            {

                throw;
            }

        }
        public async Task<int> UpdateCourse(CourseModel courseModel)
        {
            return await _dbConnection.UpdateAsync(courseModel);
        }
        public async Task<List<CourseModel>> GetAllCourse()
        {
            List<CourseModel> result = new List<CourseModel>();
            try
            {
                result = await _dbConnection.Table<CourseModel>().ToListAsync();
            }
            catch (Exception e)
            {

                throw e;
            }
            return result;

        }

        public async Task<CourseModel> GetCourseByID(string guid)
        {
            return await _dbConnection.Table<CourseModel>().Where(p => p.Guid == guid).FirstOrDefaultAsync();
        }

        public async Task<List<CourseModel>> UploadCourseFromCSV(string path)
        {
            if (!File.Exists(path)) return new List<CourseModel>();

            String[] lines = File.ReadAllLines(path);

            if (lines?.Count() < 2) return new List<CourseModel>();

            List<CourseModel> courses = new List<CourseModel>();
            List<CourseModel> dBCourses = await GetAllCourse();

            foreach (string line in lines)
            {
                if (line == "Name;Company;Category;Technology;Author;Status;Url;Github") continue;
                String[] courseArray = line.Split(';');
                if (courseArray?.Count() != 8) continue;
                if (dBCourses.Any(p => p.Name == courseArray[0])) continue;

                courses.Add(new CourseModel
                {
                    Name = courseArray[0].Trim(),
                    Company = courseArray[1].Trim(),
                    Category = courseArray[2].Trim(),
                    Technology = courseArray[3].Trim(),
                    Author = courseArray[4].Trim(),
                    Status = courseArray[5].Trim(),
                    Url = courseArray[6].Trim(),
                    Github = courseArray[7].Trim(),
                    UserId = "891fc555-a46c-40f9-b70b-45b5823afb2b",
                    Guid = Guid.NewGuid().ToString()

                });
            }
            await AddCourses(courses);


            return courses;
        }


    }
}
