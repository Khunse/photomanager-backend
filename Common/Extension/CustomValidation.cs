using imageuploadandmanagementsystem.Model;

namespace imageuploadandmanagementsystem.Common.Extension
{
    public static class CustomValidation
    {
        public static bool ValidateRegisterRequestModel(this RegisterRequest requestModel)
        {
            var isValid = false;
            if(requestModel is null) return isValid;
            if(string.IsNullOrEmpty(requestModel.Email) || string.IsNullOrEmpty(requestModel.Password) || string.IsNullOrEmpty(requestModel.ConfirmPassword)) return isValid;
            if(requestModel.Password != requestModel.ConfirmPassword) return isValid;

            return true;
        }
    }
}