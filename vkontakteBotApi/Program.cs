using VkNet;
using VkNet.Enums.Filters;
using VkNet.Enums.SafetyEnums;
using VkNet.Exception;
using VkNet.Model;
using VkNet.Model.Keyboard;
using VkNet.Model.RequestParams;

namespace vkontakteBotApi
{
    internal class Program
    {
        public static VkApi vv = new VkApi();
        static void Main(string[] args)
        {
            vv.Authorize(new ApiAuthParams
            {
                AccessToken = "vk1.a.MGmNV4-qonDbA2LASN6KxGxFr8UONSkrM0ggiYFAoo-qJB3sTQxOm9ErxeiPxDvDZNQz6BROJxh811fwHmHLbGiwssd7hvlA2hQNxIO9OH2_USi2pMi0385g5hWJffeXE5Y-ELf4_Kmw43KZx1EjJ9xb2rg7dFJxmEaeCeahyH_lj7bT1JQ_snDSefjU9E2lCoCwMl41JLinIf9dNfLjCA",
                Settings = Settings.All
            });
            while (true) 
            {
                Thread.Sleep(1000);
                Reseive();
            }
        }
        public static bool Reseive()
        {
            object[] minfo = GetMessage();
            Console.WriteLine(minfo[0] + "\t" + minfo[1].ToString() + "\t" + minfo[2]);
            int userId = Convert.ToInt32(minfo[2]);
            var messages = vv.Messages.GetConversations(new GetConversationsParams
            {
                Count = 10
            }); 
            var messageText = messages.Items[0].LastMessage.Text.ToString();
            if (minfo[0] == null)
            { return false; }
            else
            {
                Console.WriteLine(123);
                KeyboardBuilder key = new KeyboardBuilder();
                string code = "";
                if (minfo[1].ToString() != "")
                {
                    string[] codeSplit = minfo[1].ToString().Split("button\":\"");
                    string[] splittedSplitCode = codeSplit[1].Split("\"}");
                    code = splittedSplitCode[0].ToString();
                    Console.WriteLine(code);
                }
                else
                {
                    code = "";
                }
                switch (code)
                {
                    case "1":
                        key.AddButton("Меню", "menu", KeyboardButtonColor.Positive);
                        key.AddLine();
                        key.AddButton("Меня зовут Даня", "myName", KeyboardButtonColor.Negative);
                        key.AddLine();
                        SendMessage("Выберите необходимую кнопку:", userId, key.Build());
                        break;
                    case "menu":
                        SendMessage("Меню находится в разработке!", userId, null);
                        break;
                    case "myName":
                        SendMessage("Я рад с вами познакомиться !", userId, null);
                        break;
                    default:
                        SendMessage(("Вы написали: " + messageText), userId, null);
                        break;
                }
                return true;
            }
        }

        public static void SendMessage(string message, int userId, MessageKeyboard keyboard)
        {
            vv.Messages.Send(new MessagesSendParams
            {
                Message = message,
                UserId = userId,
                RandomId = new Random().Next(),
                Keyboard = keyboard
            });
        }
        public static object[] GetMessage()
        {
            string message = "";
            string keyname = "";
            long? userId = 0;
            var messages = vv.Messages.GetConversations(new GetConversationsParams
            {
                Count = 10
            });
            if (messages.Count != 0)
            {
                if (messages.Items[0].LastMessage.ToString() != "" && messages.Items[0].LastMessage.FromId != -99714091) {
                    message = messages.Items[0].LastMessage.ToString();
                }
                else { message = ""; }
                if (messages.Items[0].LastMessage.FromId != -99714091)
                {
                    userId = messages.Items[0].LastMessage.FromId;
                }
                else { userId = 0; }
                if (messages.Items[0].LastMessage.FromId != -99714091 && messages.Items[0].LastMessage.Payload != null)
                {
                    keyname = messages.Items[0].LastMessage.Payload.ToString();
                }
                else { keyname = ""; }
                return new object[] { message, keyname, userId };
            }
            else
            {
                return new object[] { null, null, null };
            }
        }
    }
}