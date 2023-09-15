using Moq;
using OtelDemo.Common.OpenTelemetry;
using Serilog;

namespace OtelDemo.Inscricoes.IntegrationTests;

public class SerilogMock
{
    public static Mock<ILogger> Create()
    {
        var logger = new Mock<ILogger>();
        logger.Setup(x => x.Verbose(It.IsAny<Exception>(), It.IsAny<string>()));
        logger.Setup(x => x.Information(It.IsAny<Exception>(), It.IsAny<string>()));
        logger.Setup(x => x.Information(It.IsAny<string>(), It.IsAny<string>()));
        logger.Setup(x => x.Error(It.IsAny<Exception>(), It.IsAny<string>()));
        logger.Setup(x => x.Error(It.IsAny<string>(), It.IsAny<string>()));
        logger.Setup(x => x.Error(It.IsAny<string>()));
        logger.Setup(x => x.ForContext<object>()).Returns(logger.Object);
        logger.Setup(x => x.ForContext(It.IsAny<string>(), It.IsAny<object>(), false)).Returns(logger.Object);

        return logger;
    }
}

public class TelemetryMockFactory
{
    public static ITelemetryFactory Create()
    {
        var tramontinaTelemetry = new Mock<ITelemetryService>();
        tramontinaTelemetry.Setup(x => x.AddTag(It.IsAny<string>(), It.IsAny<object?>()))
            .Returns(tramontinaTelemetry.Object);
        tramontinaTelemetry.Setup(x => x.AddLogInformationAndEvent(It.IsAny<string>(), It.IsAny<object?>()))
            .Returns(tramontinaTelemetry.Object);
        tramontinaTelemetry.Setup(x => x.AddWarningEvent(It.IsAny<string>(), It.IsAny<object?>()))
            .Returns(tramontinaTelemetry.Object);
        tramontinaTelemetry.Setup(x => x.AddException(It.IsAny<string>(), It.IsAny<Exception>()))
            .Returns(tramontinaTelemetry.Object);
        
        var tramontinaTelemetryFactory = new Mock<ITelemetryFactory>();
        tramontinaTelemetryFactory
            .Setup(c=> c.Create(It.IsAny<string>()))
            .Returns(tramontinaTelemetry.Object);
        return tramontinaTelemetryFactory.Object;
    }
}