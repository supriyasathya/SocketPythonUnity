
import numpy as np
import socket
import struct

# this script creates a TCP/IP socket and awaits for a client to connect to the socket. Once the clinet is connected, the script sends
# an array with 3 floats to the client code.
# In order to run this script from command line, type: python ServerPython.py 

def main():
    

    #  create a socket object
    s = socket.socket()
    print ("Socket successfully created")

    # reserve a port on your computer in our case it is 2020, it can be anything.

    port = 2020
    s.bind(('', port))
    print ("socket binded to %s" %(port))

    # put the socket into listening mode to listen to any client that will accept the socket connection
    s.listen(5)
    print ("socket is listening")
    c,addr = s.accept()
    print('got connection from ',addr)
    
    #array to be sent
    ArrToSend = np.array([10,20,30])
    print("Arr to send",ArrToSend)
    #pack the array (to be sent) into bytes before sending it
    dataTosend = struct.pack('f'*len(ArrToSend),*ArrToSend)
    #send the bytes
    c.send(dataTosend)
            


if __name__ == '__main__':
    main()
