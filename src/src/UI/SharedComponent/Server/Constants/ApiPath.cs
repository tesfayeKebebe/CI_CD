using System.Globalization;

namespace SharedComponent.Server.Constants
{
    public static class ApiPath
    {
        // public const string BaseUrl = "https://9559-196-190-60-127.eu.ngrok.io/api";
        // public const string BaseForAndroidUrl = "https://9559-196-190-60-127.eu.ngrok.io/api";
        public const string BaseUrl  = "http://localhost:5026";
        //public const string BaseUrl = "http://196.189.119.57:4030";
        public const string LabApi = "/api/Lab";
        public const string LabTestCategoryApi = "/api/LabTest/TestCategory";
        public const string LoginApi = "/api/Account/Login";
        public const string RefreshTokenApi = "/api/Account/refresh-token";
        public const string RegisterUserApi = "/api/Account/Register";
        public const string RegisterUserByAdminApi = "/api/Account/RegisterByRole";
        public const string ChangePasswordApi = "/api/Account/ChangePassword";
        public const string AccountGetUsersApi = "/api/Account/GetUsers";
        public const string AccountUpdateUserApi = "/api/Account/UpdateUser";
        public const string AccountDeleteUserApi = "/api/Account/DeleteUser";
        
        public const string LabCategoryApi = "/api/Category";
        public const string LabTestApi = "/api/LabTest";
        public const string LabTestPriceApi = "/api/TestPrice";
        public const string SampleTypeApi = "/api/SampleType";
        public const string TubeTypeApi = "/api/TubeType";
        public const string TestResultApi = "/api/TestResult";
        public const string  SelectedTestDetailApi = "/api/SelectedTestDetail";
        public const string SelectedTestStatusApi = "/api/SelectedTestStatus";
        public const string  UserAssignApi = "/api/UserAssign";
        public const string UserBranchApi = "/api/UserBranch";
        public const string PaymentProviderApi = "/api/PaymentProvider";
        public const string PatientFileApi = "/api/PatientFile";
        public const string OrganizationApi = "/api/Organization";
        public const string BankAccountApi = "/api/BankAccount";
    }
}
