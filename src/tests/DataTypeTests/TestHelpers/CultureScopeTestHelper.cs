using System;
using System.Globalization;

namespace DataTypeTests.TestHelpers
{
    internal sealed class CultureScopeTestHelper : IDisposable
    {
        private readonly CultureInfo _previousCulture;
        private readonly CultureInfo _previousUiCulture;

        internal CultureScopeTestHelper(string cultureName)
        {
            _previousCulture = CultureInfo.CurrentCulture;
            _previousUiCulture = CultureInfo.CurrentUICulture;

            var culture = new CultureInfo(cultureName, false);
            CultureInfo.CurrentCulture = culture;
            CultureInfo.CurrentUICulture = culture;
        }

        public void Dispose()
        {
            CultureInfo.CurrentCulture = _previousCulture;
            CultureInfo.CurrentUICulture = _previousUiCulture;
        }
    }
}
