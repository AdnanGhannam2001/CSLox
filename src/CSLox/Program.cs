using CSLox;
using CSLox.Core.Common;
using CSLox.Core.Exceptions;

try
{
    if (args.Length > 1)
    {
        throw new InvalidUsageException();
    }

    if (args.Length == 1)
    {
        Runner.RunFile(args[0]);
    }
    else
    {
        Runner.RunPrompt();
    }
}
catch (LoxException exp)
{
    Logger.LogError(exp.Message, exp.Details);
}
catch (Exception exp)
{
    Logger.LogFatal("Unexpected exception thrown", exp.Message);

    throw;
}