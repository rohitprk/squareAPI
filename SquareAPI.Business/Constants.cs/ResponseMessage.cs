namespace SquareAPI.Business.Constants
{
    /// <summary>
    /// Class to hold all response messages at one place.
    /// </summary>
    public class ResponseMessage
    {
        public const string ExceptionOccurred = "Exception occurred!";
        public const string NoPointDataToAdd = "No point data to add.";
        public const string RecordsInsertSuccess = "Record/s inserted Successfully!";
        public const string RecordsInsertFail = "No point data to add.";
        public const string FileUploadSuccess = "Uploaded file successfully!";
        public const string FileInvalid = "No data or invalid file type! Only CSV format file supported.";
        public const string RecordsDeleteSuccess = "Record/s deleted Successfully!";

        public const string RecordsDeleteFail = "No points data to delete.";
        public const string NoPointsData = "No points data found for user.";
        public const string NoPointDataForSquare = "No points to form square.";
        public const string InvalidCredentials = "Invalid user credentials.";
        public const string UserRegisterSuccess = "Registered user successfully!";
        public const string UserAlreadyExists = "User already exists.";
        public const string UnauthorizedAccess = "Unauthorized Access.";
    }
}

