namespace Application.Helpers
{
    public static class Helpers
    {
        public static int CalculateAge(DateTime birthDate)
        {
            var today = DateTime.Today;
            var age = today.Year - birthDate.Year;
            if (birthDate.Date > today.AddYears(-age)) age--; // Ajustar si aún no ha cumplido años
            return age;
        }
    }
}
