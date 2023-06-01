using System.Globalization;

namespace Health.Mobile.Server.Constants
{
    public static class ApiPath
    {
        public const string BaseUrl  = "https://localhost:7241/api";
        public const string LabApi = "/Lab";
        public const string LabTestCategoryApi = "/LabTest/TestCategory";
        public const string LoginApi = "/Account/Login";
        public const string RefreshTokenApi = "/Account/refresh-token";
        public const string RegisterUserApi = "/Account/Register";
        public const string RegisterUserByAdminApi = "/Account/RegisterByRole";
        public const string ChangePasswordApi = "/Account/ChangePassword";
        public const string AccountGetUsersApi = "/Account/GetUsers";
        public const string AccountUpdateUserApi = "/Account/UpdateUser";
        public const string AccountDeleteUserApi = "/Account/DeleteUser";

        public const string LabCategoryApi = "/Category";
        public const string LabTestApi = "/LabTest";
        public const string LabTestPriceApi = "/TestPrice";
        public const string SampleTypeApi = "/SampleType";
        public const string TubeTypeApi = "/TubeType";
        public const string TestResultApi = "/TestResult";
        public const string SelectedTestDetailApi = "/SelectedTestDetail";
        public const string SelectedTestStatusApi = "/SelectedTestStatus";
        public const string UserAssignApi = "/UserAssign";
        public const string UserBranchApi = "/UserBranch";
        public const string PaymentProviderApi = "/PaymentProvider";
        public const string PatientFileApi = "/PatientFile";
        public const string OrganizationApi = "/Organization";
        public const string BankAccountApi = "/BankAccount";
    }
}
