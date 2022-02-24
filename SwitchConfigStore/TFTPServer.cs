using Tftp.Net;
using System.Net;

namespace SwitchConfigStore
{
    public class TFTPServer
    {
        public Dictionary<string, MemoryStream> Configs = new();
        public TftpServer Server = new();
        public TFTPServer()
        {
            Server.Start();
            Server.OnWriteRequest += Server_OnWriteRequest;
        }
        private void Server_OnWriteRequest(ITftpTransfer transfer, EndPoint client)
        {
            transfer.UserContext = new KeyValuePair<string, MemoryStream>(((IPEndPoint)client).Address.ToString(), new());
            transfer.Start(((KeyValuePair<string, MemoryStream>)transfer.UserContext).Value);
            transfer.OnFinished += Transfer_OnFinished;
        }
        private void Transfer_OnFinished(ITftpTransfer transfer)
        {
            Configs[((KeyValuePair<string, MemoryStream>)transfer.UserContext).Key] = ((KeyValuePair<string, MemoryStream>)transfer.UserContext).Value;
        }
    }
}
