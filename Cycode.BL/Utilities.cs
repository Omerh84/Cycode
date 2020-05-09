using System;
using System.Globalization;
using System.Text.RegularExpressions;
using Cycode.Common.Contracts;

namespace Cycode.BL
{
    public class Utilities
    {
        public static bool IsValidStudentContract(StudentContract studentContract, out string message)
        {
            if (string.IsNullOrEmpty(studentContract.FirstName) || string.IsNullOrEmpty(studentContract.LastName) ||
                !Utilities.IsValidEmail(studentContract.EmailAddress))
            {
                message = "Please fill all field and enter a valid email address";
                return false;
            }
            
            message = String.Empty;
            return true;
        }

        public static bool IsValidGradeContract(GradeContract gradeContract, out string message)
        {
            if (gradeContract.Grade > 100 || gradeContract.Grade < 0)
            {
                message = "Grade should be between 0 to 100";
                return false;
            }

            message = string.Empty;
            return true;
        }

        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                    RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    var domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
            catch (ArgumentException)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email,
                    @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                    @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        public static bool IsValidCourseContract(CourseContract courseContract, out string message)
        {
            var r = new Regex("^[a-zA-Z0-9]*$");
            if (string.IsNullOrEmpty(courseContract.Name) || !r.Match(courseContract.Name).Success)
            {
                message = "Course name is invalid";
                return false;
            }

            message = string.Empty;
            return true;
        }
    }
}