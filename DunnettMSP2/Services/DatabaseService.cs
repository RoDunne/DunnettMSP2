using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using SQLite;
using Xamarin.Essentials;
using DunnettMSP2.Models;
using Xamarin.Forms;

namespace DunnettMSP2.Services
{
    public static class DatabaseService
    {
        private static SQLiteAsyncConnection _dbAsync;
        private static SQLiteConnection _db;

        private static async Task InitAsync()
        {
            if (_dbAsync != null)
            {
            }
            else
            {
                var databasePath = Path.Combine(FileSystem.AppDataDirectory, "StudentPlanner.db");
                try
                { 
                    _dbAsync = new SQLiteAsyncConnection(databasePath);

                    _ = await _dbAsync.CreateTableAsync<Assessment>();
                    _ = await _dbAsync.CreateTableAsync<Instructor>();
                    _ = await _dbAsync.CreateTableAsync<Term>();
                    _ = await _dbAsync.CreateTableAsync<Course>();
                    _ = await _dbAsync.CreateTableAsync<RomanticDate>();
                }
                catch (Exception ex)
                {
                    // Handle any exceptions here
                    Console.WriteLine($"Error creating database: {ex.Message}");
                }
            }
        }
        private static void Init()
        {
            if (_db != null)
            {
            }
            else
            {
                var databasePath = Path.Combine(FileSystem.AppDataDirectory, "StudentPlanner.db");

                try
                {

                    _db = new SQLiteConnection(databasePath);

                    _ = _db.CreateTable<Assessment>();
                    _ = _db.CreateTable<Instructor>();
                    _ = _db.CreateTable<Term>();
                    _ = _db.CreateTable<Course>();
                    _ = _db.CreateTable<RomanticDate>();
                }

                catch (Exception ex)
                {
                    // Handle any exceptions here
                    Console.WriteLine($"Error creating database: {ex.Message}");
                }
            }
        }

        #region AssessmentServices
        public static async Task AddAssessmentAsync(Assessment assessment)
        {
            await InitAsync();
            _ = await _dbAsync.InsertAsync(assessment);
        }
        public static void AddAssessment(Assessment assessment)
        {
            Init();
            _ = _db.Insert(assessment);
        }

        public static async Task RemoveCourseAssessmentsAsync(int courseId)
        {
            await InitAsync();
            _ = await _dbAsync.Table<Assessment>().Where(a => a.CourseId == courseId).DeleteAsync();
        }

        public static async Task<Assessment> GetAssessmentAsync(int assessmentId)
        {
            await InitAsync();
            var assessment = await _dbAsync.Table<Assessment>().Where(i => i.Id == assessmentId).FirstOrDefaultAsync();
            return assessment;
        }
        public static Assessment GetAssessment(int assessmentId)
        {
            Init();
            var assessment = _db.Table<Assessment>().Where(i => i.Id == assessmentId).FirstOrDefault();
            return assessment;
        }

        public static async Task<Assessment> GetPerformanceAssessmentAsync(int courseId)
        {
            await InitAsync();
            var assessment = await _dbAsync.Table<Assessment>().Where(a => a.CourseId == courseId && a.Type == "Performance").FirstOrDefaultAsync();
            return assessment;
        }
        public static  Assessment GetPerformanceAssessment(int courseId)
        {
            Init();
            var assessment = _db.Table<Assessment>().Where(a => a.CourseId == courseId && a.Type == "Performance").FirstOrDefault();
            return assessment;
        }

        public static async Task<Assessment> GetObjectiveAssessmentAsync(int courseId)
        {
            await InitAsync();
            var assessment = await _dbAsync.Table<Assessment>().Where(a => a.CourseId == courseId && a.Type == "Objective").FirstOrDefaultAsync();
            return assessment;
        }
        public static Assessment GetObjectiveAssessment(int courseId)
        {
            Init();
            var assessment = _db.Table<Assessment>().Where(a => a.CourseId == courseId && a.Type == "Objective").FirstOrDefault();
            return assessment;
        }

        public static async Task UpdateAssessmentAsync(Assessment assessmentToUpdate)
        {
            await InitAsync();
            await _dbAsync.UpdateAsync(assessmentToUpdate);
        }

        //for debugging
        public static async Task<IEnumerable<Assessment>> GetAllAssessmentsAsync()
        {
            await InitAsync();
            var assessments = await _dbAsync.Table<Assessment>().ToListAsync();
            return assessments;
        }
        public static IEnumerable<Assessment> GetAllAssessments()
        {
            Init();
            var assessments = _db.Table<Assessment>().ToList();
            return assessments;
        }
        #endregion

        #region InstructorServices
        public static async Task<int> AddInstructorAsync(Instructor instructor)
        {
            await InitAsync();
            await _dbAsync.InsertAsync(instructor);
            return instructor.Id;
        }
        public static int AddInstructor(Instructor instructor)
        {
            Init();
            _db.Insert(instructor);
            return instructor.Id;
        }

        public static async Task RemoveInstructorAsync(int instructorId)
        {
            await InitAsync();
            await _dbAsync.DeleteAsync<Instructor>(instructorId);
        }

        //finds an instructor with a matching name, email, and phone; returns the instructor ID if found, or -1 if not found
        public static async Task<int> FindInstructorAsync(string name, string email, string phone)
        {
            await InitAsync();
            int answer = -1;
            Instructor instructor = await _dbAsync.Table<Instructor>().Where(i => i.Name == name && i.Email == email && i.Phone == phone).FirstOrDefaultAsync();
            if (instructor != null)
            {
                answer = instructor.Id;
            }
            return answer;
        }

        public static async Task<Instructor> GetInstructorAsync(int instructorId)
        {
            await InitAsync();
            var instructor = await _dbAsync.Table<Instructor>().Where(i => i.Id == instructorId).FirstOrDefaultAsync();
            return instructor;
        }
        public static  Instructor GetInstructor(int instructorId)
        {
            Init();
            var instructor = _db.Table<Instructor>().Where(i => i.Id == instructorId).FirstOrDefault();
            return instructor;
        }

        public static async Task<List<Course>> GetCoursesTaughtByInstructor(int instructorId)
        {
            List<Course> courses = await _dbAsync.Table<Course>().Where(c => c.InstructorId == instructorId).ToListAsync();
            return courses;
        }
        public static async Task UpdateInstructorAsync(int id, string name, string email, string phone)
        {
            await InitAsync();
            var instructorQuery = await _dbAsync.Table<Instructor>().Where(i => i.Id == id).FirstOrDefaultAsync();

            if (instructorQuery != null)
            {
                instructorQuery.Name = name;
                instructorQuery.Email = email;
                instructorQuery.Phone = phone;

                await _dbAsync.UpdateAsync(instructorQuery);
            }
        }

        //for debugging
        public static async Task<IEnumerable<Instructor>> GetAllInstructorsAsync()
        {
            await InitAsync();
            var instructors = await _dbAsync.Table<Instructor>().ToListAsync();
            return instructors;
        }
        public static IEnumerable<Instructor> GetAllInstructors()
        {
            Init();
            var instructors = _db.Table<Instructor>().ToList();
            return instructors;
        }
        #endregion

        #region TermServices

        //returns the TermID of the new term
        public static async Task<int> AddTermAsync(Term term)
        {
            await InitAsync();
            await _dbAsync.InsertAsync(term);
            return term.Id;
        }
        public static int AddTerm(Term term)
        {
            Init();
            _db.Insert(term);
            return term.Id;
        }

        public static async Task RemoveTermAsync(int termId)
        {
            await InitAsync();
            await _dbAsync.DeleteAsync<Term>(termId);
        }

        public static async Task<Term> GetTermAsync(int termId)
        {
            await InitAsync();
            var term = await _dbAsync.Table<Term>().Where(t => t.Id == termId).FirstOrDefaultAsync();
            return term;
        }

        public static Term GetTerm(int termId)
        {
            Init();
            var term = _db.Table<Term>().Where(t => t.Id == termId).FirstOrDefault();
            return term;
        }
        public static async Task<IEnumerable<Term>> GetTermListAsync()
        {
            await InitAsync();
            List<Term> terms = await _dbAsync.Table<Term>().ToListAsync();
            return terms;
        }

        public static IEnumerable<Term> GetTermList()
        {
            Init();
            List<Term> terms = _db.Table<Term>().ToList();
            return terms;
        }

        public static async Task UpdateTermAsync(Term termToUpdate)
        {
            await InitAsync();
            await _dbAsync.UpdateAsync(termToUpdate);
        }

        #endregion

        #region CourseServices

        //returns the CourseID of the new course
        public static async Task<int> AddCourseAsync(Course course)
        {
            await InitAsync();
            await _dbAsync.InsertAsync(course);
            return course.Id;
        }
        public static int AddCourse(Course course)
        {
            Init();
            _db.Insert(course);
            return course.Id;
        }

        public static async Task RemoveCourseAsync(int courseId)
        {
            await InitAsync();
            await _dbAsync.DeleteAsync<Course>(courseId);
        }

        public static async Task<Course> GetCourseAsync(int courseId)
        {
            await InitAsync();
            var course = await _dbAsync.Table<Course>().Where(i => i.Id == courseId).FirstOrDefaultAsync();
            return course;
        }
        public static Course GetCourse(int courseId)
        {
            Init();
            Course course = _db.Table<Course>().Where(i => i.Id == courseId).FirstOrDefault();
            return course;
        }

        public static async Task UpdateCourseAsync(Course courseToUpdate)
        {
            await InitAsync();
            await _dbAsync.UpdateAsync(courseToUpdate);
        }

        public static async Task<IEnumerable<Course>> GetCoursesForTermAsync(int termId)
        {
            await InitAsync();
            var courses = await _dbAsync.Table<Course>().Where(c => c.TermId == termId).ToListAsync();
            return courses;
        }
        public static IEnumerable<Course> GetCoursesForTerm(int termId)
        {
            Init();
            var courses = _db.Table<Course>().Where(c => c.TermId == termId).ToList();
            return courses;
        }

        public static async Task<IEnumerable<Course>> GetAllCoursesAsync()
        {
            await InitAsync();
            var courses = await _dbAsync.Table<Course>().ToListAsync();
            return courses;
        }
        public static IEnumerable<Course> GetAllCourses()
        {
            Init();
            var courses = _db.Table<Course>().ToList();
            return courses;
        }

        public static async Task<IEnumerable<Course>> GetCoursesFromSearchAsync(string searchString)
        {
            await InitAsync();
            var courses = await _dbAsync.Table<Course>().Where(c => c.Title.ToUpper().Contains(searchString.ToUpper())).ToListAsync();
            return courses;
        }
        #endregion

        #region EventServices (Not used)
        public static int AddEvent(PlannedEvent plannedEvent)
        {
            Init();
            _ = _db.Insert(plannedEvent);
            return plannedEvent.Id;
        }
        public static async Task<int> AddEventAsync(PlannedEvent plannedEvent)
        {
            await InitAsync();
            _ = await _dbAsync.InsertAsync(plannedEvent);
            return plannedEvent.Id;
        }
        public static bool RemoveEvent(int eventId)
        {
            Init();
            bool removed = true;
            try
            {
                _ = _db.Delete<PlannedEvent>(eventId);
            }
            catch (Exception ex)
            {
                removed = false;
                Application.Current.MainPage.DisplayAlert("Error", ex.ToString(), "Ok");
            }

            return removed;
        }
        public static async Task<bool> RemoveEventAsync(int eventId)
        {
            await InitAsync();
            bool removed = true;
            try
            {
                _ = _dbAsync.DeleteAsync<PlannedEvent>(eventId);
            }
            catch (Exception ex)
            {
                removed = false;
                await Application.Current.MainPage.DisplayAlert("Error", ex.ToString(), "Ok");
            }

            return removed;
        }
        public static IEnumerable<PlannedEvent> GetEventList()
        {
            Init();
            List<PlannedEvent> events = _db.Table<PlannedEvent>().ToList();
            return events;
        }
        public static async Task<IEnumerable<PlannedEvent>> GetEventListAsync()
        {
            await InitAsync();
            List<PlannedEvent> events = await _dbAsync.Table<PlannedEvent>().ToListAsync();
            return events;
        }
        public static PlannedEvent GetEvent(int eventId)
        {
            Init();
            PlannedEvent plannedEvent = _db.Table<PlannedEvent>().Where(i => i.Id == eventId).FirstOrDefault();
            return plannedEvent;
        }
        public static async Task<PlannedEvent> GetEventAsync(int eventId)
        {
            await InitAsync();
            PlannedEvent plannedEvent = await _dbAsync.Table<PlannedEvent>().Where(i => i.Id == eventId).FirstOrDefaultAsync();
            return plannedEvent;
        }
        #endregion

        #region RomanticDateServices
        public static int AddRomanticDate(RomanticDate romanticDate)
        {
            Init();
            _ = _db.Insert(romanticDate);
            return romanticDate.Id;
        }
        public static async Task<int> AddRomanticDateAsync(RomanticDate romanticDate)
        {
            await InitAsync();
            _ = await _dbAsync.InsertAsync(romanticDate);
            return romanticDate.Id;
        }
        public static IEnumerable<RomanticDate> GetAllRomanticDates()
        {
            Init();
            List<RomanticDate> dates = _db.Table<RomanticDate>().ToList();
            return dates;
        }
        public static async Task<IEnumerable<RomanticDate>> GetAllRomanticDatesAsync()
        {
            await InitAsync();
            List<RomanticDate> dates = await _dbAsync.Table<RomanticDate>().ToListAsync();
            return dates;
        }
        public static async Task RemoveRomanticDateAsync(int dateId)
        {
            await InitAsync();
            try
            {
                _ = await _dbAsync.DeleteAsync<RomanticDate>(dateId);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.ToString(), "Ok");
            }
        }
        #endregion

        #region OtherServices
        public static async Task LoadSampleDataAsync()
        {
            await InitAsync();

            Term theTerm = new Term
            {
                Title = "Fall 2036",
                StartDate = new DateTime(2036, 9, 1),
                EndDate = new DateTime(2036, 11, 30)
            };
            int termId = await AddTermAsync(theTerm);

            Instructor theInstructor = new Instructor
            {
                Name = "Robert Dunnett",
                Email = "rdunne3@wgu.edu",
                Phone = "(209)642-0644"
            };
            int instructorId = await AddInstructorAsync(theInstructor);

            Course theCourse = new Course
            {
                Title = "Math 202",
                InstructorId = instructorId,
                TermId = termId,
                StartDate = new DateTime(2036, 9, 1),
                EndDate = new DateTime(2036, 11, 30),
                Status = "Enrolled",
                OptionalNotes = "This is some sample optional notes. For instance, here you can mention any course materials you wish to remember.",
                Notifications = true
            };
            int courseId = await AddCourseAsync(theCourse);

            Assessment performanceAssessment = new Assessment
            {
                CourseId = courseId,
                Title = "C# Application",
                DueDate = new DateTime(2036, 11, 30),
                Type = "Performance",
                Notifications = true
            };
            await AddAssessmentAsync(performanceAssessment);

            Assessment objectiveAssessment = new Assessment
            {
                CourseId = courseId,
                Title = "Online Exam",
                DueDate = new DateTime(2036, 11, 30),
                Type = "Objective",
                Notifications = false
            };
            await AddAssessmentAsync(objectiveAssessment);

            RomanticDate theDate = new RomanticDate
            {
                Name = "Kathryn",
                StartDateAndTime = new DateTime(2036, 9, 20, 17, 0, 0),
                Location = "Bob Dog's Pizza",
                PhoneNumber = "209-343-3955",
                Title = "Date With Kathryn"
            };
            _ = await AddRomanticDateAsync(theDate);
        }

        public static void LoadSampleData()
        {
            Init();

            //add a term and a course
            Term theTerm = new Term
            {
                Title = "Fall 2036",
                StartDate = new DateTime(2036, 9, 1),
                EndDate = new DateTime(2036, 11, 30)
            };
            int termId = AddTerm(theTerm);

            Instructor theInstructor = new Instructor
            {
                Name = "Robert Dunnett",
                Email = "rdunne3@wgu.edu",
                Phone = "(209)642-0644"
            };
            int instructorId = AddInstructor(theInstructor);

            Course theCourse = new Course
            {
                Title = "Math 202",
                InstructorId = instructorId,
                TermId = termId,
                StartDate = new DateTime(2036, 9, 1),
                EndDate = new DateTime(2036, 11, 30),
                Status = "Enrolled",
                OptionalNotes = "This is some sample optional notes.",
                Notifications = true
            };
            int courseId = AddCourse(theCourse);

            Assessment performanceAssessment = new Assessment
            {
                CourseId = courseId,
                Title = "C# Application",
                DueDate = new DateTime(2036, 11, 30),
                Type = "Performance",
                Notifications = true
            };
            AddAssessment(performanceAssessment);

            Assessment objectiveAssessment = new Assessment
            {
                CourseId = courseId,
                Title = "Online Exam",
                DueDate = new DateTime(2036, 11, 30),
                Type = "Objective",
                Notifications = false
            };
            AddAssessment(objectiveAssessment);

            RomanticDate theDate = new RomanticDate
            {
                StartDateAndTime = new DateTime(2036, 9, 20, 17, 0, 0),
                Location = "Bob Dog's Pizza",
                Name = "Kathryn",
                PhoneNumber = "209-343-3955",
                Title = "Date With Kathryn"
            };
            _ = AddRomanticDate(theDate);
        }

        public static async Task ClearDatabaseAsync()
        {
            await InitAsync();
            await _dbAsync.DropTableAsync<Assessment>();
            await _dbAsync.DropTableAsync<Instructor>();
            await _dbAsync.DropTableAsync<Course>();
            await _dbAsync.DropTableAsync<Term>();
            await _dbAsync.DropTableAsync<RomanticDate>();
            _db = null;
            _dbAsync = null;
        }

        public static List<Assessment> GetAssessmentsDueToday()
        {
            Init();
            List<Assessment> assessmentNotifications = _db.Table<Assessment>().Where(a => a.Notifications && a.DueDate == DateTime.Today).ToList();
            return assessmentNotifications;
        }

        public static List<Course> GetCoursesThatBeginToday()
        {
            Init();
            List<Course> courses = _db.Table<Course>().Where(c => c.Notifications == true && c.StartDate == DateTime.Today).ToList();
            return courses;
        }

        public static List<Course> GetCoursesThatEndToday()
        {
            Init();
            List<Course> courses = _db.Table<Course>().Where(c => c.Notifications && c.EndDate == DateTime.Today).ToList();
            return courses;
        }
        public static List<RomanticDate> GetDatesToday()
        {
            Init();
            DateTime yesterday = DateTime.Today.AddDays(-1).AddHours(23).AddMinutes(59).AddSeconds(59);
            DateTime tomorrow = DateTime.Today.AddDays(1);

            List<RomanticDate> dates = _db.Table<RomanticDate>().Where(d => d.StartDateAndTime > yesterday &&
            d.StartDateAndTime < tomorrow).ToList();
            return dates;
        }

        #endregion
    }
}