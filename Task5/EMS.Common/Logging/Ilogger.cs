using System.Collections.Generic;

namespace EMS.Common.Logging;

public interface ILogger
{
    void LogInfo(string message);
    void LogError(string message);
    void LogSuccess(string message);
}