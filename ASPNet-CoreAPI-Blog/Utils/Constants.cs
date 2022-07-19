namespace ASPNet_CoreAPI_Blog.Utils
{
    public class Constants
    {
        // role 
        public static string ROLE_ADMIN = "Admin";
        public static string ROLE_USER = "User";
        public static string ROLE_SUPPER_ADMIN = "Supper-Admin";
        
        //custom request 
        public static int SUCCESS_CODE = 200;
        public static bool SUCCESS_STATUS=true;
        public static string SUCCESS_MESSAGE = "Request success !";
        public static int ERROR_CODE = 400;
        public static bool ERROR_STATUS = false;
        public static string ERROR_MESSAGE = "Server error !";
        
        // Login message and code
        public static string LOGIN_ERROR = "Your account or password is not correct. Please enter again !";
        public static string LOGIN_SUCCESS = "Login success !";
        public static string REGISTER_SUCCESS = "Register success !";
    }
}
