using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzR.Extensions.Domain.Validation;

namespace DataTypeTests.UtilsTests.Ensure
{
    [TestClass]
    public class DomainEnsureMessageTests
    {
        private const string CallerMessage = "Caller supplied message";

        [TestMethod]
        public void IsNotNullAll_NullItemKeepsMessage_Test()
        {
            var exception = Assert.ThrowsException<ArgumentNullException>(
                () => DomainEnsure.IsNotNullAll(CallerMessage, 1, null));

            StringAssert.StartsWith(exception.Message, CallerMessage);
            Assert.IsNull(exception.ParamName);
        }

        [TestMethod]
        public void IsNotNullAll_NullMessageUsesDefaultMessage_Test()
        {
            var exception = Assert.ThrowsException<ArgumentNullException>(
                () => DomainEnsure.IsNotNullAll(null, 1, null));

            StringAssert.StartsWith(exception.Message, "Cannot be null!");
        }

        [TestMethod]
        public void IsNotNull_NullInnerExceptionKeepsMessage_Test()
        {
            object source = null;

            var exception = Assert.ThrowsException<ArgumentNullException>(
                () => DomainEnsure.IsNotNull(source, CallerMessage, (Exception)null));

            StringAssert.StartsWith(exception.Message, CallerMessage);
            Assert.IsNull(exception.ParamName);
            Assert.IsNull(exception.InnerException);
        }

        [TestMethod]
        public void IsNotNull_WithInnerExceptionKeepsMessageAndInner_Test()
        {
            object source = null;
            var inner = new InvalidOperationException("inner");

            var exception = Assert.ThrowsException<ArgumentNullException>(
                () => DomainEnsure.IsNotNull(source, CallerMessage, inner));

            StringAssert.StartsWith(exception.Message, CallerMessage);
            Assert.AreSame(inner, exception.InnerException);
        }

        [TestMethod]
        public void ThrowException_ArgumentNullKeepsMessage_Test()
        {
            var exception = Assert.ThrowsException<ArgumentNullException>(
                () => DomainEnsure.ThrowException(CallerMessage, ExceptionType.ArgumentNullException));

            StringAssert.StartsWith(exception.Message, CallerMessage);
            Assert.IsNull(exception.ParamName);
        }

        [TestMethod]
        public void ThrowException_ArgumentOutOfRangeKeepsMessage_Test()
        {
            var exception = Assert.ThrowsException<ArgumentOutOfRangeException>(
                () => DomainEnsure.ThrowException(CallerMessage, ExceptionType.ArgumentOutOfRangeException));

            StringAssert.StartsWith(exception.Message, CallerMessage);
            Assert.IsNull(exception.ParamName);
            Assert.IsNull(exception.ActualValue);
        }

        [TestMethod]
        public void ThrowException_ArgumentNullWithInnerKeepsInner_Test()
        {
            var inner = new InvalidOperationException("inner");

            var exception = Assert.ThrowsException<ArgumentNullException>(
                () => DomainEnsure.ThrowException(CallerMessage, ExceptionType.ArgumentNullException, inner));

            StringAssert.StartsWith(exception.Message, CallerMessage);
            Assert.AreSame(inner, exception.InnerException);
        }

        [TestMethod]
        public void ThrowException_ArgumentOutOfRangeWithInnerKeepsInner_Test()
        {
            var inner = new InvalidOperationException("inner");

            var exception = Assert.ThrowsException<ArgumentOutOfRangeException>(
                () => DomainEnsure.ThrowException(CallerMessage, ExceptionType.ArgumentOutOfRangeException, inner));

            StringAssert.StartsWith(exception.Message, CallerMessage);
            Assert.AreSame(inner, exception.InnerException);
        }

        [TestMethod]
        public void ThrowExceptionIfFuncIsFalse_ArgNullNoParamKeepsMessage_Test()
        {
            var exception = Assert.ThrowsException<ArgumentNullException>(
                () => DomainEnsure.ThrowExceptionIfFuncIsFalse(
                    ExceptionType.ArgumentNullException, () => false, CallerMessage, null));

            StringAssert.StartsWith(exception.Message, CallerMessage);
            Assert.IsNull(exception.ParamName);
        }

        [TestMethod]
        public void ThrowExceptionIfFuncIsFalse_ArgOutOfRangeNoParamKeepsMessage_Test()
        {
            var exception = Assert.ThrowsException<ArgumentOutOfRangeException>(
                () => DomainEnsure.ThrowExceptionIfFuncIsFalse(
                    ExceptionType.ArgumentOutOfRangeException, () => false, CallerMessage, null));

            StringAssert.StartsWith(exception.Message, CallerMessage);
            Assert.IsNull(exception.ParamName);
            Assert.IsNull(exception.ActualValue);
        }

        [TestMethod]
        public void ThrowExceptionIfFuncIsTrue_ArgNullNoParamKeepsMessage_Test()
        {
            var exception = Assert.ThrowsException<ArgumentNullException>(
                () => DomainEnsure.ThrowExceptionIfFuncIsTrue(
                    ExceptionType.ArgumentNullException, () => true, CallerMessage, null));

            StringAssert.StartsWith(exception.Message, CallerMessage);
            Assert.IsNull(exception.ParamName);
        }

        [TestMethod]
        public void ThrowExceptionIfFuncIsTrue_ArgOutOfRangeNoParamKeepsMessage_Test()
        {
            var exception = Assert.ThrowsException<ArgumentOutOfRangeException>(
                () => DomainEnsure.ThrowExceptionIfFuncIsTrue(
                    ExceptionType.ArgumentOutOfRangeException, () => true, CallerMessage, null));

            StringAssert.StartsWith(exception.Message, CallerMessage);
            Assert.IsNull(exception.ParamName);
            Assert.IsNull(exception.ActualValue);
        }

        [TestMethod]
        public void IsNotNull_ParamNameOnlySetsParamName_Test()
        {
            object source = null;

            var exception = Assert.ThrowsException<ArgumentNullException>(
                () => DomainEnsure.IsNotNull(source, "source"));

            Assert.AreEqual("source", exception.ParamName);
        }

        [TestMethod]
        public void IsNotNull_ParamNameAndMessageSetsBoth_Test()
        {
            object source = null;

            var exception = Assert.ThrowsException<ArgumentNullException>(
                () => DomainEnsure.IsNotNull(source, "source", CallerMessage));

            Assert.AreEqual("source", exception.ParamName);
            StringAssert.StartsWith(exception.Message, CallerMessage);
        }

        [TestMethod]
        public void IsNotNullOrEmptyArgNull_EmptyWithParamSetsBoth_Test()
        {
            var exception = Assert.ThrowsException<ArgumentNullException>(
                () => DomainEnsure.IsNotNullOrEmptyArgNull(string.Empty, "source", CallerMessage));

            Assert.AreEqual("source", exception.ParamName);
            StringAssert.StartsWith(exception.Message, CallerMessage);
        }

        [TestMethod]
        public void ThrowExceptionIfFuncIsFalse_ArgOutOfRangeWithParamSetsBoth_Test()
        {
            var exception = Assert.ThrowsException<ArgumentOutOfRangeException>(
                () => DomainEnsure.ThrowExceptionIfFuncIsFalse(
                    ExceptionType.ArgumentOutOfRangeException, () => false, CallerMessage, "source"));

            Assert.AreEqual("source", exception.ParamName);
            StringAssert.StartsWith(exception.Message, CallerMessage);
        }

        [TestMethod]
        public void ThrowExceptionIfFuncIsTrue_ArgNullWithParamSetsBoth_Test()
        {
            var exception = Assert.ThrowsException<ArgumentNullException>(
                () => DomainEnsure.ThrowExceptionIfFuncIsTrue(
                    ExceptionType.ArgumentNullException, () => true, CallerMessage, "source"));

            Assert.AreEqual("source", exception.ParamName);
            StringAssert.StartsWith(exception.Message, CallerMessage);
        }

        [TestMethod]
        public void ThrowExceptionIfFuncIsFalse_ArgNullParamNoMessageSetsParamName_Test()
        {
            var exception = Assert.ThrowsException<ArgumentNullException>(
                () => DomainEnsure.ThrowExceptionIfFuncIsFalse(
                    ExceptionType.ArgumentNullException, () => false, null, "source"));

            Assert.AreEqual("source", exception.ParamName);
        }

        [TestMethod]
        public void ThrowExceptionIfFuncIsTrue_WhitespaceMessageUsesDefault_Test()
        {
            var exception = Assert.ThrowsException<ArgumentNullException>(
                () => DomainEnsure.ThrowExceptionIfFuncIsTrue(
                    ExceptionType.ArgumentNullException, () => true, "   ", null));

            Assert.AreEqual(new ArgumentNullException().Message, exception.Message);
            Assert.IsNull(exception.ParamName);
        }
    }
}
