using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MySocketServer2
{
  /// <summary>
  /// msg like [MSG=JOSN/XML/NONE]
  /// </summary>
  public class SocketRequestHandler
  {
    private static string temp = string.Empty;

    public static SocketRequestMessage GetActualString(string input, List<string> outputList = null)
    {
      if (outputList == null)
        outputList = new List<string>();

      if (!string.IsNullOrEmpty(temp))
        input = temp + input;

      string output = "";
      string lenStr = @"(?<=^\[length=)(\d+)(?=\])";
      string pattern = @"(\[MSG=)(JSON|XML|NONE|REQUEST)(?=\])";
      int length = 0;
      SocketMessageInfoType _type = CheckMessageInfoType(input);

      if (Regex.IsMatch(input, lenStr))
      {
        Match lenRex = Regex.Match(input, lenStr);
        length = Convert.ToInt32(lenRex.Groups[0].Value);

        if (Regex.IsMatch(input, pattern))
        {
          Match m = Regex.Match(input, pattern);
          int startIndex = input.IndexOf(m.Groups[0].Value + "]") + m.Groups[0].Value.Length + 1;
          output = input.Substring(startIndex);

          if (output.Length == length)
          {
            outputList.Add(output);
            temp = "";
          }
          else if (output.Length < length)
          {
            temp = input;
          }
          else if (output.Length > length)
          {
            output = output.Substring(0, length);
            outputList.Add(output);
            temp = "";
            input = input.Substring(startIndex + length);
            GetActualString(input, outputList);
          }
        }
      }
      else
        temp = input;

      SocketRequestMessage message = new SocketRequestMessage()
      {
        MessageLength = length,
        MessageType = _type,
        Messages = outputList.ToArray()
      };
      return message;
    }

    public static SocketMessageInfoType CheckMessageInfoType(string input)
    {
      SocketMessageInfoType _type = SocketMessageInfoType.NONE;

      string[] _names = Enum.GetNames(typeof(SocketMessageInfoType));
      foreach (string name in _names)
      {
        if (input.IndexOf(string.Format("[MSG={0}]", name)) >= 0)
          return _type = (SocketMessageInfoType)Enum.Parse(typeof(SocketMessageInfoType), name);
      }
      return _type;
    }
  }

  public struct SocketRequestMessage
  {
    public int MessageLength { get; set; }
    public SocketMessageInfoType MessageType { get; set; }
    public string[] Messages { get; set; }
  }
}
