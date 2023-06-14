
using System;
using System.Net.Sockets;
using System.Text;

class Tesira
{
    static void Main()
    {

        try{
        // Create and connect a TcpClient to the server
        // the real ip address needs to be added here
        TcpClient client = new TcpClient("120.0.0.1", 23);

        // Get the client steam for reading and writing
        NetworkStream stream = client.GetStream();

        // Telnet negotiation
        TelnetNegotiation(stream);

        //  Biamp Tesira Level Control
        BiampTesiraLevelControl(stream);

        // Close the network stream and TcpClient
        stream.Close();
        client.Close();
        }
        catch(Exception ex)
        {
            Console.WriteLine("error occured" + ex.Message);
        }
    }

    static void TelnetNegotiation(NetworkStream stream)
    {
        // Send negotiation responses to reject all options
        NegotiationResponse(stream, TelnetCommands.DO, TelnetOptions.BINARY_TRANSMISSION, TelnetCommands.WONT);
        NegotiationResponse(stream, TelnetCommands.DO, TelnetOptions.SUPPRESS_GO_AHEAD, TelnetCommands.WONT);
        NegotiationResponse(stream, TelnetCommands.DO, TelnetOptions.ECHO, TelnetCommands.WONT);
    }

    //Level1 increment level 3 2 and Level1 decrement level 3 2 are the command strings from tesira api
    static void BiampTesiraLevelControl(NetworkStream stream)
    {
        while (true)
        {
            Console.WriteLine("Biamp Tesira Level Control");
            Console.WriteLine("Enter 'UP' or 'DOWN' to adjust the level");
            Console.WriteLine("Enter 'QUIT' to exit");
            string userInput = Console.ReadLine();

            if (userInput.Equals("UP", StringComparison.OrdinalIgnoreCase))
            {
                SendCommand(stream, "Level1 increment level 3 2");
            }
            else if (userInput.Equals("DOWN", StringComparison.OrdinalIgnoreCase))
            {
                SendCommand(stream, "Level1 decrement level 3 2");
            }
            else if (userInput.Equals("QUIT", StringComparison.OrdinalIgnoreCase))
            {
                break;
            }
            else
            {
                Console.WriteLine("Invalid command. Please try again.");
            }
        }
    }

    static void SendCommand(NetworkStream stream, string command)
    {
        try{

        
        byte[] data = Encoding.ASCII.GetBytes(command + "\r\n");
        stream.Write(data, 0, data.Length);

        // Receive the response
        byte[] buffer = new byte[stream.Socket.ReceiveBufferSize];
        int bytesRead = stream.Read(buffer, 0, buffer.Length);
        string response = Encoding.ASCII.GetString(buffer, 0, bytesRead);


        Console.WriteLine(response);
        }
        catch(Exception ex ){
            Console.WriteLine("error occured while sending the command" + ex.Message);
        }
    }

    static void NegotiationResponse(NetworkStream stream, byte command1, byte option, byte command2)
    {
        try{

        
        byte[] responseBytes = new byte[] { TelnetCommands.IAC, command1, option, TelnetCommands.IAC, command2, option };
        stream.Write(responseBytes, 0, responseBytes.Length);
        stream.Flush();
        }
        catch (Exception ex){
            Console.WriteLine("An error occurde while sending negotiation" + ex.Message);

        }
    }
}

class TelnetCommands
{
    public const byte IAC = 255;
    public const byte WILL = 251;
    public const byte DO = 253;
    public const byte WONT = 252;
    public const byte DONT = 254;
}

class TelnetOptions
{
    public const byte BINARY_TRANSMISSION = 0;
    public const byte SUPPRESS_GO_AHEAD = 3;
    public const byte ECHO = 1;
    public const byte STATUS = 5;
    public const byte TERMINAL_TYPE = 24;
}

