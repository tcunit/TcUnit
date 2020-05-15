using EnvDTE80;
using System.Collections;
using System.Collections.Generic;

namespace TcUnit.Verifier
{
    class ErrorList : IEnumerable<ErrorList.Error>, IEnumerable
    {
        protected List<Error> _errors = new List<Error>();

        public struct Error
        {
            public string Description;
            public vsBuildErrorLevel ErrorLevel;

            public Error(ErrorItem item)
            {
                Description = item.Description.ToUpper();
                ErrorLevel = item.ErrorLevel;
            }
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