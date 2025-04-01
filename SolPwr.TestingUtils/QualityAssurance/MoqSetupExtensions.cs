using Microsoft.Extensions.Logging;
using Moq;
using OnionDlx.SolPwr.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionDlx.SolPwr.QualityAssurance
{
    public static class MoqRepoSetupExtensions
    {
        private static Mock<IBusinessObjectCollection<TElement>>
            WithAsyncData<TElement>(this Mock<IBusinessObjectCollection<TElement>> mock, AsyncMockCollection<TElement> data)
            where TElement : IBusinessObject
        {
            mock.Setup(p => p.GetEnumerator()).Returns(data.GetEnumerator());
            mock.Setup(p => p.GetAsyncEnumerator(It.IsAny<CancellationToken>())).Returns(data.GetAsyncEnumerator());

            return mock;
        }


        public static Mock<IBusinessObjectCollection<TElement>>
           WithAsyncData<TElement>(this Mock<IBusinessObjectCollection<TElement>> mock, IList<TElement> data)
           where TElement : IBusinessObject
        {
            var asyncData = new AsyncMockCollection<TElement>(data);
            return mock.WithAsyncData(asyncData);
        }
    }


    public static class MoqLoggerSetupExtensions
    {
        public static Mock<ILogger<IPlantManagementService>> 
            VerifyFor<IPlantManagementService>(this Mock<ILogger<IPlantManagementService>> loggerMock, string pattern)
        {
            loggerMock.Verify(logger => logger.Log(
              It.Is<LogLevel>(l => true),
              It.IsAny<EventId>(),
              It.Is<It.IsAnyType>((v, t) => v.ToString().Contains(pattern)),
              It.IsAny<Exception>(),
              It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)));

            return loggerMock;
        }
    }


    public static class VerifyLoggingUtil
    {
        public static Mock<ILogger<T>> SetupLogging<T>(this Mock<ILogger<T>> logger)
        {
            Func<object, Type, bool> state = (v, t) => true;
            logger.Setup(
                x => x.Log(
                    It.Is<LogLevel>(l => true),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => state(v, t)),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)));
            return logger;
        }
    }
}
