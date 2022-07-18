using ASPNet_CoreAPI_Blog.Models.DTO;

namespace ASPNet_CoreAPI_Blog.Respository
{
    public interface IBlogRepository:IDisposable
    {
        IEnumerable<Object> GetAllBlogs();
        Object GetBlogById(int id);
        Object CreateBlog(BlogDTO blogDTO, string listpos);
        Object EditBlogById(int id, BlogDTO blogDTO,string listpos);
        Object DeleteBlogById(int id);
        void Save();

        /*IEnumerable<Student> GetStudents();
        Student GetStudentByID(int studentId);
        void InsertStudent(Student student);
        void DeleteStudent(int studentID);
        void UpdateStudent(Student student);
        void Save();*/
    }
}
