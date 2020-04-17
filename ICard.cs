using CardInterface;
using System.Collections.Generic;

public interface ICard
{
	ResultObj Execute(Dictionary<string, string> args);
}
