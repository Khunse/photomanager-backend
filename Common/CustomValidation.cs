namespace imageuploadandmanagementsystem.Common
{
    public static class EnvironmentValidator
    {
       public static void ValidateEnvironmentVariables()
       {
            var requiredVariables = new List<string>{
                "AWS_ACCESS_KEY_ID",
                "AWS_SECRET_ACCESS_KEY",
                "JWT_KEY",
                "DB_HOST_NAME",
                "DB_PORT",
                "DB_NAME",
                "DB_USER_NAME",
                "DB_USER_PASSWORD",
            };

            var missingVariables = requiredVariables.Where(variable => string.IsNullOrEmpty(Environment.GetEnvironmentVariable(variable))).ToList();

            if (missingVariables.Count > 0)
            {
                Console.WriteLine($"The following environment variables are not set: {string.Join(", ", missingVariables)}");
                Environment.Exit(1);
            }
        
    }
    }
}