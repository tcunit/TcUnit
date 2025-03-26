using EnvDTE80;
using System;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace TcUnit.Verifier
{
    class ErrorList : IEnumerable<ErrorList.Error>, IEnumerable
    {
        protected List<Error> _errors = new List<Error>();

        public struct Error
        {
            public string Description;
            public vsBuildErrorLevel ErrorLevel;
            public DateTime Timestamp;

            public Error(ErrorItem item)
            {
                Description = item.Description.ToUpper();
                ErrorLevel = item.ErrorLevel;
                Timestamp = ParseTimestampFromDescription(item.Description);
            }
        }

        static public DateTime ParseTimestampFromDescription(string description)
        {
            string pattern = @"^(.*?)\s+(\d+\s)ms\s+\|";
            Match match = Regex.Match(description, pattern, RegexOptions.IgnoreCase);
            var parsedDate = DateTime.Parse(match.Groups[1].Value, CultureInfo.CurrentCulture).AddMilliseconds(double.Parse(match.Groups[2].Value));
            return parsedDate;
        }

        public IEnumerable<Error> AddNew(ErrorItems errorItems)
        {
            int N1 = _errors.Count + 1;
            int N2 = errorItems.Count;
            for (int i = N1; i <= N2; i++)
            {
                var item = errorItems.Item(i);
                _errors.Add(new Error(item));
            }
            return _errors.GetRange(N1 - 1, N2 + 1 - N1);
        }

        IEnumerator<Error> IEnumerable<Error>.GetEnumerator() {
            return _errors.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _errors.GetEnumerator();
        }
    }
}